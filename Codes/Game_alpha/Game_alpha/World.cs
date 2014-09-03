using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;

namespace Game_alpha
{
    public class World
    {
        public Dictionary<string, Image> Images = Directory.EnumerateFiles("images", "*.png")
            .ToDictionary(Path.GetFileNameWithoutExtension, Image.FromFile);
        public Dictionary<string, SoundPlayer> Media;
        public Point Hero;
        public Point Sun = new Point();
        public Point Exit;
        public WorldObjects Objects = new WorldObjects();
        public Point[] BigPolygon = new Point[9];
        public Point[] SmallPolygon = new Point[4];
        public Dictionary<int, Enemy> Enemies;
        public bool IsHeroDead;
        public bool IsWin;
        public int Patron;

        public bool IsIntersect(Point point, int dX, int dY)
        {
            return (dX >= point.X && dX <= point.X + 50 && dY >= point.Y && dY <= point.Y + 50);
        }

        public bool IsIntersectWall(Point upLeft, Point downRight, int dX, int dY)
        {
            return (dX >= upLeft.X && dX <= downRight.X && dY >= upLeft.Y && dY <= downRight.Y);
        }

        public void ReadWorld()
        {
            ReadWalls();
            ReadCreatures();
            ReadActiveObject();

            var sounds = Directory.EnumerateFiles("media", "*.wav").ToList();
            Media = new Dictionary<string, SoundPlayer>();
            foreach (var m in sounds)
                Media[Path.GetFileNameWithoutExtension(m) ?? ""] = new SoundPlayer(m);
        }

        private void ReadActiveObject()
        {
            var lines = File.ReadLines("activobjects.txt").ToList();
            var objAmount = int.Parse(lines[0]);
            for (var i = 0; i < objAmount; ++i)
            {
                var point = lines[1 + i].Split(' ').Select(int.Parse).ToList();
                Objects.Bridges[i] = new Bridge(new Point(point[0], point[1]));
                Objects.Switches[i] = new Switch(new Point(point[2], point[3]));
            }
            var exit = lines[objAmount + 1].Split(' ').Select(int.Parse).ToList();
            Exit = new Point(exit[0], exit[1]);
            var patronAmount = int.Parse(lines[objAmount + 2]);
            for (var i = 0; i < patronAmount; ++i)
            {
                var patron = lines[i + objAmount + 3].Split(' ').Select(int.Parse).ToList();
                Objects.Patrons.Add(new Point(patron[0], patron[1]));
            }
        }

        private void ReadCreatures()
        {
            var lines = File.ReadLines("creatures.txt").ToList();
            var hero = lines[0].Split(' ').Select(int.Parse).ToList();
            Hero = new Point(hero[0], hero[1]);
            Patron = hero[2];
            var enemyCount = int.Parse(lines[1]);
            Enemies = new Dictionary<int, Enemy>();
            for (var i = 0; i < enemyCount; ++i)
            {
                var info = lines[2 + i].Split(' ').ToList();
                var point = new Point(int.Parse(info[0]), int.Parse(info[1]));
                Enemies[i] = new Enemy(point, info[2], this);
            }
        }

        private void ReadWalls()
        {
            var lines = File.ReadLines("walls.txt").ToList();
            var wallAmount = int.Parse(lines[0]);
            for (var i = 0; i < wallAmount; ++i)
            {
                var point = lines[1 + i].Split(' ').Select(int.Parse).ToList();
                Objects.Walls.Add(new Tuple<Point, Point>(new Point(point[0], point[1]), new Point(point[2], point[3])));
            }
        }

        internal void RemoveSun(int x, int y)
        {
            Sun.X = x;
            Sun.Y = y;
        }

        internal void ChangeVisualArea()
        {
            MoveEnemies();
            if (Enemies.Any(x => CanEatHero(x.Value)))
                IsHeroDead = true;
            double width = Sun.X - Hero.X;
            double height = Sun.Y - Hero.Y;
            var len = Math.Sqrt(width * width + height * height);
            var delta = len / 400;
            width /= delta;
            height /= delta;
            SmallPolygon = GetSmallPolygon(width, height);
            BigPolygon = GetBigPolygon(width, height);
        }

        private bool CanEatHero(Enemy enemy)
        {
            if (!enemy.IsDead)
                return (Hero.X >= enemy.Location.X && Hero.X <= enemy.Location.X + 32 &&
                     Hero.Y >= enemy.Location.Y && Hero.Y <= enemy.Location.Y + 32);
            return false;
        }

        private Point[] GetBigPolygon(double width, double height)
        {

            if (Sun.Y > Hero.Y)
            {
                return new[]
                {
                    new Point(0, 0),
                    new Point(2000, 0),
                    new Point(2000, 1200),
                    new Point((int) (Hero.X + width + height/2), 1200),
                    new Point((int) (Hero.X + width + height/2), (int) (Hero.Y + height - width/2)),
                    new Point(Hero.X, Hero.Y),
                    new Point((int) (Hero.X + width - height/2), (int) (Hero.Y + height + width/2)),
                    new Point((int) (Hero.X + width - height/2), 1200),
                    new Point(0, 1200)
                };
            }
            return new[]
            {
                new Point(0, 0),
                new Point(0, 1200),
                new Point(2000, 1200),
                new Point(2000, 0),
                new Point((int) (Hero.X + width - height/2), 0),
                new Point((int) (Hero.X + width - height/2), (int) (Hero.Y + height + width/2)),
                new Point(Hero.X, Hero.Y),
                new Point((int) (Hero.X + width + height/2), (int) (Hero.Y + height - width/2)),
                new Point((int) (Hero.X + width + height/2), 0)
            };
        }

