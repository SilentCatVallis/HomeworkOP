using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Package;

namespace Client_DNS
{
	public class Client
	{
		private readonly string ip;
		private const int DEFAULT_PORT = 53;
		private const int UDP_TIMEOUT = 2000;

		public Client(string ip)
		{
			this.ip = ip;
		}

		public string Resolve(string domain, string requestType)
		{
			var endPoint = new IPEndPoint(IPAddress.Parse(ip), DEFAULT_PORT);

			var udp = new UdpClient();

			udp.Connect(endPoint);

			udp.Client.SendTimeout = UDP_TIMEOUT;

			var sender = new IPEndPoint(IPAddress.Any, DEFAULT_PORT);

			var packageType = requestType == "NS" ? Package.Type.NS : Package.Type.A;
			DnsPackage request = new DnsPackage(new PackageHeader(), new PackageQuestion(domain, packageType), new List<PackageAnswer>());
			var bytes = request.GetBytes();
			udp.Send(bytes, bytes.Length);

			var serverMessage = udp.Receive(ref endPoint);
			var answer = DnsPackage.FromArray(serverMessage);
			return answer.Answers.Select(x => x.GetAnswer(serverMessage)).Aggregate("", (current, ans) => current + ans + "\r\n");
		}
	}
}
