using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trace
{
	class Program
	{
		static void Main(string[] args)
		{
			var command = Console.ReadLine();
			var commands = "tracert " + command;
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "cmd.exe",
					RedirectStandardInput = true,
					UseShellExecute = false,
					RedirectStandardOutput = true

				}
			};
			process.Start();

			using (StreamWriter writer = process.StandardInput)
			{
				if (writer.BaseStream.CanWrite)
				{
					foreach (var line in commands.Split('\n'))
						writer.WriteLine(line);
				}
			}
			

			using (StreamReader reader = process.StandardOutput)
			{
				var lines = reader.ReadToEnd().Split(new []{"\r\n"}, StringSplitOptions.None).Skip(8);
				foreach (var line in lines.Where(line => line != ""))
				{
					var ip = line.Split(new[] {" ", "[", "]"}, StringSplitOptions.RemoveEmptyEntries).Last();
					IPAddress address;
					if (!IPAddress.TryParse(ip, out address)) continue;
					Console.WriteLine(line);
					if (ip.StartsWith("192.168") || ip.StartsWith("10."))
					{
						Console.WriteLine("ip: {0}, AS: {1}, country: {2}, netname: {3}\n", ip, "***", "***", "***");
						continue;
					}
					var client = new TcpClient("whois.iana.org", 43);
					var bytes = Encoding.ASCII.GetBytes(ip + "\n");
					client.GetStream().Write(bytes, 0, bytes.Length);
					var bytesRecieve = new byte[1024];
					Thread.Sleep(1000);
					client.GetStream().Read(bytesRecieve, 0, 1024);
					var resultString = Encoding.UTF8.GetString(bytesRecieve);

					//Console.WriteLine(resultString);
					
					var whois = resultString
						.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries)
						.First(s => s.StartsWith("refer"))
						.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
						.Last();

					client = new TcpClient(whois, 43);
					bytes = Encoding.ASCII.GetBytes(ip + "\n");
					client.GetStream().Write(bytes, 0, bytes.Length);
					bytesRecieve = new byte[2048];
					Thread.Sleep(5000);
					client.GetStream().Read(bytesRecieve, 0, 2048);
					resultString = Encoding.UTF8.GetString(bytesRecieve);

					PrintInfo(resultString, ip);
				}
			}
		}

		private static void PrintInfo(string resultString, string ip)
		{
			var strings = resultString.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);

			var origin = Last(strings, "origin").Replace("as", "");

			var country = Last(strings, "country");

			var netName = Last(strings, "netname");
			Console.WriteLine("ip: {0}, AS: {1}, country: {2}, netname: {3}\n", ip, origin, country, netName);
		}

		private static string Last(string[] strings, string key)
		{
			try
			{
				return (strings
					.Select(s => s.ToLower())
					.FirstOrDefault(x => x.StartsWith(key)) ?? "")
					.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
					.Last();
			}
			catch
			{
				if (key == "origin")
					return Last(strings, "originas");
				if (key == "originas")
					return GetAS(strings);
				return "***";
			}
		}

		private static string GetAS(string[] strings)
		{
			try
			{
				int a;
				return strings.SelectMany(s => s.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries))
					.Where(s => s.StartsWith("AS"))
					.Select(s => s.Replace("AS", "").Split('-').First())
					.FirstOrDefault(s => int.TryParse(s, out a)) ?? "***";
			}
			catch
			{
				return "***";
			}
		}

	}
}
