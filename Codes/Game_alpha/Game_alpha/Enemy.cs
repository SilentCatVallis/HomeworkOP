using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_alpha
{
    public class Enemy
    {
        public Enemy(Point point, string name, World world)
        {
            Location = point;
            if (name == "zombi")
            {
                Image = world.Images["Zombi"];
                Hp = 1;
            }
            else if (name == "pig")
            {
                Image = world.Images["Pig"];
                Hp = 3;
            }
            else
            {
                Image = world.Images["Boss"];
                Hp = 8;
            }
            Name = name;
        }

        public string Name;
        public Point Location;
        public bool IsDead = false;
        public Image Image;
        public int Hp;

        public void SwitchState()
        {
            IsDead = !IsDead;
        }

        public Enemy Shoot(World world)
        {
            var newEnemy = new Enemy(Location, Name, world) {Hp = Hp - 1, IsDead = IsDead};
            if (newEnemy.Hp == 0)
                newEnemy.SwitchState();
            return newEnemy;
        }
    }
}
