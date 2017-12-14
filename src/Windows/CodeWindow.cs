using System;
using System.Collections.Generic;
using System.Linq;

using Befunge.Interpreter;

namespace Befunge.Windows
{
	public class CodeWindow : IWindow
	{
		private InstructionPointer _ip;
		private Stack<long> _stack;
		private List<List<long>> _field;

		public void Run()
		{
			if (_ip == null) _ip = new InstructionPointer();
			if (_stack == null) _stack = new Stack<long>();
			if (_field == null) _field = Helpers.CreateEmptyField();

			while (true)
			{
				if (HandleKey())
					break;
			}
			Console.Clear();
			Console.SetCursorPosition(0, 0);
			PrintField();
			Console.WriteLine("^ field");
			Console.ReadLine();
		}

		// returns true if Esc was pressed 
		private bool HandleKey()
		{
			var k = Console.ReadKey(true);
			switch (k.Key)
			{
				case ConsoleKey.Escape:
					return true;
				case ConsoleKey.RightArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Right;
					else
					{
						_ip.X++;
						Console.SetCursorPosition(_ip.X, _ip.Y);
					}
					break;
				case ConsoleKey.LeftArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Left;
					else
					{
						_ip.X--;
						Console.SetCursorPosition(_ip.X, _ip.Y);
					}
					break;
				case ConsoleKey.UpArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Up;
					else
					{
						_ip.Y--;
						Console.SetCursorPosition(_ip.X, _ip.Y);
					}
					break;
				case ConsoleKey.DownArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Down;
					else
					{
						_ip.Y++;
						Console.SetCursorPosition(_ip.X, _ip.Y);
					}
					break;
				// actual input
				default:
				{
					char c = k.KeyChar;
					_field[_ip.X][_ip.Y] = Convert.ToInt32(c);
					Console.Write(Helpers.ToPrintable(c));
					switch (_ip.Direction)
					{
						case InstructionPointerDirection.Right:
							_ip.X++;
							break;
						case InstructionPointerDirection.Left:
							_ip.X--;
							break;
						case InstructionPointerDirection.Up:
							_ip.Y--;
							break;
						case InstructionPointerDirection.Down:
							_ip.Y++;
							break;
					}
					Console.SetCursorPosition(_ip.X, _ip.Y);
					break;
				}
			}
			return false;
		}

		private void PrintField()
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
					Console.Write(Helpers.ToPrintable(_field[i][j]));
				Console.WriteLine();
			}
		}

		public WindowType Type => WindowType.Code;
		static class Helpers
		{

			internal static List<List<long>> CreateEmptyField()
			{
				var row = new List<long>(
					new long[] { 32, 32, 32, 32, 32, 32, 32, 32,
									  32, 32, 32, 32, 32, 32, 32, 32 });
				var field = new List<List<long>>();
				for (int i = 0; i < 16; i++)
					field.Add(row);
				return field;
			}
			internal static bool IsInstruction(char c)
			{
				return c > 31 && c < 127;
			}

			internal static bool IsInstruction(long l)
			{
				return l > 31 && l < 127;
			}

			internal static char ToPrintable(char c)
			{
			return IsInstruction(c) ? c : (char)0xFFFD;
			}

			internal static char ToPrintable(long l)
			{
				return (char)(IsInstruction(l) ? l : 0xFFFDL);
			}
		}
	}
}
