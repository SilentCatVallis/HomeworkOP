using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class CreatureAnimation
    {
        public ICreature Creature;
        public CreatureCommand Command;
        public Point Location;
    }

    public class DiggerWindow : Form
    {
        const int ElementSize = 32;
        Dictionary<CreatureType, Bitmap> bitmaps = new Dictionary<CreatureType, Bitmap>();
        static List<CreatureAnimation> animations = new List<CreatureAnimation>();


        public DiggerWindow()
        {
            ClientSize = new Size(ElementSize * Game.MapWidth, ElementSize * Game.MapHeight + ElementSize);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Text = "Digger";
            DoubleBuffered = true;
            foreach (var e in Enum.GetValues(typeof(CreatureType)))
                bitmaps[(CreatureType)e] = (Bitmap)Bitmap.FromFile("Images\\" + e.ToString() + ".png");
            var timer = new Timer();
            timer.Interval = 1;
            timer.Tick += TimerTick;
            timer.Start();
        }

        void Act()
        {
            animations.Clear();
            for (int x = 0; x < Game.MapWidth; x++)
                for (int y = 0; y < Game.MapHeight; y++)
                {
                    var creature = Game.Map[x, y];
                    if (creature == null) continue;
                    var command = creature.Act(x,y);
                    animations.Add(new CreatureAnimation
                    {
                        Command=command,
                        Creature = creature,
                        Location = new Point(x * ElementSize, y * ElementSize)
                    });
                }
            animations = animations.OrderByDescending(z => (int)z.Creature.GetCreatureType()).ToList();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0,ElementSize);
            e.Graphics.FillRectangle(Brushes.Black,0,0,ElementSize*Game.MapWidth,ElementSize*Game.MapHeight);
            foreach(var a in animations)
                e.Graphics.DrawImage(bitmaps[a.Creature.GetCreatureType()],a.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(Game.Scores.ToString(), new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        int tickCount = 0;

        void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0) Act();
            foreach (var e in animations)
                e.Location = new Point(e.Location.X + 4*e.Command.DeltaX, e.Location.Y + 4*e.Command.DeltaY);
            if (tickCount==7)
            {
                for (int x=0;x<Game.MapWidth;x++) for (int y=0;y<Game.MapHeight;y++) Game.Map[x,y]=null;
                foreach(var e in animations)
                {
                    var x=e.Location.X/32;
                    var y=e.Location.Y/32;
                    var nextCreature = e.Creature;
                    if (e.Creature.GetCreatureType() != e.Command.NextState)
                        nextCreature = Game.CreateCreature(e.Command.NextState);

                    if (Game.Map[x, y] == null) Game.Map[x, y] = nextCreature;
                    else
                    {
                        bool newDead = nextCreature.DeadInConflict(Game.Map[x, y]);
                        bool oldDead = Game.Map[x, y].DeadInConflict(nextCreature);
                        /*if (Game.IsOver)
                        {
                            //throw new Exception(string.Format("Game Over"));
                            //MessageBox.Show("Game Over", "Game Over", MessageBoxButtons.OK);
                        }*/
                        if (newDead && oldDead)
                            Game.Map[x, y] = null;
                        else if (!newDead && oldDead)
                            Game.Map[x, y] = nextCreature;
                        //else if (!newDead && !oldDead)
                         //   throw new Exception(string.Format("Существа {0} и {1} претендуют на один и тот же участок карты", nextCreature.GetCreatureType(), Game.Map[x, y].GetCreatureType()));
                    }

                }
            }
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}
