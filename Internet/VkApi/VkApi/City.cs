using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VkApi
{
	[DataContract(Name = "city")]
	public class City
	{
		public City()
		{
		}

		[DataMember] public int id;
		[DataMember] public string title;

		public override string ToString()
		{
			return string.Format("id: {0} title: {1}", id, title);
		}
	}
}
