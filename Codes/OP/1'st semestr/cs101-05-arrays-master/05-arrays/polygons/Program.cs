using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TestingRoom;

namespace polygons
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
			Application.Run(new TestRoom(CreateTests()));
		}

		private static IEnumerable<PolygonTestCase> CreateTests()
		{
			foreach (var test in LoadTests(0, 0)) yield return test;
			foreach (var test in LoadTests(50, 0)) yield return test;
			foreach (var test in LoadTests(50, 50)) yield return test;
			foreach (var test in LoadTests(50, -50)) yield return test;
			foreach (var test in LoadTests(-50, -50)) yield return test;
		}

		private static IEnumerable<PolygonTestCase> LoadTests(float dx, float dy)
		{
			return
				Directory
					.EnumerateFiles("tests", "*.txt")
					.Select(Path.GetFileNameWithoutExtension)
					.Select(name => new PolygonTestCase(name, dx, dy));
		}
	}

	internal class PolygonTestCase : TestCase
	{
		private PointF[] points = new PointF[0];
		private double calculatedArea, area;
		private bool calculatedIsConvex, isConvex;
		private PointF[] scaledPoints;

		public PolygonTestCase(string name, float dx, float dy) : base(name)
		{
			LoadPolygon(".\\tests\\" + name + ".txt", dx, dy);
		}

		private void LoadPolygon(string filename, float dx, float dy)
		{
			var lines = File.ReadAllLines(filename);
			isConvex = lines[0] == "Convex";
			area = double.Parse(lines[1]);
			points = LoadPoints(lines.Skip(2)).Select(p => new PointF(p.X + dx, p.Y + dy)).ToArray();
		}

		private IEnumerable<PointF> LoadPoints(IEnumerable<string> lines)
		{
			return
				lines
					.Select(l => l.Split(' '))
					.Select(p => new PointF(float.Parse(p[0]), float.Parse(p[1])));
		}
	

		protected override void InternalVisualize(TestCaseUI ui)
		{
			ui.Log("Calculated area: " + calculatedArea);
			ui.Log("Calculated isConvex: " + calculatedIsConvex);
			ui.Log("Expected area: " + area);
			ui.Log("Expected isConvex: " + isConvex);
			ui.Rect(new Rectangle(-90, -90, 180, 180), new Pen(Color.FromArgb(50, Color.Green), 1));
			Action<PointF> plot = p => ui.Circle(p.X, p.Y, 3, neutralPen);
			PointF prev = scaledPoints.Last();
			foreach (var curr in scaledPoints)
			{
				plot(curr);
				ui.Line(prev.X, prev.Y, curr.X, curr.Y, neutralThinPen);
				prev = curr;
			}
		}

		protected override bool InternalRun()
		{
			var tasks = new PolygonTasks();
			scaledPoints = tasks.RescalePolygon(points, new RectangleF(-90, -90, 180, 180));
			calculatedArea = tasks.GetArea(points);
			calculatedIsConvex = tasks.IsConvex(points);
			return
				Math.Abs(calculatedArea - area) < Math.Max(1e-10, 1e-5 * area)
				&& calculatedIsConvex == isConvex;
		}
	}
}
