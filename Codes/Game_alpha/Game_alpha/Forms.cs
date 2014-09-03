using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game_alpha
{
    public sealed partial class Forms : Form
    {
        private const int ObjectSize = 50;
        private const int CreatureSize = 32;
        private const int AnotherSize = 16;
        private Timer Timer;
        private bool IsWin;
        private bool Shoot;
        public World World = new World();
        public WorldObjects Objects = new WorldObjects();

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            var deltaX = 0;
            var deltaY = 0;
            if (e.KeyCode == Keys.W)
                deltaY -= 2;
            if (e.KeyCode == Keys.S)
                deltaY += 2;
            if (e.KeyCode == Keys.A)
                deltaX -= 2;
            if (e.KeyCode == Keys.D)
                deltaX += 2;
            World.MoveHero(deltaX, deltaY, Size.Width, Size.Height);
            if (e.KeyCode != Keys.Space) return;
            World.ActivateSomething();
        }

        public Forms(World world)
        {
            World = world;
            DoubleBuffered = true;
            Timer = new Timer { Interval = 30 };
            Timer.Tick += TimerTick;
            Timer.Start();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            Paint += PaintWorld;
            ResizeRedraw = true;
            KeyDown += OnKeyDown;
            //MouseClick += OnMouseClick;
        }
        
        private void PaintWorld(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            if (World.IsWin)
            {
                graphics.DrawImage(World.Images["Win"], 0, 0, Size.Width, Size.Height);
                return;
            }
            if (World.IsHeroDead)
            {
                graphics.DrawImage(World.Images["Loose"], 0, 0, Size.Width, Size.Height);
                return;
            }
            graphics.DrawImage(World.Images["Map"], 0, 0);
            DrawActiveObjects(graphics);
            DrawEnemies(graphics);
            DrawVisualEffects(graphics);
        }

        private void DrawActiveObjects(Graphics graphics)
        {
            foreach (var obj in World.Objects.Bridges)
                graphics.DrawImage(obj.Value.IsOpen ? World.Images["OpenBridge"] : World.Images["CloseBridge"],
                    obj.Value.Location.X, obj.Value.Location.Y, ObjectSize, ObjectSize);
            foreach (var obj in World.Objects.Switches)
                graphics.DrawImage(obj.Value.IsOpen ? World.Images["SwitcherOn"] : World.Images["SwitcherOff"],
                    obj.Value.Location.X, obj.Value.Location.Y, ObjectSize, ObjectSize);
            foreach (var obj in World.Objects.Patrons)
                graphics.DrawImage(World.Images["Patron"], obj.X, obj.Y, AnotherSize, AnotherSize);
        }

        private void DrawVisualEffects(Graphics graphics)
        {
            if (Shoot)
            {
                graphics.FillEllipse(new SolidBrush(Color.Yellow), World.Hero.X - 15, World.Hero.Y - 15, 30, 30);
                Shoot = false;
            }
            graphics.FillPolygon(new SolidBrush(Color.Black), World.BigPolygon);
            graphics.FillPolygon(new SolidBrush(Color.Black), World.SmallPolygon);
            graphics.DrawString(World.Patron.ToString(), Font, new SolidBrush(Color.Red), Size.Width - 40, Size.Height - 30);
        }

        private void DrawEnemies(Graphics graphics)
        {
            foreach (var enemy in World.Enemies)
                if (enemy.Value.IsDead)
                    graphics.DrawImage(World.Images["DeadEnemy"],
                         enemy.Value.Location.X, enemy.Value.Location.Y, CreatureSize, CreatureSize);
                else
                {
                    graphics.TranslateTransform(enemy.Value.Location.X + AnotherSize, enemy.Value.Location.Y + AnotherSize);
                    var angle = World.GetEnemyAngle(enemy.Value.Location);
                    graphics.RotateTransform((int)angle);
                    graphics.DrawImage(enemy.Value.Image, -AnotherSize, -AnotherSize, CreatureSize, CreatureSize);
                    graphics.RotateTransform(-(int)angle);
                    graphics.TranslateTransform(-enemy.Value.Location.X - AnotherSize, -enemy.Value.Location.Y - AnotherSize);
                }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            World.RemoveSun(e.X, e.Y);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button != MouseButtons.Left) return;
            if (World.Patron == 0) return;
            World.Shoot();
            Shoot = true;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (!IsWin && !World.IsHeroDead)
                World.ChangeVisualArea();
            Invalidate();
        }
    }
}
