using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;


namespace Digger
{

    public class Game
    {
        public static bool IsIt(int x, int y, CreatureType type)
        {
            if (Map[x, y] == null)
                return false;
            else
                return (Map[x, y].GetCreatureType() == type);
        }

        public static ICreature[,] Map;
        public static int MapWidth = 20;
        public static int MapHeight = 20;
        public static int Scores;
        public static bool IsOver;

        public static ICreature CreateCreature(CreatureType type)
        {
            return new Gold();
        }

        public static void CreateMap()
        {
            //TODO: Инициализируйте здесь карту
            MapWidth = 20;
            MapHeight = 20;
            Map = new ICreature[MapWidth, MapHeight];
            for (int i = 0; i < MapHeight; ++i)
                for (int j = 0; j < MapWidth; ++j)
                {
                    Map[i, j] = new Terrain();
                }
            for (int i = 0; i < 20; i++)
                Map[i, 10] = null;
            Map[9,9]= new Monster();
            Map[5, 5] = new Sack();
            Map[5, 4] = new Sack();
            Map[0, 0] = new Digger();
            Map[7, 18] = new Sack();
            Map[6, 9] = null;
            Map[6, 10] = new Monster();
           // Map[7, 10] = null;
            //Map[8, 10] = null;
           Map[7, 14] = new Monster();
            Map[7, 15] = null;
            Map[7, 16] = null;
            Map[8, 16] = new Monster();
            Map[10, 18] = new Monster();
            Map[9, 1] = new Monster();
            Map[7, 10] = null;
            Map[8, 10] = null;
            Map[9, 10] = null;
            Map[10, 10] = new Monster();
        }

    }

