using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Package;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace ServerDNS
{
	[Serializable]
	public class Storage
	{
		private const string StorageFileName = "storage.txt";
		public Dictionary<PackageQuestion, List<PackageAnswer>> dict = new Dictionary<PackageQuestion, List<PackageAnswer>>();
		private Dictionary<DateTime, PackageQuestion> events = new Dictionary<DateTime, PackageQuestion>();

		private Storage()
		{
		}

		public void SetValue(PackageQuestion q, List<PackageAnswer> l)
		{
			dict[q] = l;
		}

		public static Storage GetStorage()
		{
			if (!File.Exists(StorageFileName))
				return new Storage();

			try
			{
				var storage = new Storage();
				var lines = File.ReadAllLines(StorageFileName);
				var keys = JsonConvert.DeserializeObject<List<PackageQuestion>>(lines[0]);
				var values = JsonConvert.DeserializeObject<List<List<PackageAnswer>>>(lines[1]);
				foreach (var key in keys.Select((x, i) => new KeyValuePair<PackageQuestion, List<PackageAnswer>>(x, values[i])))
					storage.Add(key.Key, key.Value);

				storage.events = JsonConvert.DeserializeObject<Dictionary<DateTime, PackageQuestion>>(lines[2]);
				return storage;

				var dictCount = int.Parse(lines[0]);
				for (int i = 1; i < dictCount + 1; i++)
				{
					var answerss = lines[i].Split('=').Last().Split(new[] {"&"}, StringSplitOptions.RemoveEmptyEntries);
					var answersss = answerss.Select(x => Encoding.UTF8.GetBytes(x));
					var answers = answersss.Select(x => PackageAnswer.FromArray(x, 0));
					storage.Add(PackageQuestion.FromArray(Encoding.UTF8.GetBytes(lines[i].Split('=').First()).ToArray(), 0),
						answers.ToList());
				}
				var eventsCount = int.Parse(lines[dictCount + 1]);
				for (int i = dictCount + 2; i < eventsCount + dictCount + 2; i++)
				{
					storage.events.Add(DateTime.Parse(lines[i].Split('=').First()),
						PackageQuestion.FromArray(Encoding.UTF8.GetBytes(lines[i].Split('=').Last()).ToArray(), 0));
				}
				return storage;
			}
			catch(Exception)
			{
				throw;
				return new Storage();
			}
		}

		public void Serialise()
		{
			var dictKeysString = JsonConvert.SerializeObject(dict.Keys.ToList());
			var dictValuesString = JsonConvert.SerializeObject(dict.Values.ToList());
			var eventsString = JsonConvert.SerializeObject(events);
			File.WriteAllLines(StorageFileName, new []
			{
				dictKeysString, dictValuesString, eventsString
			});
			return;

			var lines = new List<string>();
			lines.Add(dict.Count.ToString());
			foreach (var pair in dict)
			{
				var bytes = pair.Key.GetBytes().ToArray();
				lines.Add(Encoding.UTF8.GetString(bytes, 0, bytes.Length) + "=" +
					pair.Value.Aggregate("", (str, ans) => str + 
						Encoding.UTF8.GetString(ans.GetBytes().ToArray(), 0, ans.GetBytes().ToArray().Length) + "&"));
			}

			lines.Add(events.Count.ToString());
			foreach (var pair in events)
			{
				lines.Add(pair.Key.ToString(CultureInfo.InvariantCulture) + "=" +
						  Encoding.UTF8.GetString(pair.Value.GetBytes().ToArray(), 0, pair.Value.GetBytes().ToArray().Length));
			}
			File.WriteAllLines(StorageFileName, lines);
		}

		public void Add(PackageQuestion question, List<PackageAnswer> answer)
		{
			if (dict.ContainsKey(question))
			{
				dict[question].AddRange(answer);
				events[DateTime.Now.AddSeconds(answer.First().TTL)] = question;
			}
			else
			{
				dict.Add(question, answer);
				events[DateTime.Now.AddSeconds(answer.First().TTL)] = question;
			}
		}

		public List<PackageAnswer> Get(PackageQuestion question)
		{
			return dict[question];
		}

		public void RemoveOld()
		{
			foreach (var e in events.ToList().Where(e => DateTime.Now.CompareTo(e.Key) > 0))
			{
				dict.Remove(e.Value);
				events.Remove(e.Key);
			}
		}

		public bool ContainKey(PackageQuestion question)
		{
			return dict.ContainsKey(question);
		}
	}
}
