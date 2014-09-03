using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CollectionsLinkedList
{
    public static class LinkedList
    {
        
        public interface IRepositoryLinkedList<T>
        {
            void AddFirst(T item);
            void AddLast(T item);
            bool Contains(T item);
        }

        public class LinkedListRepository<T> : IRepositoryLinkedList<T>
        {
            LinkedList<T> list = new LinkedList<T>();

            public void AddFirst(T item)
            {
                list.AddFirst(item);
            }

            public void AddLast(T item)
            {
                list.AddLast(item);
            }

            public bool Contains(T item)
            {
                return list.Contains(item);
            }
        }

        public static void TestLinkedList()
        {
            MeasureLinkedList(new LinkedListRepository<int>(), TestAddFirst);
        }

        public static void MeasureLinkedList(IRepositoryLinkedList<int> linkedList, Action<IRepositoryLinkedList<int>, int> run)
        {
            int AmountOfTestElement = 1000;
            Console.WriteLine("LinkedList: additional test");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            run(linkedList, 1000 * AmountOfTestElement); // возможно, 10000 — слишком мало?
            stopWatch.Stop();
            var timeSpan = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(timeSpan);

            stopWatch.Restart();
            TestContains(linkedList, AmountOfTestElement);
            stopWatch.Stop();
            timeSpan = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(timeSpan);

            stopWatch.Restart();
            TestAddLast(linkedList, 1000 * AmountOfTestElement);
            stopWatch.Stop();
            timeSpan = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(timeSpan);
            Console.WriteLine();
        }
        static void TestAddFirst(IRepositoryLinkedList<int> list, int count)
        {
            for (int i = 0; i < count; ++i)
                list.AddFirst(i);
        }

        static void TestAddLast(IRepositoryLinkedList<int> list, int count)
        {
            for (int i = 0; i < count; ++i)
                list.AddLast(i);
        }

        static void TestContains(IRepositoryLinkedList<int> list, int count)
        {
            var containsAll = true;
            for (int i = 0; i < count; i++)
                containsAll &= list.Contains(i);
            Debug.Assert(containsAll);
        }
    }
}
