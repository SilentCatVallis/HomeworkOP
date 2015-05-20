using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new TimeServer.Client();
			client.Start();
		}
	}
}
