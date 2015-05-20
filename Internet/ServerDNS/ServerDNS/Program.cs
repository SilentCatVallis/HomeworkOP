using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Package;

namespace ServerDNS
{
	class Program
	{
		static void Main(string[] args)
		{
			var server = new Server();
			server.StartListen();
			//DnsServer server = new DnsServer("8.8.8.8");
			//
			//server.MasterFile.Deserialize();
			//
			//
			//
			//server.Responded += (request, response) => Console.WriteLine("{0} \n=>\n {1}\n{2}", request, response, server.MasterFile.Count);
			//
			//
			//var task = new Task(() => server.Listen());
			//task.Start();
			//while (true)
			//{
			//	var line = Console.ReadLine()??"".ToLower();
			//	if (line == "exit" || line == "quit")
			//	{
			//		server.MasterFile.Serialize();
			//		try
			//		{
			//			task.Dispose();
			//		}
			//		catch
			//		{
			//		}
			//		return;
			//	}
			//}
		}
	}
}
