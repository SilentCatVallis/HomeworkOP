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
	public class FResponse
	{
		public FResponse()
		{
		}

		[DataMember] public int count;
		[DataMember(Name = "items")] public User[] friends;
	}

	[DataContract]
	public class FriendsResponse
	{
		public FriendsResponse()
		{
		}

		[DataMember(Name = "response")] public FResponse response;
	}

	public class FriendsWorker
	{
		private Api api;
		//'''METHOD_NAME'''?'''PARAMETERS'''&access_token='''ACCESS_TOKEN


		private const string MethodName = "friends.get";


		public FriendsWorker(Api api)
		{
			this.api = api;
		}

		public FriendsResponse GetFriends(string userId = null, string order = "hints", string[] fields = null)
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
						new KeyValuePair<string, string>("fields", "nickname,domain,sex,city,online"),
						new KeyValuePair<string, string>("order", order),
						new KeyValuePair<string, string>("oauth", "1"),
						new KeyValuePair<string, string>("method", MethodName),
						new KeyValuePair<string, string>("user_id", userId),
						new KeyValuePair<string, string>("v", api.v)
					});
					var result = client.PostAsync("", content).Result;

					var responseString = WebUtility.HtmlDecode(result.Content.ReadAsStringAsync().Result)
						.Replace("<HTML><BODY>", "")
						.Replace("</BODY></HTML>", "");

					var json = new DataContractJsonSerializer(typeof (FriendsResponse));
					return (FriendsResponse) json.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(responseString)));

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
