using System;

namespace names
{
	internal class NamesTasks
	{
        /*int[] amount = {29, 26, 29, 28, 29, 28, 29, 29, 28, 29, 28, 29};
        public struct Statistics
        {
            public int firstDay;
            public int lastDay;
            public int otherDay;
        };

        Statistics[] numerOfMonth = new Statistics[12];

        public void CalculateStatistics(NameData name)
        {
            if (name.BirthDate.Day == 1)
            {
                numerOfMonth[name.BirthDate.Month - 1].firstDay++;
            }
            else if (name.BirthDate.Day == System.DateTime.DaysInMonth(name.BirthDate.Year, name.BirthDate.Month))
            {
                numerOfMonth[name.BirthDate.Month - 1].lastDay++;
            }
            else
            {
                numerOfMonth[name.BirthDate.Month - 1].otherDay++;
            }
        }
        */
		public void ShowBirthDayMonthStatistics(NameData[] names)
		{
            int firstDay = 0, lastDay = 0, otherDays = 0;
			Console.WriteLine("Статистика рождаемости по датам");
            for (int i = 0; i < names.Length; ++i)
            {
                if (names[i].BirthDate.Day == 1)
                {
                    firstDay++;
                }
                else if (names[i].BirthDate.Day == System.DateTime.DaysInMonth(names[i].BirthDate.Year, names[i].BirthDate.Month))
                {
                    lastDay++;
                }
                else
                {
                    otherDays++;
                }
            }
            /*for (int i = 0; i < names.Length; ++i)
            {
                CalculateStatistics(names[i]);
            }
            for (int i = 0; i < 12; ++i)
            {
                Console.WriteLine("in {0} mounth {1} was born in first day, {2} in last day and {3} in other Day",
                i + 1, numerOfMonth[i].firstDay, numerOfMonth[i].lastDay, numerOfMonth[i].otherDay / (System.DateTime.DaysInMonth(1999, i + 1) - 2));
            }*/
            Console.WriteLine(firstDay + " " + (otherDays / 341.25) + " " + lastDay);
            //COMPLYTE
		}

        int[] year = new int[115];
        int[] amountOfPeople = new int[115];
       
        public void ShowBirthYearsStatisticsHistogram(NameData[] names)
		{
            for (int i = 0; i < 115; ++i)
            {
                year[i] = i + 1890;
            }
			Console.WriteLine("Статистика рождаемости по годам");
            for (int i = 0; i < names.Length; ++i)
            {
                amountOfPeople[names[i].BirthDate.Year - 1890]++;
            }
			//COMPLYTE
			Histogram.Show("Рождаемость по годам", year, amountOfPeople);
		}
		public void ShowBirthDayMonthStatisticsForName(NameData[] names, string name)
		{
            int[,] kalendar = new int [12,31];
            int amountOfPeople = 0;
			Console.WriteLine("Статистика рождаемости имени {0}", name);
            for (int i = 0; i < names.Length; ++i)
            {
                if (names[i].Name == name)
                {
                    amountOfPeople++;
                    kalendar[names[i].BirthDate.Month - 1, names[i].BirthDate.Day - 1] += 1;
                }
            }
            int answer = 0;
            int answerDay = 0, answerMounth = 0;
            for (int month = 0; month < 12; ++month)
            {
                for (int day = 0; day < 31; ++day)
                {
                    if (kalendar[month, day] > answer)
                    {
                        answerDay = day;
                        answerMounth = month;
                        answer = kalendar[month, day];
                    }
                }
            }
            var persent =  (answer * 100.0 / amountOfPeople);
            Console.Write((answerMounth + 1) + " " + (answerDay + 1) + " ");
            Console.WriteLine(persent.ToString() + "%");
        }
    }
			/*
			Выведите на консоль:
				1. Дату без года (только день и месяц) с максимальной рождаемостью людей с именем name.
				2. Процент людей с заданным именем, рожденных в найденную дату из предыдущего пункта.
				Проинтерпретируйте результат этой функции на именах Виктория, Юрий, Илья, Владимир.
				Сильно ли выше среднего рождаемость в самые "плодотворные" дни?
             */ 
}
