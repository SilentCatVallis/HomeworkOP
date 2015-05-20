using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VkApi
{
	public class Api
	{
		protected const string Login = "";
		protected const string Password = "";
		protected const string Id = "4904106";
		protected const string Secret = "eqYMkwCInCBEjQIuNZYb";
		public string userId = "18209068";
		public string v = "5.30";
		public string accessToken;
		public string clientAccessToken;
		public string MethodBaseAddres = "https://api.vk.com/method/";

		public Api()
		{
			FriendsWorker = new FriendsWorker(this);
			WallWorker = new WallWorker(this);
		}

		public WallWorker WallWorker { get; private set; }
		public FriendsWorker FriendsWorker { get; private set; }

		public string Authorization()
		{
			return Authorization(Id, Secret);
		}

		public string Authorization(string id, string secret)
		{
			using (var client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://oauth.vk.com/access_token?");

					var content = new FormUrlEncodedContent(new[]
					{
						new KeyValuePair<string, string>("client_id", id),
						new KeyValuePair<string, string>("client_secret", secret),
						new KeyValuePair<string, string>("v", v),
						new KeyValuePair<string, string>("grant_type", "client_credentials")
					});
					var result = client.PostAsync("", content).Result;

					var resultContent = WebUtility.HtmlDecode(result.Content.ReadAsStringAsync().Result)
						.Replace("<HTML><BODY>", "")
						.Replace("</BODY></HTML>", "")
						.Split(new[] {"{", "\"", "}", ":", ","}, StringSplitOptions.RemoveEmptyEntries);
					accessToken = resultContent[1];
					return accessToken;
				}
				catch
				{
					throw;
					return "";
				}
			}
		}

		public string ClientAuthorization(string id=null)
		{
			if (id == null)
				id = Id;
			using (var client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://oauth.vk.com/access_token?");

					var content = new FormUrlEncodedContent(new[]
					{
						new KeyValuePair<string, string>("client_id", userId),
						new KeyValuePair<string, string>("scope", "31"),
						new KeyValuePair<string, string>("REDIRECT_URI", "https://oauth.vk.com/blank.html "),
						new KeyValuePair<string, string>("DISPLAY", "page"),
						new KeyValuePair<string, string>("v", v),
						new KeyValuePair<string, string>("response_type", "token")
					});
					var result = client.PostAsync("", content).Result;

					var resultContent = WebUtility.HtmlDecode(result.Content.ReadAsStringAsync().Result)
						.Replace("<HTML><BODY>", "")
						.Replace("</BODY></HTML>", "")
						.Split(new[] { "{", "\"", "}", ":", "," }, StringSplitOptions.RemoveEmptyEntries);
					clientAccessToken = resultContent[1];
					return accessToken;
				}
				catch
				{
					throw;
					return "";
				}
			}
		}
	}
}
