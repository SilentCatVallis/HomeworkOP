using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CollectionsLinkedList;
using CollectionsHashSet;

namespace Сollections
{
	// Интерфейс определяет, что можно делать с репозиторием
	public interface IRepository<T>
	{
		void Add(T item);
		bool Contains(T item);
		T GetElementAt(int index);
	}

	// Конкретная реализация репозитория
	public class ListRepository<T> : IRepository<T>
	{
		IList<T> list = new List<T>();

		public void Add(T item)
		{
			list.Add(item);
		}

		public bool Contains(T item)
		{
			return list.Contains(item);
		}

		public T GetElementAt(int index)
		{
			return list[index];
		}
	}


    public class LinkedListRepository<T> : IRepository<T>
    {
        LinkedList<T> list = new LinkedList<T>();

        public void Add(T item)
        {
            list.AddFirst(item);
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public T GetElementAt(int index)
        {
            int count = -1;
            foreach (var e in list)
            {
                count++;
                if (count == index)
                    return e;
            }
            throw new Exception();
        }
    }

    public class HashSetRepository<T> : IRepository<T>
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

        public T GetElementAt(int index)
        {
            int count = -1;
            foreach (var e in list)
            {
                count++;
                if (count == index)
                    return e;
            }
            throw new Exception();
        }
    }

	static class Program
	{
        static public void Benchmark(IRepository<int> list, int count)
        {
            MeasureList(list, TestAppendToList, count * 10000);
            MeasureList(list, TestContains, count * 100);
            MeasureList(list, TestSequentialAccess, count * 100);
            Console.WriteLine();
        }

		static void Main(string[] args)
        {
            Console.WriteLine("First test run list");
            Benchmark(new ListRepository<int>(), 10);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Main tests");
            
            Console.WriteLine("List:");
            var list = new ListRepository<int>();
            MeasureList(list, TestAppendToList, 10000000);
            MeasureList(list, TestContains, 50000);
            MeasureList(list, TestSequentialAccess, 10000000);
            //Benchmark(new ListRepository<int>(), 1000);

            Console.WriteLine("LinkedList:");
            var LinkedList = new LinkedListRepository<int>();
            MeasureList(LinkedList, TestAppendToList, 1000000);
            MeasureList(LinkedList, TestContains, 1000);
            MeasureList(LinkedList, TestSequentialAccess, 10000);
            //Benchmark(new LinkedListRepository<int>(), 100);

            Console.WriteLine("HashSet:");
            var hashSet = new HashSetRepository<int>();
            MeasureList(hashSet, TestAppendToList, 10000000);
            MeasureList(hashSet, TestContains, 10000000);
            MeasureList(hashSet, TestSequentialAccess, 10000);
            //Benchmark(new HashSetRepository<int>(), 1000);

            //LinkedList.TestLinkedList();
            //HashSet.TestHashSet();
            // Задача 2. Добавить тестирование других методов Repository:
            // Задача 3. Добавить новую реализацию IRepository, 
            //			 основанную на LinkedList<int>, вместо List<int>
            // Задача 4. Придумать ещё какую-нибудь (любую) реализацию IRepository.
            //			 Можете попробовать какую-то собственную оригинальную идею.
        }

		static void MeasureList(IRepository<int> list, Action<IRepository<int>, int> run, int count)
		{
			// Задача 1. С помощью класса Stopwatсh замерить длительность работы run.
			// Вывести информацию о выполненном действии и его длительности.
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
			run(list, count); // возможно, 10000 — слишком мало?
            stopWatch.Stop();
            var timeSpan = (double)stopWatch.ElapsedTicks / Stopwatch.Frequency;
            Console.Write(timeSpan + " Seconds, ");
            Console.WriteLine(count / timeSpan + " Operation per seconds");
           
		}

		static void TestAppendToList(IRepository<int> list, int count)
		{
			for (int i = 0; i < count; i++)
				list.Add(i);
		}

		static void TestContains(IRepository<int> list, int count)
		{
			var containsAll = true;
			for (int i = 0; i < count; i++)
				containsAll &= list.Contains(i);
			Debug.Assert(containsAll); // это проверка, не нарушилось ли какое-то условие консистентности
		}

		static void TestSequentialAccess(IRepository<int> list, int count)
		{
			var sum = 0;
			for(int i=0; i<count; i++)
				sum += list.GetElementAt(i);
			Debug.Assert(sum != 0); 
		}
	}
}
