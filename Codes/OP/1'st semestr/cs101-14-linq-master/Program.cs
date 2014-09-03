using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using LinqTasks;

namespace LinqTasks
{
	static class DataAnalysis
	{
        public static double last = 0;
        public static double[] array = new double[3] { 0.5, 0.9, 0.95 };

        public static IEnumerable<double> ExponentialSmooth(this IEnumerable<double> x, double a)
        {
            last = 0;
            var count = -1;
            foreach (var i in x)
            {
                count++;
                last = i * a + (1 - a) * last;
                if (count == 0)
                    yield return i;
                yield return last;
            }
        }

        public static IEnumerable<DataPoint> ToHistogramData(this IList<int> data)
        {
            int count = 0;
            foreach (var i in data)
            {
                count++;
                yield return new DataPoint(count, i);
            }
        }
	}

	static class Program
	{
        public enum Data
        {
            Calendar,
            Load,
        }

		private const string calendarPath = "calendar.csv";
		private const string loadPath = "load.csv";

        struct Pair
        {
            public int amount;
            public int total;
        }

        static Pair workday, freeday;
        public static Dictionary<DateTime, Statistic> calendar;

        public class Statistic
        {
            public DateTime date;
            public int number;
        }

        public static Statistic CreateStat(string[] arr, Data data)
        {
            Statistic stat = new Statistic();
            stat.number = int.Parse(arr[1]);
            var preDate = arr[0].Split( '.' , '-' ).Select(int.Parse).ToArray();
            DateTime date;
            if (data == Data.Calendar)
                date = new DateTime(preDate[2], preDate[1], preDate[0]);
            else
                date = new DateTime(preDate[0], preDate[1], preDate[2]);
            stat.date = date;
            return stat;
        }

        public static List<Statistic> PrepareData(Data data)
        {
            string path;
            if (data == Data.Calendar)
                path = calendarPath;
            else
                path = loadPath;
            return File.ReadAllLines(path).Where(x => !x.StartsWith("#"))
                                          .Select(x => x.Split( '\t', ';' ))
                                          .Select(x => CreateStat(x, data)).ToList();
        }

        public static IEnumerable<Statistic> CalculateStatistic (this IEnumerable<Statistic> stat)
        {
            foreach (var e in stat)
            if (calendar.ContainsKey(e.date))
            {
                if (calendar[e.date].number == 0)
                {
                    workday.amount++;
                    workday.total += e.number;
                }
                else
                {
                    freeday.amount++;
                    freeday.total += e.number;
                }
                yield return e;
            }
        }

        static public double CalculateAverage(List<Statistic> stat, double type)
        {
            return stat.Where(x => calendar[x.date].number == type).Average(y => y.number);
        }
		static void Main(string[] args)
		{
            var load = PrepareData(Data.Load);
            calendar = PrepareData(Data.Calendar).ToDictionary(z => z.date);

            var maxLoad = load.Max(x => x.number);
            var minLoad = load.Min(x => x.number);
            var numberOfColumns = Math.Log(maxLoad - minLoad, 2) + 1;
            var step = (maxLoad - minLoad) / numberOfColumns;

            var averageForFreeDay = CalculateAverage(load, 1);
            var averageForWorkDay = CalculateAverage(load, 0);

            Console.WriteLine("FreeDay: " + averageForFreeDay);
            Console.WriteLine("WorkDay: " + averageForWorkDay);

            var list = load.Select(x => (int)x.number)
                           .GroupBy(x => (int)(x / step))
                           .OrderBy(x => x.Key)
                           .Select(x => x.ToArray())
                           .Select(x => x.Length)
                           .ToList();

            Chart.ShowHistogram("Load histogram", list.ToHistogramData());
            Chart.ShowLines("Testing", DataAnalysis.array.Select(x => list.Select(y => (double)y).ExponentialSmooth(x)).ToArray());
            Chart.ShowLines("Testing", DataAnalysis.array.Select(x => load.Select(y => (double)y.number).ExponentialSmooth(x)).ToArray());
		}
	}
}
