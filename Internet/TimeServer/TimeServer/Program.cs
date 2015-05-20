using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeServer
{
	class Sntp
	{
		public Sntp()
		{ }

		public Sntp(byte[] clientData)
		{
			if (clientData.Length < 48)
				return;
			var firstLine = BitConverter.ToInt32(clientData, 0);

			IndicatorCorrectness = (int) (firstLine/Math.Pow(2, 30));
			firstLine -= IndicatorCorrectness*(int)Math.Pow(2, 30);

			VersionNumber = (int)(firstLine / Math.Pow(2, 27));
			firstLine -= VersionNumber * (int)Math.Pow(2, 27);

			Mode = (int)(firstLine / Math.Pow(2, 24));
			firstLine -= Mode * (int)Math.Pow(2, 24);

			Strata = (int)(firstLine / Math.Pow(2, 16));
			firstLine -= Strata * (int)Math.Pow(2, 16);

			PollingInterval = (int) (firstLine/Math.Pow(2, 8));
			firstLine -= PollingInterval * (int)Math.Pow(2, 8);

			Accuracy = firstLine;

			Delay = BitConverter.ToInt32(clientData, 4);
			Dispersion = BitConverter.ToInt32(clientData, 8);
			SourseIdentifier = BitConverter.ToInt32(clientData, 12);
			UpdateTime = BitConverter.ToUInt32(clientData, 16) + (UInt64)BitConverter.ToUInt32(clientData, 20) << 32;
			StartTime = BitConverter.ToUInt32(clientData, 24) + (UInt64)BitConverter.ToUInt32(clientData, 28) << 32;
			TimeOfReceipt = BitConverter.ToUInt32(clientData, 32) + (UInt64)BitConverter.ToUInt32(clientData, 36) << 32;
			SendingTime = BitConverter.ToUInt32(clientData, 40) + (UInt64)BitConverter.ToUInt32(clientData, 44) << 32;
		}

		public int IndicatorCorrectness { get; set; }
		public int Mode { get; set; }
		public int VersionNumber { get; set; }
		public int Strata { get; set; }
		public int PollingInterval { get; set; }
		public int Accuracy { get; set; }
		public int Delay { get; set; }
		public int Dispersion { get; set; }
		public int SourseIdentifier { get; set; }
		public UInt64 UpdateTime { get; set; }
		public UInt64 StartTime { get; set; }
		public UInt64 TimeOfReceipt { get; set; }
		public UInt64 SendingTime { get; set; }

		public byte[] GetBinary()
		{
			var intPack = new UInt32[12];
			intPack[0] = (UInt32)((UInt32)IndicatorCorrectness * (UInt32)Math.Pow(2, 30) +
						 VersionNumber * Math.Pow(2, 27) +
						 Mode * Math.Pow(2, 24) +
						 Strata * Math.Pow(2, 16) +
						 PollingInterval * Math.Pow(2, 8) +
						 Accuracy);
			intPack[1] = (UInt32)Delay;
			intPack[2] = (UInt32)Dispersion;
			intPack[3] = (UInt32)SourseIdentifier;
			intPack[4] = (UInt32)(UpdateTime >> 32);
			intPack[5] = (UInt32)((UpdateTime << 32) >> 32);
			intPack[6] = (UInt32)(StartTime >> 32);
			intPack[7] = (UInt32)((StartTime << 32) >> 32);
			intPack[8] = (UInt32)(TimeOfReceipt >> 32);
			intPack[9] = (UInt32)((TimeOfReceipt << 32) >> 32);
			intPack[11] = (UInt32)(SendingTime >> 32);
			intPack[10] = (UInt32)(SendingTime << 32) >> 32;

			return intPack.SelectMany(BitConverter.GetBytes).ToArray();
		}

		public static UInt64 GetSntpTime(DateTime time)
		{
			var startTime = new DateTime(1900, 1, 1, 0, 0, 0);
			var delta = time.Subtract(startTime);
			var totalSeconds = (UInt64)delta.TotalSeconds;
			var totalMilliseconds = (UInt64)time.Millisecond;
			var answer = totalSeconds * (UInt64)Math.Pow(2, 32) + totalMilliseconds;
			return answer;
		}

		public static DateTime GetRealTime(UInt64 time)
		{
			var seconds = (UInt32)(time >> 32);
			var milliseconds = (UInt32)(time << 32) >> 32;
			var startTime = new DateTime(1900, 1, 1, 0, 0, 0);

			return startTime.AddSeconds(seconds).AddMilliseconds(milliseconds);
		}
	}


	class Program
	{
		static void Main(string[] args)
		{
			const int port = 123;
			var time = int.Parse(File.ReadAllLines("time.txt").First());
			var server = new Server(port, time);
			server.Start();
			var client = new Client();
			client.Start();
		}

	}

	public class Server
	{
		private readonly int localPort;
		private Thread servThread;
		TcpListener listener;
		private readonly int seconds;

		private const int IndicatorCorrectness = 0;
		private const int Mode = 4;
		private const int VersionNumber = 4;
		private const int Strata = 1;
		private const int PollingInterval = 3;
		private const int Accuracy = 0;
		private const int Delay = 0;
		private const int Dispersion = 0;
		private const int SourseIdentifier = 1;

		public Server(int port, int seconds)
		{
			localPort = port;
			this.seconds = seconds;
		}

		public void Start()
		{
			servThread = new Thread(ServStart);
			servThread.Start();
		}

		public void Close()
		{
			listener.Stop();
			servThread.Abort();
		}

		private void ServStart()
		{
			Task<Socket> clientSock;
			var clientData = new byte[1024];
			listener = new TcpListener(localPort);
			listener.Start();
			try
			{
				clientSock = listener.AcceptSocketAsync();
			}
			catch
			{
				servThread.Abort();
				return;
			}

			var i = 0;

			if (!clientSock.Result.Connected) return;
			while (true)
			{
				try
				{
					i = clientSock.Result.Receive(clientData);
				}
				catch { }

				try
				{
					if (i > 0)
					{
						var parsedData = new Sntp(clientData);
						var currentTime = DateTime.Now;
						var sntp = new Sntp
						{
							IndicatorCorrectness = IndicatorCorrectness,
							VersionNumber = parsedData.VersionNumber,
							Mode = Mode,
							Strata = Strata,
							PollingInterval = PollingInterval,
							Accuracy =Accuracy,
							Delay = Delay,
							Dispersion = Dispersion,
							SourseIdentifier = SourseIdentifier,
							UpdateTime = Sntp.GetSntpTime(DateTime.Now.AddSeconds(seconds)),
							StartTime = Sntp.GetSntpTime(Sntp.GetRealTime(parsedData.SendingTime).AddSeconds(seconds)),
							SendingTime =Sntp.GetSntpTime(DateTime.Now.AddSeconds(seconds)),
							TimeOfReceipt = Sntp.GetSntpTime(currentTime)
						};
						var bytes = sntp.GetBinary();
						var checkSntp = new Sntp(bytes);
						clientSock.Result.Send(bytes, bytes.Length, SocketFlags.None);
					}
				}
				catch
				{
					clientSock.Dispose();//.Close();
					listener.Stop();
					servThread.Abort();
				}
			}
		}

		
	}

	public class Client
	{
		public void Start()
		{
			var client = new TcpClient();
			try
			{
				client.Connect("127.0.0.1", 123);
			}
			catch
			{
				return;
			}
			var sock = client.Client;
			while (true)
			{
				var command = Console.ReadLine();
				if (command == "time")
				{
					byte[] remdata = new byte[1024];
					var sntpSend = new Sntp
					{
						VersionNumber = 4,
						Mode = 3,
						SendingTime = Sntp.GetSntpTime(DateTime.Now)
					};
					var binary = sntpSend.GetBinary();
					sock.Send(binary);
					sock.Receive(remdata);

					var sntp = new Sntp(remdata);

					Console.WriteLine(Sntp.GetRealTime(sntp.StartTime));
				}
			}
		}
	}
}