using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VkApi
{
	[DataContract]
	public class Group
	{
		public Group()
		{
		}

		[DataMember] public int id;
		[DataMember] public string name;
		[DataMember] public string screen_name;
		[DataMember] public int is_closed;
		[DataMember] public int is_admin;
		[DataMember] public int admin_level;
		[DataMember] public int is_member;
		[DataMember] public string type;
		[DataMember] public string photo_50;
		[DataMember] public string photo_100;
		[DataMember] public string photo_200;
	}
}
