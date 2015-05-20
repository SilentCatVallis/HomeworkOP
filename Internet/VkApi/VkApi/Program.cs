using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkApi
{
	class Program
	{
		static void Main(string[] args)
		{
			var api = new Api();

			while (true)
			{
				var line = Console.ReadLine().Split(' ');
				var command = line[0];
				if (command == "friends")
				{
					var friends = api.FriendsWorker.GetFriends();
					try
					{
						foreach (var friend in friends.response.friends)
						{
							Console.WriteLine(friend);
						}
					}
					catch { }
				}
				if (command == "wall")
				{
					var wallElements = api.WallWorker.GetWall();
					try
					{
						foreach (var wallElement in wallElements.response.WallElements)
						{
							Console.WriteLine(wallElement);
						}
					}
					catch { }
				}
			}
			//Console.WriteLine(api.Authorization());
			//var friends = api.FriendsWorker.GetFriends();
			//Console.WriteLine(friends.response.count);
			//var wall = api.WallWorker.GetWall();
			// Console.WriteLine(wall.response.count);
		}
	}
}
