using System;
using System.Collections.Generic;
using System.Linq;

using Befunge.Interpreter;

namespace Befunge.Windows
{
	public class Codeindow : IWindow
	{
		private InstructionPointer _ip;
		private Stack<long> _stack;
		private List<List<long>> _field;

		private CaretPosition _caretPosition;

		public void Run()
		{
			if (_ip            == null) _ip            = new InstructionPointer();
			if (_stack         == null) _stack         = new Stack<long>();
			if (_field         == null) _field         = CreateEmptyField();
			if (_caretPosition == null) _caretPosition = new CaretPosition();
			
			while (true)
			{
				var k = Console.ReadKey();
				switch (k.Key)
				{
					case ConsoleKey.RightArrow:
						_caretPosition.X++;
						break;
					case ConsoleKey.LeftArrow:
						_caretPosition.X--;
						break;
					case ConsoleKey.UpArrow:
						_caretPosition.Y++;
						break;
					case ConsoleKey.DownArrow:
						_caretPosition.Y--;
						break;
					default:
					{
						char c = k.KeyChar;
						break;
					}
				}
			}
		}

		private List<List<long>> CreateEmptyField()
		{
			var row = new List<long>(
				new long [] { 32, 32, 32, 32, 32, 32, 32, 32,
				              32, 32, 32, 32, 32, 32, 32, 32 });
			var field = new List<List<long>>();
			for (int i = 0; i < 16; i++)
				field.Add(row);
			return field;
		}

		private void PrintField()
		{
			for (int i = 0; i < 16; i++)
				for (int j = 0; j < 16; j++)
					Console.WriteLine((char)
						((_field[i][j] > 31 && _field[i][j] < 35)
							? _field[i][j]
							: 0xFFFD));
		}

		private bool IsInstruction(char c)
		{
			return c > 31 && c < 127;
		}

		public WindowType Type => WindowType.Code;

		class CaretPosition
		{
			private int _x, _y;
			public int X
			{
				get => _x;
				set => _x = value;
			}

			public int Y
			{
				get => _y;
				set => _y = value;
			}
		}
	}
}
