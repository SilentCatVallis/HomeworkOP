using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VkApi
{
	[DataContract]
	public class User
	{
		public User()
		{
		}

		[DataMember] public int id;
		[DataMember] public int sex;
		[DataMember] public string first_name;
		[DataMember] public string last_name;
		[DataMember] public string nickname;
		[DataMember] public string domain;
		[DataMember] public City city;
		[DataMember] public int online;
		[DataMember] public string online_app;
		[DataMember] public int online_mobile;

		public override string ToString()
		{
			return string.Format("first name: {0}\r\n  last name: {1}\r\n  nickname: {2}\r\n", first_name, last_name, nickname) +
				string.Format("    id: {0} sex: {1} domain: {2}\r\n", id, sex, domain) +
				string.Format("    City: {0} \r\n    online: {1}, online mobile: {2}\r\n", city, online, online_mobile);
		}
	}
}
