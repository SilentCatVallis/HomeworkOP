using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Package
{
	public class BitAttribute : Attribute
	{
		public BitAttribute()
		{
			Bits = 1;
			Offset = 0;
		}
		public int Bits { get; set; }
		public int Offset { get; set; }
	}

	public enum Type
	{
		A = 1,
		NS = 2,
		MX = 15,
		PTR = 12,
		CNAME = 5
	}
}
