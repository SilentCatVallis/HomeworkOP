using System;
using System.Windows.Forms;
using mazes;

namespace Flies
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
			var world = new World();
			
			//Определяем правила интерпретации символов.
			new WorldLoader()
				.AddRule('#', loc => new Wall(loc))
				.AddRule('S', loc => new Source(loc))
				.AddRule('T', loc => new Target(loc))
				.Load("mazes\\maze.txt", world);

			var mainForm = new MazeForm(new Images(".\\images"), world);
			
			Application.Run(mainForm);
		}
	}
}
