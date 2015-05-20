using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Package
{
	public class PackageQuestion
	{
		protected bool Equals(PackageQuestion other)
		{
			return Equals(Encoding.ASCII.GetString(QNAME), Encoding.ASCII.GetString(other.QNAME)) && TypesEquals(QTYPE, other.QTYPE) && ClassEquals(QCLASS, other.QCLASS);
		}

		private bool ClassEquals(short a, short b)
		{
			return a == b ||
				   a == 512 && b == 2 ||
				   a == 2 && b == 512 ||
				   a == 1 && b == 256 ||
				   a == 256 && b == 1;
		}

		private bool TypesEquals(short a, short b)
		{
			return a == b ||
			       a == 512 && b == 2 ||
			       a == 2 && b == 512 ||
			       a == 1 && b == 256 ||
			       a == 256 && b == 1;
		}

		public override int GetHashCode()
		{
			return 0;
			unchecked
			{
				var hashCode = (QNAME != null ? QNAME.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ QTYPE.GetHashCode();
				hashCode = (hashCode*397) ^ QCLASS.GetHashCode();
				return hashCode;
			}
		}

		public PackageQuestion()
		{
		}

		public PackageQuestion(string name, Type type)
		{
			var domens = name.Split('.');
			var bytes = new List<byte>();
			foreach (var domen in domens)
			{
				var lengthByte = BitConverter.GetBytes(domen.Length).First();
				var nameBytes = Encoding.ASCII.GetBytes(domen);
				bytes.AddRange(new byte[0].Concat(new []{lengthByte}).Concat(nameBytes));
			}
			bytes.Add(new byte());
			QNAME = bytes.ToArray();
			QTYPE = (Int16)((int) type);
			QCLASS = 1;
		}
			
		public byte[] QNAME { get; set; }

		
		[BitAttribute(Bits = 16)]
		public Int16 QTYPE { get; set; }

		
		[BitAttribute(Bits = 16)]
		public Int16 QCLASS { get; set; }

		public int Size { get { return QNAME.Length + 4; } }

		public IEnumerable<byte> GetBytes()
		{
			var bytes =  QNAME;
			bytes = bytes.Concat(BitConverter.GetBytes(QTYPE).Reverse()).ToArray();
			bytes = bytes.Concat(BitConverter.GetBytes(QCLASS).Reverse()).ToArray();
			return bytes;
		}

		public static PackageQuestion FromArray(byte[] clientMessage, int offset)
		{
			var question = new PackageQuestion {QNAME = new byte[0]};
			var lastIndex = offset;
			for (var i = offset; i < clientMessage.Length; i++)
			{
				var number = BitConverter.ToInt16(new[] { clientMessage[i], new byte() }, 0);
				if (number >= 192)
				{
					question.QNAME = question.QNAME.Concat(new[] { clientMessage[i], clientMessage[i + 1] }).ToArray();
					lastIndex += 2;
					break;
				}
				question.QNAME = question.QNAME.Concat(new[] {clientMessage[i]}).ToArray();

				if (clientMessage[i].CompareTo(new byte()) != 0) continue;
				lastIndex = i;
				break;
			}
			question.QTYPE = BitConverter.ToInt16(new []{clientMessage[lastIndex + 2], clientMessage[lastIndex + 1]}, 0);
			question.QCLASS = BitConverter.ToInt16(new[] { clientMessage[lastIndex + 4], clientMessage[lastIndex + 3] }, 0);
			return question;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((PackageQuestion) obj);
		}

		public string GetName()
		{
			var index = 0;
			var answer = "";
			while (true)
			{
				var b = QNAME[index];
				var length = BitConverter.ToInt16(new []{b, new byte()}, 0);
				if (length == 0)
					break;
				index++;
				for (var i = index; i <= index + length; i++)
				{
					answer += Encoding.ASCII.GetString(new[] {QNAME[i]});
				}
				index += length + 1;
			}
			return answer;
		}
	}
}
