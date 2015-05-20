using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;


namespace DNS.Server
{
	public class MasterFile
	{
		private static readonly TimeSpan DEFAULT_TTL = new TimeSpan(0);

		private IList<IResourceRecord> entries = new List<IResourceRecord>();
		private readonly TimeSpan ttl = DEFAULT_TTL;

		public MasterFile(TimeSpan ttl)
		{
			this.ttl = ttl;
		}

		public int Count
		{
			get { return entries.Count; }
		}

		public MasterFile() { }

		public void Add(IResourceRecord entry)
		{
			entries.Add(entry);
		}

		public void AddIPAddressResourceRecord(string domain, string ip)
		{
			AddIPAddressResourceRecord(new Domain(domain), IPAddress.Parse(ip));
		}

		public void AddIPAddressResourceRecord(Domain domain, IPAddress ip)
		{
			Add(new IPAddressResourceRecord(domain, ip, ttl));
		}

		public void AddNameServerResourceRecord(string domain, string nsDomain)
		{
			AddNameServerResourceRecord(new Domain(domain), new Domain(nsDomain));
		}

		public void AddNameServerResourceRecord(Domain domain, Domain nsDomain)
		{
			Add(new NameServerResourceRecord(domain, nsDomain, ttl));
		}

		public void AddCanonicalNameResourceRecord(string domain, string cname)
		{
			AddCanonicalNameResourceRecord(new Domain(domain), new Domain(cname));
		}

		public void AddCanonicalNameResourceRecord(Domain domain, Domain cname)
		{
			Add(new CanonicalNameResourceRecord(domain, cname, ttl));
		}

		public void AddPointerResourceRecord(string domain, string pointer)
		{
			AddPointerResourceRecord(new Domain(domain), new Domain(pointer));
		}

		public void AddPointerResourceRecord(Domain domain, Domain pointer)
		{
			Add(new PointerResourceRecord(domain, pointer, ttl));
		}

		public void AddMailExchangeResourceRecord(string domain, int preference, string exchange)
		{
			AddMailExchangeResourceRecord(new Domain(domain), preference, new Domain(exchange));
		}

		public void AddMailExchangeResourceRecord(Domain domain, int preference, Domain exchange)
		{
			Add(new MailExchangeResourceRecord(domain, preference, exchange));
		}

		public IList<IResourceRecord> Get(Domain domain, RecordType type)
		{
			return entries.Where(e => e.Name.Equals(domain) && e.Type == type).ToList();
		}

		public IList<IResourceRecord> Get(Question question)
		{
			return Get(question.Name, question.Type);
		}

		public void Serialize()
		{
			var stream = File.OpenWrite("hash.txt");
			BsonWriter writer = new BsonWriter(stream);

			JsonSerializer serializer = new JsonSerializer();
			serializer.Serialize(writer, entries
				.Where(e => e is IPAddressResourceRecord)
				.Cast<IPAddressResourceRecord>()
				.Select(e => String.Format("{0} {1} {2} {3}", e.IPAddress.Address, e.Name, (int)e.Type, e.TimeToLive.ToString()))
				.ToList());
			stream.Flush();

		}

		public void Deserialize()
		{
			if (!File.Exists("hash.txt"))
			{
				AddIPAddressResourceRecord("google.com", "127.0.0.1");
				AddIPAddressResourceRecord("github.com", "127.0.0.1");
				return;
			}

			var stream = File.OpenRead("hash.txt");
			BsonReader reader = new BsonReader(stream);

			JsonSerializer serializer = new JsonSerializer();

			var lines = serializer.Deserialize<List<string>>(reader);
			entries = entries.Concat(lines
				.Select(l => l.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries))
				.Select(ConvertIpAddressResourceRecord))
				.ToList();
		}

		private static IPAddressResourceRecord ConvertIpAddressResourceRecord(string[] l)
		{
			var ip =  new IPAddressResourceRecord(new Domain(l[1]), new IPAddress(long.Parse(l[0])), TimeSpan.Parse(l[3]));
			ip.Type = (RecordType) int.Parse(l[2]);
			return ip;
		}
	}
}