    public class Terrain : ICreature
    {
        public CreatureType GetCreatureType()
        {
            return CreatureType.Terrain;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() 
            {
                DeltaX = 0, 
                DeltaY = 0, 
                NextState = CreatureType.Terrain 
            };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetCreatureType() == CreatureType.Digger)
                return true;
            else
                return false;
        }
    }



    public class Digger : ICreature
    {
        struct Pair {public int x, y;}
        public CreatureType GetCreatureType()
        {
            return CreatureType.Digger;
        }

        Pair Mooving (int x, int y)
        {
            Pair pair = new Pair();
            pair.x = 0; pair.y = 0;
            if (Keyboard.IsKeyDown(Key.W) && y > 0)
                pair.y = -1;
            if (Keyboard.IsKeyDown(Key.S) && y < Game.MapHeight - 1)
                pair.y = 1;
            if (Keyboard.IsKeyDown(Key.A) && x > 0)
                pair.x = -1;
            if (Keyboard.IsKeyDown(Key.D) && x < Game.MapWidth - 1)
                pair.x = 1;
            if (pair.x != 0 && pair.y != 0)
            {
                pair.x = 0;
                //pair.y = 0;
            }
            return pair;
        }

        public CreatureCommand Act(int x, int y)
        {
            CreatureCommand Command =  new CreatureCommand() 
            {
                DeltaX = 0, 
                DeltaY = 0, 
                NextState = CreatureType.Digger 
            };
            Pair move = Mooving(x, y);
            Command.DeltaX = move.x;
            Command.DeltaY = move.y;
            if (Command.DeltaX == 0)
                if (Game.IsIt(x, y + Command.DeltaY, CreatureType.Sack))
                    {
                        Command.DeltaX = 0;
                        Command.DeltaY = 0;
                    }
            if (Command.DeltaY == 0)
                if (Game.IsIt(x + Command.DeltaX, y, CreatureType.Sack))
                        if ((x + 2 * Command.DeltaX >= Game.MapWidth || x + 2 * Command.DeltaX < 0) || (Game.Map[x + 2 * Command.DeltaX, y] != null))
                        {
                            Command.DeltaX = 0;
                            Command.DeltaY = 0;
                        }
            if (Game.IsIt(x + Command.DeltaX, y + Command.DeltaY, CreatureType.Gold))
                Game.Scores += 1;
            return Command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetCreatureType() == CreatureType.Monster
                    ||
                conflictedObject.GetCreatureType() == CreatureType.Sack)
            {
                Game.IsOver = true;
                return true;           
            }
            else
                return false;
        }
    }

    public class Sack : ICreature
    {
        public CreatureType GetCreatureType()
        {
            return CreatureType.Sack;
        }

        bool isFall = false;

        public int IsNeedToFall(int x, int y)
        {
            if (x + 1 < Game.MapWidth) 
                if (Game.IsIt(x + 1, y, CreatureType.Digger))
                    if (Keyboard.IsKeyDown(Key.A) && x > 0)
                        if (Game.Map[x - 1, y] == null)
                            return -1;
            if (x > 0)
                if (Game.IsIt(x - 1, y, CreatureType.Digger))
                    if (Keyboard.IsKeyDown(Key.D) && x < Game.MapWidth - 1)
                        if (Game.Map[x + 1, y] == null)
                            return 1;
            return 0;
        }

        public CreatureCommand Act(int x, int y)
        {           
            CreatureCommand Command = new CreatureCommand()
            {
                DeltaX = 0,
                DeltaY = 0,
                NextState = CreatureType.Sack
            };
            if (y < Game.MapHeight - 1)
            {
                if (Game.Map[x, y + 1] == null)
                {
                    isFall = true;
                    Command.DeltaY += 1;
                    if (y + Command.DeltaY + 1 >= Game.MapHeight)
                        Command.NextState = CreatureType.Gold;
                    else if (Game.IsIt(x, y + Command.DeltaY + 1, CreatureType.Terrain) || Game.IsIt(x, y + Command.DeltaY + 1, CreatureType.Gold))
                        Command.NextState = CreatureType.Gold;
                }
                else
                {
                    if (Game.Map[x, y + 1].GetCreatureType() == CreatureType.Monster)
                    {
                        Command.DeltaY += 1;
                        Command.NextState = CreatureType.Gold;
                    }
                    else if (isFall && Game.Map[x, y + 1].GetCreatureType() == CreatureType.Digger)
                    {
                        Command.DeltaY += 1;
                    }
                }
            }

            Command.DeltaX += IsNeedToFall(x, y + Command.DeltaY);
            return Command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }
    }

    public class Gold : ICreature
    {
        public CreatureType GetCreatureType()
        {
            return CreatureType.Gold;
        }

        public CreatureCommand Act(int x, int y)
        {
            int startHeight = y;
            CreatureCommand Command = new CreatureCommand()
            {
                DeltaX = 0,
                DeltaY = 0,
                NextState = CreatureType.Gold
            };
            return Command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetCreatureType() == CreatureType.Digger)
                return true;
            else
                return false;
        }
    }

    public class Monster : ICreature
    {
        struct Pair {public int x, y;}

        bool[,] map = new bool[Game.MapHeight, Game.MapWidth];
        Pair[,] length = new Pair[Game.MapHeight , Game.MapWidth];

        Pair BFS(Pair[] start, int x, int y)
        {
            if (start.Length == 0)
                return new Pair {x = -1, y = -1};
            List<Pair> list = new List<Pair>();
            for (int l = 0; l < start.Length; ++l)
                for (int i = start[l].y - 1; i <= start[l].y + 1; ++i)
                    for (int j = start[l].x - 1; j <= start[l].x + 1; ++j)
                        if (i >= 0 && i < Game.MapHeight && j >= 0 && j < Game.MapWidth && (j == start[l].x || i == start[l].y))
                            if (!map[i, j])
                            {
                                if (Game.IsIt(j, i, CreatureType.Monster) && j == x && i == y)
                                        continue;
                                if (Game.IsIt(j, i, CreatureType.Digger))
                                {
                                    int mayBeX = start[l].x, mayBeY = start[l].y;
                                    while (true)
                                    {
                                        int localX = length[mayBeY, mayBeX].x;
                                        int localY = length[mayBeY, mayBeX].y;
                                        if (localX == x && localY == y)
                                            return new Pair { y = mayBeX, x = mayBeY};
                                        mayBeX = localX; mayBeY = localY;
                                        if (mayBeY == -1 || mayBeX == -1)
                                            return new Pair { y = j, x = i };
                                    }
                                }
                                if (Game.Map[j, i] == null || Game.IsIt(j, i, CreatureType.Monster))
                                {
                                    length[i, j].x = start[l].x;
                                    length[i, j].y = start[l].y;
                                    list.Add(new Pair { y = i, x = j });
                                    map[i, j] = true;
                                }
                            }
            return BFS(list.ToArray(), x, y);
        }   

        public CreatureType GetCreatureType()
        {
            return CreatureType.Monster;
        }

        public CreatureCommand Act(int x, int y)
        {
            int startHeight = y;
            CreatureCommand Command = new CreatureCommand()
            {
                DeltaX = 0,
                DeltaY = 0,
                NextState = CreatureType.Monster
            };
            Pair[] startPoint = new Pair[1];
            startPoint[0].y = y;
            startPoint[0].x = x;
            for (int i = 0; i < Game.MapHeight; ++i)
                for (int j = 0; j < Game.MapWidth; ++j)
                {
                    length[i, j].x = -1;
                    length[i, j].y = -1;
                    map[i, j] = false;
                }
            Pair answer = BFS(startPoint, x, y);
            if (answer.x != -1)
            {
                Command.DeltaX = answer.y - x;
                Command.DeltaY = answer.x - y;
            }
            return Command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetCreatureType() == CreatureType.Sack 
                     ||
                conflictedObject.GetCreatureType() == CreatureType.Gold /*
                     ||
                conflictedObject.GetCreatureType() == CreatureType.Monster*/)
                return true;
            else
                return false;
        }
    }
}
