using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Package;

namespace ServerDNS
{
	public class Server
	{
		private readonly Storage storage;

		private const int DEFAULT_PORT = 53;
		private const int UDP_TIMEOUT = 2000;
		//private const string SERVER_ADDRESS = "8.8.8.8";
		private const string SERVER_ADDRESS = "212.193.163.6";

		public Server()
		{
			storage = Storage.GetStorage();
			var deleteDemon = new Task(() =>
			{
				while (true)
				{
					lock (storage)
					{
						storage.RemoveOld();
					}
				}
			});
			deleteDemon.Start();
		}

		public void StartListen()
		{
			Console.WriteLine("Start listening");
			var udp = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), DEFAULT_PORT));

			var endPoint = new IPEndPoint(IPAddress.Any, DEFAULT_PORT);

			udp.Client.SendTimeout = UDP_TIMEOUT;

			Task.Factory.StartNew(() =>
			{
				AcceptClients(udp, endPoint);
			});
			while (true)
			{
				var line = Console.ReadLine();
				if (line == "exit")
				{
					lock (storage)
					{
						storage.Serialise();
					}
					return;
				}
			}
		}

		private void AcceptClients(UdpClient udp, IPEndPoint endPoint)
		{
			while (true)
			{
				byte[] clientMessage;
				try
				{
					clientMessage = udp.Receive(ref endPoint);
					Console.WriteLine("CONNECTED");
				}
				catch (SocketException)
				{
					continue;
				}

				var taskEndPoint = endPoint;
				Task.Factory.StartNew(() =>
				{
					Console.WriteLine("I'M IN TASK");

					var request = DnsPackage.FromArray(clientMessage);

					DnsPackage response = ResolveRequest(request);
					response.Header.ANCOUNT = (short)(response.Answers.Count);

					response.Header.ARCOUNT = 0;
					response.Header.NSCOUNT = 0;

					var bytes = response.GetBytes();
					udp.Send(bytes, bytes.Length, taskEndPoint);
				});
			}
		}

		private DnsPackage ResolveRequest(DnsPackage request)
		{
			lock (storage)
			{
				if (storage.ContainKey(request.Question))
					return new DnsPackage(request.Header, request.Question, storage.Get(request.Question));
			}
			try
			{
				var answer = ResolveFromServer(request);
				lock (storage)
				{
					storage.Add(request.Question, answer.Answers);
					foreach (var addit in answer.Additionals)
					{
						storage.Add(addit.Question, new List<PackageAnswer> {addit});
					}
					foreach (var a in answer.Auth)
					{
						storage.Add(a.Question, new List<PackageAnswer> { a });
					}
				}
				return answer;
			}
			catch
			{
				throw;
				return request;
			}
		}

		private DnsPackage ResolveFromServer(DnsPackage request)
		{
			UdpClient udp = new UdpClient();
			IPEndPoint dns = new IPEndPoint(IPAddress.Parse(SERVER_ADDRESS), DEFAULT_PORT);
			udp.Client.ReceiveTimeout = UDP_TIMEOUT;

			try
			{
				udp.Connect(dns);
				var bytes = request.GetBytes();
				udp.Send(bytes, bytes.Length);
				byte[] buffer = udp.Receive(ref dns);
				DnsPackage response = DnsPackage.FromArray(buffer); //null;


				return response;
			}
			catch
			{
				throw;
				return ResolveFromServer(request);
			}
			finally
			{
				udp.Close();
			}
		}

		~Server()
		{
			lock (storage)
			{
				storage.Serialise();
			}
		}
	}
}
