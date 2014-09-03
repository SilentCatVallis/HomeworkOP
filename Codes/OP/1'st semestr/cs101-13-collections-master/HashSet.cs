using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CollectionsHashSet
{
    public static class HashSet
    {
        public interface IRepositoryHashSet<T>
        {
            void Add(T item);
            void Remove(T item);
            bool Contains(T item);
        }

        public class HashSetRepository<T> : IRepositoryHashSet<T>
        {
            HashSet<T> list = new HashSet<T>();

            public void Add(T item)
            {
                list.Add(item);
            }

            public bool Contains(T item)
            {
                return list.Contains(item);
            }

            public void Remove(T item)
            {
                list.Remove(item);
            }
        }

        public static void TestHashSet()
        {
            MeasureHashSet(new HashSetRepository<int>(), TestAdd);
        }

        public static void MeasureHashSet(IRepositoryHashSet<int> hashSet, Action<IRepositoryHashSet<int>, int> run)
        {
            int AmountOfTestElenent = 10000000;
            Console.WriteLine("HashSet: additional test");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            run(hashSet, AmountOfTestElenent); // возможно, 10000 — слишком мало?
            stopWatch.Stop();
            var timeSpan = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(timeSpan);

            stopWatch.Restart();
            TestContains(hashSet, AmountOfTestElenent);
            stopWatch.Stop();
            timeSpan = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(timeSpan);

            stopWatch.Restart();
            TestRemove(hashSet, AmountOfTestElenent);
            stopWatch.Stop();
            timeSpan = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(timeSpan);
            Console.WriteLine();
        }

        static void TestAdd(IRepositoryHashSet<int> list, int count)
        {
            for (int i = 0; i < count; ++i)
                list.Add(i);
        }

        static void TestRemove(IRepositoryHashSet<int> list, int count)
        {
            for (int i = 0; i < count; ++i)
                list.Remove(i);
        }

        static void TestContains(IRepositoryHashSet<int> list, int count)
        {
            var containsAll = true;
            for (int i = 0; i < count; i++)
                containsAll &= list.Contains(i);
            Debug.Assert(containsAll);
        }
    }
}
