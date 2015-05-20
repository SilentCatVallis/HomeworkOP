using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DNS.Client;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;

namespace ClientDNS
{
	class Program
	{
		static void Main(string[] args)
		{
			DnsClient client = new DnsClient("127.0.0.1");

			while (true)
			{

				var request = new ClientRequest("8.8.8.8");

				var add = Console.ReadLine();

				// Request an IPv6 record for the foo.com domain
				request.Questions.Add(new Question(Domain.FromString(add), RecordType.A));
				request.RecursionDesired = true;

				Console.WriteLine(request.ToArray().Length);

				var bytes = request.ToArray();
				ClientResponse response = request.Resolve();

				// Get all the IPs for the foo.com domain
				IList<IPAddress> ips = response.AnswerRecords
					.Where(r => r.Type == RecordType.A)
					.Cast<IPAddressResourceRecord>()
					.Select(r => r.IPAddress)
					.ToList();
				foreach (var ip in ips)
					Console.WriteLine(ip);
			}
		}
	}
}
