using System;
using System.Collections.Generic;
using System.Linq;

namespace Befunge.Interpreter
{
	public class Field
	{
		private List<List<long>> _field;

		private static readonly int _width = 64;
		private static readonly int _heigth = 32;

		public Field()
		{
			_field = new List<List<long>>(_heigth);
			for (int i = 0; i < _heigth; i++)
			{
				var line = new List<long>(_width);
				for (int j = 0; j < _width; j++)
					line.Add(32);
				_field.Add(line);
			}
		}

		public long this [int x, int y]
		{
			get => _field[y][x];
			set => _field[y][x] = value;
		}

		public long this [InstructionPointer ip]
		{
			get => _field[ip.Y][ip.X];
			set => _field[ip.Y][ip.X] = value;
		}

        public void Print()
        {
			Console.WriteLine(string.Join(
				Environment.NewLine,
				_field.Select(l => string.Join(
					"",
					l.Select(c => Helpers.ToPrintable(c))
				))
			));
        }

		public void Print(InstructionPointer cellToHighlight)
        {
            for (int i = 0; i < _heigth; i++)
			{
				if (cellToHighlight.Y != i)
				{
					Console.WriteLine(string.Join(
						"", _field[i].Select(c => Helpers.ToPrintable(c))));
				}
				else
				{
					for (int j = 0; j < _width; j++)
					{
						if (cellToHighlight.X == j)
						{
							ConsoleColor bgc = Console.BackgroundColor,
							             fgc = Console.ForegroundColor;
							Console.BackgroundColor = ConsoleColor.White;
							Console.ForegroundColor = ConsoleColor.Black;
							Console.Write(Helpers.ToPrintable(_field[i][j]));
							Console.BackgroundColor = bgc;
							Console.ForegroundColor = fgc;
						}
						else
							Console.Write(Helpers.ToPrintable(_field[i][j]));
					}
					Console.WriteLine();
				}
			}
        }
	}
}
