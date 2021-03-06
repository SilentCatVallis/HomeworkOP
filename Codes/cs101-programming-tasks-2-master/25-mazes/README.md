* [К оглавлению задачника](https://github.com/urfu-code/cs101-main)
* [Кодекс разработчика](https://docs.google.com/document/d/1w8C1VyDPh9_1DaGD6oDJWmHw8V6cWrr469CgMiLGmdE/edit#)

# Задачи на поиск в ширину 2

## Грязь

В [контесте](http://acm.timus.ru/auth.aspx?source=monitor.aspx%3fid=204) появилась (или вот-вот появится) ещё одна задача с тимуса — [1325 Грязь](http://acm.timus.ru/problem.aspx?num=1325).

Первые 28 тестов не содержат грязи, и фактически, требуют использовать стандартный поиск в ширину. За прохождение этих тестов вы зарабатываете 2 балла.

За полное решение задачи вы можете заработать 3 балла.

Ещё один дополнительный балл можно заработать, если сделать визуализатор по аналогии с задачами из прошлого блока.
Проще всего это будет сделать так — взять проект визуализатора из прошлого блока задач и переделать его под новую задачу.


# Подсказки

**Внимание!** 

Вы не обязаны решать задачи в точности следуя указаниям ниже.
Более того, будет лучше, если у вас получится решить задачу без использования этих подсказок.

Рекомендуется пользоваться этим подсказками, только если совсем не можете самостоятельно придумать как решать задачу.

Впрочем, если вы уже решили задачу, будет небесполезно подсказки все же прочитать.
Возможно, они откроют вам какой-то новый взгляд на задачу.

↓

↓

↓

↓

↓

↓

↓

↓

↓

↓


###Серия подсказок для упрощенной версии задачи на 2 балла "без грязи":

1.
Попробуйте написать класс Map, описывающий план квартиры. Этот класс должен считывать своё состояние из консоли, получать содержимое клетки по её координатам, и вычислять всех свободных соседей заданной клетки.

Для представления точки в лабиринте удобно использовать кортеж из двух чисел-координат ```Tuple<int, int>```

2.
Интерфейс этого касса может быть таким:

```csharp
class Map{
  public Tuple<int, int> Computer;
  public Tuple<int, int> Fridge; //холодильник
  public void LoadFromConsole()
  public int GetCell(Tuple<int, int> location)
  public IEnumerable<Tuple<int, int>> GetFreeNeighbours(Tuple<int, int> p)
}
```

3.
Метод GetCell может возвращать 0 (WALL), для всех координат, выходящих за границы квартиры.
Это поможет не проверять выход за границы в других местах программы.

4.
Если не смотря на все подсказки выше у вас возникли затруднения с написанием класса Map, вы можете воспользоваться этим кодом:

```csharp
	class Map
	{
		private const int WALL = 0;
		private int[][] Cells;
		public Tuple<int, int> Computer;
		public Tuple<int, int> Fridge;

		public void LoadFromConsole()
		{
			var size = ReadPointFromConsole();
			Computer = ReadPointFromConsole();
			Fridge = ReadPointFromConsole();
			Cells = Enumerable.Range(0, size.Item2 + 1)
				.Select(y => (Console.ReadLine() ?? "").Select(ch => ch - '0').ToArray())
				.ToArray();
		}

		public int GetCell(Tuple<int, int> p)
		{
			return Inside(p) ? Cells[p.Item2][p.Item1] : WALL;
		}

		public IEnumerable<Tuple<int, int>> GetFreeNeighbours(Tuple<int, int> p)
		{
			for (int dx = -1; dx <= 1; dx++)
				for (int dy = -1; dy <= 1; dy++)
				{
					if (dx == 0 && dy == 0) continue;
					var n = Tuple.Create(p.Item1 + dx, p.Item2 + dy);
					if (GetCell(n) != WALL) yield return n;
				}
		}

		private bool Inside(Tuple<int, int> p)
		{
			return p.Item2 >= 0 && p.Item2 < Cells.Length && p.Item1 >= 0 && p.Item1 < Cells[0].Length;
		}


		private static Tuple<int, int> ReadPointFromConsole()
		{
			var parts = (Console.ReadLine() ?? "").Split(' ');
			return Tuple.Create(int.Parse(parts[1])-1, int.Parse(parts[0])-1);
		}
	}

```

Использовать его можно так:
```csharp
var map = new Map();
map.LoadFromConsole();
var paths = Bfs(map, map.Computer, map.Fridge)
...

```


### Подсказки для полной версии

1.
Идея решения в том, чтобы пусти серию поисков в ширину. Сначала найти все клетки, до которых можно дойти без переобувания. Все граничащие с ними ещё не посещенные клетки — это стартовые клетки для следующих запусков поиска в ширину. Так можно действовать до тех пор, пока не достигните холодильника, либо пока идти будет уже некуда.

2.
Если вы получаете TimeLimit, подумайте, какие могут быть особо сложные случаи для вашего алгоритма. Например, расмотрите два таких случая: "Змейка", с чередующимися грязью и чистотй, а также грязь и чистота в шахматном порядке.

Одна из причин Wrong answer может быть в том, что среди всех путей с K переобуваниями вы находите не кратчайший. Попробуйте сконструировать тест, который поможет это показать.

3.
В контесте сильно ослаблены ограничения по времени по сравнению с задачей на тимусе.
Если вы хотите сдать задачу на тимус, а не только в контест, вам придется придумать другое, более эффективное решение.
