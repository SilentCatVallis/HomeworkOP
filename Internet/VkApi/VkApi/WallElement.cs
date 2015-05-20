using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VkApi
{
	[DataContract]
	public class WallElement
	{
		public WallElement()
		{
		}

		[DataMember] public int id;
		[DataMember] public string post_type;
		[DataMember] public int from_id;
		[DataMember] public int owner_id;
		[DataMember] public int date;
		[DataMember] public string text;
		[DataMember] public Attachments[] attachments;
		[DataMember] public Comments comments;
		[DataMember] public Likes likes;
		[DataMember] public Reposts reposts;

		public override string ToString()
		{
			return string.Format("id: {0}, post type: {1}\r\n    from_id: {2} owner_id: {3}\r\n   date: {4}\r\n" +
			                     "    text: {5}\r\n    likes: {6}", id, post_type, from_id, owner_id, date, text, likes.count);
		}
	}

	[DataContract]
	public class Reposts
	{
		public Reposts()
		{
		}

		[DataMember] public int count;
	}

	[DataContract]
	public class Likes
	{
		public Likes()
		{
		}

		[DataMember] public int count;
	}

	[DataContract]
	public class Comments
	{
		public Comments()
		{
		}

		[DataMember] public int count;
	}

	[DataContract]
	public class Attachments
	{
		public Attachments()
		{
		}

		[DataMember] public string type;
		[DataMember] public Photo photo;
	}

	[DataContract]
	public class Photo
	{
		public Photo()
		{
		}

		[DataMember] public int id;
		[DataMember] public int album_id;
		[DataMember] public int owner_id;
		[DataMember] public string photo_75;
		[DataMember] public string photo_130;
		[DataMember] public string photo_604;
		[DataMember] public string photo_807;
		[DataMember] public string photo_1280;
		[DataMember] public int width;
		[DataMember] public int height;
		[DataMember] public string text;
		[DataMember] public int date;
		[DataMember] public int post_id;
	}
}
