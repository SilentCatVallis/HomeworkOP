using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public static class Program
    {
        private static readonly int[,] FinishPosition = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
        private static readonly int[] Dx = {0, 0, 1, -1};
        private static readonly int[] Dy = {1, -1, 0, 0};

        [STAThread]
        static void Main()
        {
            var startPosition = ReadStartPosition();
            List<string> steps;
            if (IsEqual(startPosition, FinishPosition))
                steps = new List<string>(new[] { "012345678" });
            else
                steps = AssembleThePuzzle(startPosition) ?? new List<string>(new[] {"000000000"});

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(steps.Select(FromHash).Reverse().ToList()));
        }

        public static int[,] FromHash(string hash)
        {
            var field = new int[3, 3];
            for (var index = 0; index < hash.Length; index++)
            {
                field[index/3, index%3] = int.Parse(hash[index].ToString());
            }
            return field;
        }

        public static List<string> AssembleThePuzzle(int[,] startPosition)
        {
            var stepsGraph = new Dictionary<string, string>();
            var visitedStates = new HashSet<string>(new[] {GetHash(startPosition)});
            var queue = new Queue<Tuple<int[,], int>>();
            queue.Enqueue(Tuple.Create(startPosition, 0));
            while (queue.Count > 0)
            {
                var localPosition = queue.Dequeue();
                foreach (var neighbourState in GetAllNextStates(localPosition.Item1)
                    .Where(x => !visitedStates.Contains(GetHash(x))))
                {
                    var hash = GetHash(neighbourState);
                    stepsGraph[hash] = GetHash(localPosition.Item1);
                    if (IsEqual(neighbourState, FinishPosition))
                        return GetPath(stepsGraph, startPosition);
                    visitedStates.Add(hash);
                    queue.Enqueue(Tuple.Create(neighbourState, localPosition.Item2 + 1));
                }
            }
            return null;
        }

        public static string GetHash(int[,] field)
        {
            var hash = new StringBuilder();
            for (var row = 0; row < 3; row++)
                for (var column = 0; column < 3; column++)
                    hash.Append(field[row, column]);
            return hash.ToString();
        }

        private static List<string> GetPath(Dictionary<string, string> stepsGraph, int[,] startPosition)
        {
            var answer = new List<string>();
            var startHash = GetHash(startPosition);
            var localPosition = GetHash(FinishPosition);
            while (localPosition != startHash)
            {
                answer.Add(localPosition);
                localPosition = stepsGraph[localPosition];
            }
            answer.Add(localPosition);
            return answer;
        }

        public static bool IsEqual(int[,] localPosition, int[,] finishPosition)
        {
            for (var row = 0; row < 3; row++)
                for (var column = 0; column < 3; ++column)
                    if (localPosition[row, column] != finishPosition[row, column])
                        return false;
            return true;
        }

        public static IEnumerable<int[,]> GetAllNextStates(int[,] localPosition)
        {
            var zero = GetZeroLocation(localPosition);
            for (var delta = 0; delta < 4; delta++)
                if (IsNotAwayField(zero, delta))
                    yield return GetMovedSquare(zero, delta, localPosition);
        }

        private static int[,] GetMovedSquare(Point zero, int delta, int[,] localPosition)
        {
            var answer = new int[3, 3];
            for (var row = 0; row < 3; row++)
                for (var column = 0; column < 3; column++)
                    answer[row, column] = localPosition[row, column];
            answer[zero.Y, zero.X] = localPosition[zero.Y + Dy[delta], zero.X + Dx[delta]];
            answer[zero.Y + Dy[delta], zero.X + Dx[delta]] = 0;
            return answer;
        }

        public static bool IsNotAwayField(Point zero, int delta)
        {
            return (zero.X + Dx[delta] >= 0 && zero.X + Dx[delta] < 3 &&
                    zero.Y + Dy[delta] >= 0 && zero.Y + Dy[delta] < 3);
        }

        public static Point GetZeroLocation(int[,] localPosition)
        {
            for (var row = 0; row < 3; row++)
                for (var column = 0; column < 3; ++column)
                    if (localPosition[row, column] == 0)
                    {
                        return new Point(column, row);
                    }
            return new Point(-1, -1);
        }

        private static int[,] ReadStartPosition()
        {
            try
            {
                var startPosition = new int[3, 3];
                for (var row = 0; row < 3; row++)
                    for (var column = 0; column < 3; column++)
                        startPosition[row, column] = File.ReadAllLines("field.txt")
                            .Select(x => x.Split(' ')
                                .Select(int.Parse).ToArray())
                            .ToArray()[row][column];
                return startPosition;
            }
            catch
            {
                return new[,] {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}};
            }
        }
    }
}
