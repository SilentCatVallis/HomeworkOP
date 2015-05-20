using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Package
{
	public class PackageAnswer
	{
		protected bool Equals(PackageAnswer other)
		{
			return RDLENGTH == other.RDLENGTH && Equals(Encoding.ASCII.GetString(RDATA), Encoding.ASCII.GetString(other.RDATA));
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (RDLENGTH.GetHashCode()*397) ^ (RDATA != null ? RDATA.GetHashCode() : 0);
			}
		}

		private PackageAnswer() { }

		public PackageQuestion Question;
		
		[BitAttribute(Bits = 32)]
		public int TTL { get; set; }

		
		[BitAttribute(Bits = 16)]
		public Int16 RDLENGTH { get; set; }

		public byte[] RDATA { get; set; }
		public int Size { get { return (Question == null ? 0 : Question.Size) + 6 + (RDATA == null ? 0 : RDATA.Length); } }

		public string GetAnswer(byte[] mssg)
		{
			if (RDATA.Length == 4)
				return String.Format("{0}.{1}.{2}.{3}", RDATA[0], RDATA[1], RDATA[2], RDATA[3]);/*
				BitConverter.ToUInt32(RDATA, 0) >> 24,
				(BitConverter.ToUInt32(RDATA, 0) << 8) >> 24,
				(BitConverter.ToUInt32(RDATA, 0) << 16) >> 24,
				(BitConverter.ToUInt32(RDATA, 0) << 24) >> 24);*/
			var str = "";
			var data = SetNormalName(RDATA, mssg);
			int curLen = 0;
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] == new byte())
				{
					str += ".";
					return str;
				}
				if (curLen == 0)
				{
					if (str.Length != 0)
						str += ".";
					curLen = BitConverter.ToInt16(new[] {data[i], new byte()}, 0);
				}
				else
				{
					str += Encoding.ASCII.GetString(new[] {data[i]});
					curLen--;
				}
			}
			return "";
		}

		public IEnumerable<byte> GetBytes()
		{
			if (Question == null)
				return Enumerable.Empty<byte>();
			return Question
				.GetBytes()
				.Concat(BitConverter.GetBytes(TTL).Reverse())
				.Concat(BitConverter.GetBytes(RDLENGTH).Reverse())
				.Concat(RDATA);
		}

		public static PackageAnswer FromArray(byte[] clientMessage, int offset)
		{
			if (clientMessage.Length <= offset + 1)
				return new PackageAnswer();
			var answer = new PackageAnswer {Question = PackageQuestion.FromArray(clientMessage, offset)};
			answer.TTL = BitConverter.ToInt32(clientMessage.Skip(offset + answer.Question.Size).Take(4).Reverse().ToArray(), 0);
			answer.RDLENGTH = BitConverter.ToInt16(clientMessage.Skip(offset + answer.Question.Size + 4).Take(2).Reverse().ToArray(), 0);
			answer.RDATA = clientMessage.Skip(offset + answer.Question.Size + 6).Take(answer.RDLENGTH).ToArray();
			return answer;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((PackageAnswer) obj);
		}

		public byte[] SetNormalName(byte[] name, byte[] clientMessage)
		{
			return SimplifyName(name, clientMessage);
		}

		public void SetNormalName(byte[] clientMessage)
		{
			Question.QNAME = SimplifyName(Question.QNAME, clientMessage);
			if (Question.QTYPE == 512)
			{
				RDATA = SimplifyName(RDATA, clientMessage);
				RDLENGTH = (short)RDATA.Length;
			}
		}

		private byte[] SimplifyName(byte[] qname, byte[] clientMessage)
		{
			var answer = new List<byte>();
			for (var i = 0; i < qname.Length; i++)
			{
				var offset = BitConverter.ToUInt16(new[] { qname[i], new byte() }, 0);
				if (offset >= 192)
				{
					answer.AddRange(
						SimplifyName(
							clientMessage.Skip(
								BitConverter.ToUInt16(new[] { qname[i + 1], new byte() }, 0)).ToArray(), clientMessage));
					return answer.ToArray();
				}
				answer.Add(qname[i]);
				if (qname[i] == new byte())
					return answer.ToArray();
			}
			return null;
		}
	}
}
