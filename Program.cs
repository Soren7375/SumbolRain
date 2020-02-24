using System;
using System.Linq;
using System.Threading;
namespace SumbolRain
{
	class Snake
	{
		int length, index, speed, currentLength = 1, absoluteLength;
		public static Random random = new Random();
		private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&?";
		static object locker = new object();

		public Snake(int length, int index, int speed)
		{
			this.length = length;
			absoluteLength = length;
			this.index = index;
			this.speed = speed;
		}

		public void Print(int j)
		{
			var randomSumbols = Enumerable.Repeat(chars, currentLength).Select(s => s[random.Next(s.Length)]).ToList();
			if (j >= Console.WindowHeight - length)
			{
				randomSumbols.Remove(randomSumbols[--length]);
				--currentLength;
			}
			if (randomSumbols.Count() == 0)
			{
				Thread thread = new Thread(new ParameterizedThreadStart(Program.Start));
				thread.Start(new Snake(random.Next(4, 8), index, speed));
				return;
			}
			lock (locker)
			{
				randomSumbols[0] = ' ';
				for (int i = 0; i < randomSumbols.Count(); i++)
				{
					Console.ForegroundColor = (i == randomSumbols.Count() - 1) && ((absoluteLength == randomSumbols.Count())
						|| j <= absoluteLength) ? ConsoleColor.White : ConsoleColor.DarkGreen;
					Console.SetCursorPosition(index, j + i);
					Console.Write(randomSumbols[i]);
				}
			}
			Thread.Sleep(speed);
			if (currentLength == length)
			{
				++j;
			}
			else
			{
				++currentLength;
			}
			Print(j);
		}
	}

	class Program
	{
		public static void Start(object snake)
		{
			Snake s = (Snake)snake;
			s.Print(0);
		}

		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			for (int i = 0; i < 100; i+=2)
			{
				Thread thread = new Thread(new ParameterizedThreadStart(Start));
				thread.Start(new Snake(Snake.random.Next(4,8), i, Snake.random.Next(200,1000)));
			}
		}
	}
}