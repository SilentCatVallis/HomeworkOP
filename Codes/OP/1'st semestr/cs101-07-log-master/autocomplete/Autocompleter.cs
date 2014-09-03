using System;
using System.Collections.Generic;

namespace autocomplete
{
	// Внимание!
	// Есть одна распространенная ловушка при сравнении строк: строки можно сравнивать по-разному:
	// с учетом регистра, без учета, зависеть от кодировки и т.п.
	// В файле словаря все слова отсортированы мтодом StringComparison.OrdinalIgnoreCase.
	// Во всех функциях сравнения строк в C# можно передать способ сравнения.
	class Autocompleter
	{
		private readonly string[] items;

		public Autocompleter(string[] loadedItems)
		{
			items = loadedItems;
		}

		// Найти произвольный элемент словаря, начинающийся с prefix.
		// Ускорьте эту фунцию так, чтобы она работала за O(log(n))

        public static int Comparator(string name, string prefix)
        {
            return String.Compare(name.Substring(0, Math.Min(prefix.Length, name.Length)), prefix, StringComparison.OrdinalIgnoreCase);
        }

        public class StringComp : IComparer<String>   // компаратор сравнения
        {
            public int Compare(string x, string y)
            {
                //return Comparator(x, y);
                return String.Compare(x.Substring(0, Math.Min(y.Length, x.Length)), y, StringComparison.OrdinalIgnoreCase);
            }
        }

		public string FindByPrefix(string prefix)
		{
            if (prefix == "")
                return null;
            StringComp StrComp = new StringComp();
            int result = Array.BinarySearch(items, prefix, StrComp);
            if (result >= 0)
                return items[result];
            else
                return null;
		}

		// Найти первые (в порядке следования в файле) 10 (или меньше, если их меньше 10) элементов словаря, 
		// начинающийся с prefix.
		// Эта функция должна работать за O(log(n) + count)
		public string[] FindByPrefix(string prefix, int count)
		{
            if (prefix == "")
                return new string[0];
            int left = 0;
            int right = items.Length - 1;
            int mid;
            while (left <= right)
            {
                if (left == right)
                    break;
                mid = (left + right) / 2;
                int cmp = /*Comparator(items[mid], prefix);*/ String.Compare(items[mid].Substring(0, Math.Min(prefix.Length, items[mid].Length)), prefix, StringComparison.OrdinalIgnoreCase);
                if (cmp < 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            var name = new List <string> ();
            for (int i = left; i < items.Length && i <= left + count + 1; i++)
            {
                if (items[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    name.Add(items[i]);
            }
            var names = name.ToArray();
            return names;/*
            StringComp SC = new StringComp();
            int firstName = Array.BinarySearch(items, prefix, SC);
            for (int i = firstName - 1; i >= 0; i--)
            {
                if (items[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    firstName = i;
                else
                    break;
            }
            var name = new List<string>();
            for (int i = firstName; i < firstName + count && i < items.Length && i >= 0; ++i)
            {
                if (items[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    name.Add(items[i]);
                else
                    break;
            }
            var names = name.ToArray();
			return names;*/
		}

        // Найти количество слов словаря, начинающихся с prefix
        // Эта функция должна работать за O(log(n))
        public int FindCount(string prefix)
		{
            if (prefix == "")
                return 0;
            int left = 0;
            int right = items.Length - 1;
            int mid;
            while (left <= right)
            {
                if (left == right)
                    break;
                mid = (left + right) / 2;
                int cmp = /*Comparator(items[mid], prefix);*/ String.Compare(items[mid].Substring(0, Math.Min(prefix.Length, items[mid].Length)), prefix, StringComparison.OrdinalIgnoreCase);
                if (cmp < 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            int first = left;
            left = 0;
            right = items.Length - 1;
            while (left <= right)
            {
                if (left == right)
                    break;
                mid = (left + right) / 2;
                int cmp = /*Comparator(items[mid], prefix);*/ String.Compare(items[mid].Substring(0, Math.Min(prefix.Length, items[mid].Length)), prefix, StringComparison.OrdinalIgnoreCase);
                if (cmp <= 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            int last = left;
            int answer = 0;
            if (items[first + 1].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                answer += last - first;
            if (answer == 0)
                if (items[first].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                answer++;
            //if (first != last && items[last].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
              //  answer++;
            return answer;
		}
	}
}
