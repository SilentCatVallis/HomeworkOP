using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MapMaker
{
	[TestFixture]
	class MapMakerShould
	{
		[Test]
		public void ReturnMap()
		{
			const int height = 30;
			const int width = 30;
			var mapMaker = new MapMaker(height, width);
			var map = mapMaker.GetMap();
			for (var y = 0; y < height + 3; y++)
			{
				Console.WriteLine(map[y]);
			}
		}
	}
}
