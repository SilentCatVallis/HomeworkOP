using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace VkApi
{
	[DataContract(Name = "response")]
	public class WResponse
	{
		public WResponse()
		{
		}

		[DataMember] public int count;
		[DataMember(Name = "items")] public WallElement[] WallElements;
	}

	[DataContract]
	public class WallResponse
	{
		public WallResponse()
		{
		}

		[DataMember(Name = "response")] public WResponse response;
	}

	public class WallWorker
	{
		private Api api;

		private const string MethodName = "wall.get";

		public WallWorker(Api api)
		{
			this.api = api;
		}

		public WallResponse GetWall(string userId = null)
		{
			if (userId == null)
				userId = api.userId;
			using (var client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri(api.MethodBaseAddres + MethodName + "?");

					var content = new FormUrlEncodedContent(new[]
					{
						new KeyValuePair<string, string>("oauth", "1"),
						new KeyValuePair<string, string>("owner_id", userId),
						//new KeyValuePair<string, string>("count", "2"),
						new KeyValuePair<string, string>("v", api.v)
					});
					var result = client.PostAsync("", content).Result;

					var responseString = WebUtility.HtmlDecode(result.Content.ReadAsStringAsync().Result)
						.Replace("<HTML><BODY>", "")
						.Replace("</BODY></HTML>", "");

					var json = new DataContractJsonSerializer(typeof(WallResponse));
					return (WallResponse)json.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(responseString)));

				}
				catch
				{
					throw;
				}
				return null;
			}
		}
	}
}
