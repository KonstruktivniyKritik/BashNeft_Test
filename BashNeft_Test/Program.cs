using System.Text.RegularExpressions;

internal class Program
{
	public static bool[,] Board;
    public static string BoardDimensity = "";
	public static int BoardXDim;
	public static int BoardYDim;
    public static void Main(string[] args)
	{
		//Вводим размерность таблицы
		BoardDimensity = "";
		Regex pass = new Regex("^\\d+ \\d+$");
		do 
		{
			Console.Clear();
			Console.WriteLine("Введите размерность таблицы (X Y)");
			BoardDimensity = Console.ReadLine();
		}
		while (!pass.IsMatch(BoardDimensity));
		BoardXDim = Int32.Parse(BoardDimensity.Substring(0, BoardDimensity.IndexOf(' ')));
		BoardYDim = Int32.Parse(BoardDimensity.Substring(BoardDimensity.IndexOf(' ')));
		
		//Генерируем таблицу
		Board = new bool[BoardXDim, BoardYDim];
		var rand = new Random();
		for (int i = 0; i < BoardXDim; i++)
		{
			for (int j = 0; j < BoardYDim; j++)
			{
				Board[i, j] = rand.Next(10) < 7 ? true: false;
			}
		}

		//Ищем группы
        List<List<Tuple<int,int>>> AllGroups = new List<List<Tuple<int, int>>>();
		for (int i = 0; i < BoardXDim; i++)
		{
			for (int j = 0; j < BoardYDim; j++)
			{
				if (Board[i, j] == true)
					DigGroup(i, j, AllGroups);
			}
		}

		//Печатаем результат
		Print2DArray<bool>(Board, AllGroups);
	}

	public static void DigGroup(int i, int j, List<List<Tuple<int, int>>> allgroups)
	{
		Tuple<int, int> Start = new Tuple<int, int>(i, j);
		//Смотрим, принадлежит ли точка Start к какой либо  уже обнаруженной группе группе
		foreach (var checkgroup in allgroups)
			if (checkgroup.Contains(Start))
				return;

		//Если нет, то значит точка образует новую группу
        List<Tuple<int, int>> NewGroup = new List<Tuple<int, int>>();
		//алгоритм "раскопки" групп. 
		DigginIn(NewGroup, i, j);
		allgroups.Add(NewGroup);
	}

    public static void DigginIn(List<Tuple<int, int>> actualgroup, int i, int j)
	{
		//Рекурсивный алгоритм для объединения групп
		//От начальной точки мы смотрим на соседние точки:вверх,вниз,влево,вправо.
		//Если соседняя точка равна true, то мы рекурсивно перепрыгиваем на нее.
		//Если точка не была добавлена в группу, мы ее добавляем.
		//Если точка была добавлена в группу, например,
		//от точки (0,1) мы перепрыгнули сначала на (1,1),
		//а потом по рекурсивности вернулись обратно на (0,1)
		//то рекурсия прерывается.

		//прерывание рекурсии если уже были в этой точке
		if (actualgroup.Contains(new Tuple<int, int>(i, j)))
			return;

		actualgroup.Add(new Tuple<int,int> (i, j));
		if (i-1 >= 0)
			if (Board[i - 1, j] == true)
			{
				DigginIn(actualgroup, i - 1, j);
			}
		if (j - 1 >= 0)
			if (Board[i, j - 1] == true)
			{
				DigginIn(actualgroup, i, j - 1);
			}
		if (i + 1 < BoardXDim)
			if (Board[i + 1, j] == true)
			{
				DigginIn(actualgroup, i + 1, j);
			}
		if (j + 1 < BoardYDim)
			if (Board[i, j + 1] == true)
			{
				DigginIn(actualgroup, i, j + 1);
			}
	}

    public static void Print2DArray<T>(T[,] matrix, List<List<Tuple<int, int>>> allgroups)
	{
		//Печать двумерного массива, разбавленная цветами.
		//По желанию можно сделать вывод как указано в тестовом задании

		Console.WriteLine($"Количество групп: {allgroups.Count}");
		ConsoleColor[] GroupColors = new ConsoleColor[allgroups.Count];
		for (int i = 0; i < GroupColors.Length; i++)
		{
			int ConsoleColorc = i + 1 < 15 ? i + 1 : i - (15 * (i / 15)) + 1;

			GroupColors[i] = (ConsoleColor)(i + 1 < 15 ? i + 1 : i - (15 * (i / 15)));
        }
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
                Tuple<int, int> point = new Tuple<int, int>(i, j);
				bool t = false;
				for (int k = 0; k < allgroups.Count; k++)
				{
					var group = allgroups[k];
					if (group.Contains(point))
					{
                        //Если делать вывод как в ТЗ
                        //Console.Write(k);

                        //Если сделать красивый вывод
                        Console.BackgroundColor = GroupColors[k];
                        Console.Write(matrix[i, j]+" ");

                        t = true;
						break;
                    }
				}
				if (!t)
				{
                    //Если делать вывод как в ТЗ
                    //Console.Write("*");

                    //Если сделать красивый вывод
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(matrix[i, j]);
                }

            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();
		}
	}
}