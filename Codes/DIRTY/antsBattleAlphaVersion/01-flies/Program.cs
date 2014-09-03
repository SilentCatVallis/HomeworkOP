using System;
using System.Windows.Forms;
using System.Linq;
using mazes;

namespace Ants
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
				.AddRule('0', loc => new Wall(loc))
				.AddRule('F', loc => new Fridge(loc))
                .AddRule('C', loc => new Computer(loc))
                .AddRule('2', loc => new Dirty(loc))
                .Load("mazes\\maze.txt", world);

			var mainForm = new MazeForm(new Images(".\\images"), world);
			
			Application.Run(mainForm);
		}
	}
}