        private Point[] GetSmallPolygon(double width, double height)
        {
            if (Sun.Y > Hero.Y)
            {
                return new[]
                {
                    new Point((int) (Hero.X + width + height/2), 1200),
                    new Point((int) (Hero.X + width + height/2), (int) (Hero.Y + height - width/2)),
                    new Point((int) (Hero.X + width - height/2), (int) (Hero.Y + height + width/2)),
                    new Point((int) (Hero.X + width - height/2), 1200)
                };
            }
            return new[]
            {
                new Point((int) (Hero.X + width - height/2), 0),
                new Point((int) (Hero.X + width - height/2), (int) (Hero.Y + height + width/2)),
                new Point((int) (Hero.X + width + height/2), (int) (Hero.Y + height - width/2)),
                new Point((int) (Hero.X + width + height/2), 0)
            };
        }

        public void MoveHero(int deltaX, int deltaY, int mapWidth, int mapHeight)
        {
            if (Objects.Walls.Any(x => IsIntersectWall(x.Item1, x.Item2, Hero.X + deltaX, Hero.Y + deltaY))) return;
            if (Objects.Bridges.Any(x => !x.Value.IsOpen && IsIntersect(x.Value.Location, Hero.X + deltaX, Hero.Y + deltaY))) return;
            if (Hero.X + deltaX <= 0 || Hero.Y + deltaY <= 0 || Hero.X + deltaX >= mapWidth || Hero.Y + deltaY >= mapHeight) return;
            Hero.X += 2 * deltaX;
            Hero.Y += 2 * deltaY;
        }

        private void MoveEnemies()
        {
            var temporaryHold = new Dictionary<int, Enemy>();
            while (Enemies.Any(x => IsEnemySeeHero(x.Value)))
                foreach (var enemy in Enemies.Where(x => IsEnemySeeHero(x.Value)))
                {
                    if (
                        Objects.Walls.Any(
                            x => 
                            IsIntersectWall(x.Item1, x.Item2, enemy.Value.Location.X + 1, enemy.Value.Location.Y + 16)) ||
                        Objects.Bridges.Any(
                            x =>
                                !x.Value.IsOpen &&
                                IsIntersect(x.Value.Location, enemy.Value.Location.X + 16, enemy.Value.Location.Y + 16)))
                        temporaryHold[enemy.Key] = GetRemovedEnemy(enemy.Value, 2);
                    else
                        temporaryHold[enemy.Key] = GetRemovedEnemy(enemy.Value, 4);
                    Enemies.Remove(enemy.Key);
                    break;
                }
            foreach (var newEnemy in temporaryHold)
                Enemies[newEnemy.Key] = newEnemy.Value;
        }

        private Enemy GetRemovedEnemy(Enemy value, int delta)
        {
            return new Enemy(
                            new Point(
                                value.Location.X - delta * Math.Sign(value.Location.X + 16 - Hero.X),
                                value.Location.Y - delta * Math.Sign(value.Location.Y + 16 - Hero.Y)),
                            value.Name, this)
            { Hp = value.Hp };
        }

        private bool IsEnemySeeHero(Enemy enemy)
        {
            if (!enemy.IsDead)
                return Math.Sqrt(Math.Pow(enemy.Location.X - Hero.X, 2) + Math.Pow(enemy.Location.Y - Hero.Y, 2)) < 500;
            return false;
        }

        public void ActivateSomething()
        {
            if (IsIntersect(Exit, Hero.X, Hero.Y))
                IsWin = true;
            foreach (var obj in Objects.Switches.Where(obj => IsIntersect(obj.Value.Location, Hero.X, Hero.Y)))
            {
                obj.Value.SwitchState();
                Objects.Bridges[obj.Key].SwitchState();
                Media["Switch"].Play();
            }
            foreach (var obj in Objects.Patrons.Where(obj => IsIntersect(obj, Hero.X, Hero.Y)))
            {
                Objects.Patrons.Remove(obj);
                Patron += 10;
                break;
                //Media["Switch"].Play();
            }
        }

        public void Shoot()
        {
            Patron -= 1;
            if (!IsHeroDead)
                Media["Shoot"].Play();
            if (Math.Sqrt(Math.Pow(Hero.X - Sun.X, 2) + Math.Pow(Hero.Y - Sun.Y, 2)) >= 400) return;
            var temporaryHold = new Dictionary<int, Enemy>();
            foreach (var enemy in Enemies.Where(x => IsCanKill(x.Value.Location)))
            {
                var newEnemy = enemy.Value;
                newEnemy.Hp -= 1;
                if (newEnemy.Hp == 0)
                    newEnemy.SwitchState();
                temporaryHold[enemy.Key] = newEnemy;
                Enemies.Remove(enemy.Key);
                break;
            }
            foreach (var newEnemy in temporaryHold)
                Enemies[newEnemy.Key] = newEnemy.Value;
        }

        private bool IsCanKill(Point enemy)
        {
            return (Sun.X >= enemy.X && Sun.X <= enemy.X + 32 && Sun.Y >= enemy.Y && Sun.Y <= enemy.Y + 32);
        }

        public double GetEnemyAngle(Point enemy)
        {
            if (Hero.X - enemy.X - 16 == 0)
            {
                if (Hero.Y > enemy.Y + 16)
                    return -90;
                return 90;
            }
            var angle = Math.Atan((double)Math.Abs(Hero.Y - enemy.Y - 16) / Math.Abs(Hero.X - enemy.X - 16));
            if (Hero.X - enemy.X - 16 > 0)
            {
                if (Hero.Y - enemy.Y - 16 > 0)
                    return 180 + angle*(180/Math.PI);
                return 180 - angle * (180 / Math.PI);
            }
            if (Hero.Y - enemy.Y - 16 > 0)
                return - angle * (180 / Math.PI);
            return angle * (180 / Math.PI);
        }
    }
}
