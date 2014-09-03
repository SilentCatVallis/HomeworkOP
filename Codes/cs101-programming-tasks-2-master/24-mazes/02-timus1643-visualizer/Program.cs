using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using mazes;
using timus1643;

namespace Timus1643.Visualization
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var mazeLines = File.ReadLines("input.txt").Skip(1).ToList();
			var world = LoadWorld(mazeLines);
			var attackPlan = AttackSandroAlgorithms.FindPlan(mazeLines);
			AssignPlan(attackPlan, world);
	
			Application.Run(new MazeForm(new Images("images"), world, 1));
		}

		private static void AssignPlan(AttackPlan assignPlan, World world)
		{
			if (assignPlan != null)
			{
				var gelu = world.Objects.OfType<Gelu>().Single();
				var catherine = world.Objects.OfType<Catherine>().Single();
				var shift = new Point(1, 1);
				gelu.SetPathPlan(assignPlan.GeluPath.Select(p => p.Add(shift)).ToArray());
				catherine.SetPathPlan(assignPlan.CatherinePath.Select(p => p.Add(shift)).ToArray());
			}
		}

		private static World LoadWorld(List<string> mazeLines)
		{
			var world = new World();
			var loader = new WorldLoader();
			for (char ch = 'A'; ch <= 'Z'; ch++)
			{
				var teleportSymbol = ch;
				loader.AddRule(ch, location => new Teleport(location, teleportSymbol));
			}
			loader
				.AddRule('#', loc => new Wall(loc))
				.AddRule('!', loc => new Gelu(loc))
				.AddRule('$', loc => new Catherine(loc))
				.AddRule('*', loc => new Sandro(loc))
				.AddBorder('#');
			loader.Load(mazeLines, world);
			return world;
		}
	}
}
