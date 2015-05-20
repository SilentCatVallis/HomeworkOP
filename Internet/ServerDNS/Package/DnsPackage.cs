using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Package
{
	public class DnsPackage
	{
		public DnsPackage(PackageHeader header, PackageQuestion question, List<PackageAnswer> answers)
		{
			Header = header;
			Question = question;
			Answers = answers;
		}

		public PackageHeader Header { get; private set; }
		public PackageQuestion Question { get; private set; }
		public List<PackageAnswer> Answers { get; private set; }
		public List<PackageAnswer> Additionals { get; set; }
		public List<PackageAnswer> Auth { get; set; }
		public int Size { get { return Header.Size + Question.Size + Answers.Select(a => a.Size).Sum(); } }

		public byte[] GetBytes()
		{
			byte[] bytes = new byte[0];
			var ans = Header.GetBytes();
			ans = ans.Concat(Question.GetBytes());
			ans = ans.Concat(Answers.Aggregate(bytes, (localBytes, answer) => localBytes.Concat(answer.GetBytes()).ToArray()));
			return ans.ToArray();
		}

		public static DnsPackage FromArray(byte[] clientMessage)
		{
			Console.WriteLine(clientMessage.Length);
			var header = PackageHeader.FromArray(clientMessage);
			Console.WriteLine("HEADER DONE");
			var question = PackageQuestion.FromArray(clientMessage, 12);
			Console.WriteLine("QUESTION DONE");
			var answers = new List<PackageAnswer>();
			var lastSize = 12 + question.Size;
			for (int i = 0; i < header.ANCOUNT; i++)
			{
				var answer = PackageAnswer.FromArray(clientMessage, lastSize);
				if (answer.Size == 6)
					break;
				answers.Add(answer);
				lastSize += answer.Size;
			}
			var auths = new List<PackageAnswer>();
			for (int i = 0; i < header.NSCOUNT; i++)
			{
				var authorityAnswer = PackageAnswer.FromArray(clientMessage, lastSize);
				lastSize += authorityAnswer.Size;
				authorityAnswer.SetNormalName(clientMessage);
				auths.Add(authorityAnswer);
			}
			var additionals = new List<PackageAnswer>();
			for (int i = 0; i < header.ARCOUNT; i++)
			{
				var additionalAnswer = PackageAnswer.FromArray(clientMessage, lastSize);
				lastSize += additionalAnswer.Size;
				additionalAnswer.Question.QTYPE = (int)Type.NS;
				additionalAnswer.SetNormalName(clientMessage);
				additionals.Add(additionalAnswer);
			}
			foreach (var add in additionals)
			{
				var q = auths.FirstOrDefault(x => ArraysEquals(x.RDATA, add.Question.QNAME));
				if (q != null)
					add.Question.QNAME = q.Question.QNAME;
			}
			Console.WriteLine("ANSWER DONE");
			return new DnsPackage(header, question, answers) {Additionals = additionals, Auth = auths};
		}

		private static bool ArraysEquals(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;
			return !a.Where((t, i) => t != b[i]).Any();
		}
	}
}
