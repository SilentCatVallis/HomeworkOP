using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Package
{
	public class PackageHeader
	{
		public PackageHeader()
		{
			ID = 1;
			Opcode = 0;
			RD = 1;
			Z = 0;
			QDCOUNT = 1;
			ANCOUNT = 0;
			ARCOUNT = 0;
			NSCOUNT = 0;
		}

		[BitAttribute(Bits = 16)]
		public Int16 ID { get; set; }

		////
		[BitAttribute(Offset = 15)]
		public int QR { get; set; }

		[BitAttribute(Bits = 4, Offset = 10)]
		public int Opcode { get; set; }

		[BitAttribute(Offset = 9)]
		public int AA { get; set; }

		[BitAttribute(Offset = 8)]
		public int TC { get; set; }

		[BitAttribute(Offset = 7)]
		public int RD { get; set; }

		[BitAttribute(Offset = 6)]
		public int RA { get; set; }

		[BitAttribute(Bits = 3, Offset = 3)]
		public int Z { get; set; }

		[BitAttribute(Bits = 4, Offset = 0)]
		public int RCODE { get; set; }
		////

		[BitAttribute(Bits = 16)]
		public Int16 QDCOUNT { get; set; }


		[BitAttribute(Bits = 16)]
		public Int16 ANCOUNT { get; set; }

		[BitAttribute(Bits = 16)]
		public Int16 NSCOUNT { get; set; }

		[BitAttribute(Bits = 16)]
		public Int16 ARCOUNT { get; set; }

		public IEnumerable<byte> GetBytes()
		{
			return BitConverter.GetBytes(ID)
				.Concat(GetFlags())
				.Concat(BitConverter.GetBytes(QDCOUNT).Reverse())
				.Concat(BitConverter.GetBytes(ANCOUNT).Reverse())
				.Concat(BitConverter.GetBytes(NSCOUNT).Reverse())
				.Concat(BitConverter.GetBytes(ARCOUNT).Reverse());
		}

		public int Size
		{
			get { return 12; }
		}

		private IEnumerable<byte> GetFlags()
		{
			UInt16 a = (UInt16)(RD << 16);
			var gavno = 
				BitConverter.GetBytes((UInt16) ((QR << 15) + (Opcode << 11) + (AA << 10) + (TC << 9) + (RD << 8) + (RA << 7) + (Z << 4) + RCODE)).Reverse();
			return gavno;
		}

		public static PackageHeader FromArray(byte[] clientMessage)
		{
			var header = new PackageHeader
			{
				ID = BitConverter.ToInt16(clientMessage, 0),
				QR = BitConverter.ToUInt16(clientMessage, 2) >> 15,
				AA = (BitConverter.ToUInt16(clientMessage, 2) << 5) >> 15,
				TC = (BitConverter.ToUInt16(clientMessage, 2) << 6) >> 15,
				RA = (BitConverter.ToUInt16(clientMessage, 2) << 8) >> 15,
				RCODE = (BitConverter.ToUInt16(clientMessage, 2) << 12) >> 15,
				QDCOUNT = BitConverter.ToInt16(clientMessage.Skip(4).Take(2).Reverse().ToArray(), 0),
				ANCOUNT = BitConverter.ToInt16(clientMessage.Skip(6).Take(2).Reverse().ToArray(), 0),
				NSCOUNT = BitConverter.ToInt16(clientMessage.Skip(8).Take(2).Reverse().ToArray(), 0),
				ARCOUNT = BitConverter.ToInt16(clientMessage.Skip(10).Take(2).Reverse().ToArray(), 0)
			};
			return header;
		}
	}
}
