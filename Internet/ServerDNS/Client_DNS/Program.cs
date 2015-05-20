
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DNS
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new Client("127.0.0.1");
			while (true)
			{
				var line = Console.ReadLine().Split(' ');
				Console.WriteLine(client.Resolve(line.First(), line.Last()));
			}
		}
	}
}
