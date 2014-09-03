using System;
using System.Drawing;
using System.Windows.Forms;

namespace mazes
{
	public class MazeForm : Form
	{
		private const int imageSize = 32;
		private int steps = 8;
		private readonly Timer timer = new Timer();
		private int timeFractions;
		private readonly World world = new World();
		private readonly int stepsPerSecond;

		public Images Images { get; set; }

		public MazeForm(): this(new Images("."), new World())
		{
		}

		public MazeForm(Images images, World world, int stepsPerSecond = 10)
		{
			this.world = world;
			this.stepsPerSecond = stepsPerSecond;
			Images = images;
			ClientSize = world.Size.Scale(imageSize);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			timer.Tick += OnTimer;
			timer.Interval = 20;
			steps = 1000/stepsPerSecond/timer.Interval;
			timer.Start();
			DoubleBuffered = true;
			Text = "Maze";
		}

		//protected override void OnMouseMove(MouseEventArgs e)
		//{
		//	base.OnMouseMove(e);
		//	world.Cursor = new Point((e.X) / imageSize, (e.Y) / imageSize);
		//}

		private void OnTimer(object sender, EventArgs e)
		{
			timeFractions = (timeFractions + 1)%steps;;
			if (timeFractions == 0) world.MakeStep();
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			foreach (var obj in world.Objects)
			{
				var delta = (float)timeFractions / steps;
				var x = obj.Destination.X * delta + obj.Location.X * (1 - delta);
				var y = obj.Destination.Y * delta + obj.Location.Y * (1 - delta);
				g.DrawImage(obj.GetImage(Images, world.Time), new RectangleF(x * imageSize, y * imageSize, imageSize, imageSize));
			}
		}
	}
}
