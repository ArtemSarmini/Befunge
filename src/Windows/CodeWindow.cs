using System;
using System.Collections.Generic;

using Befunge.Interpreter;

namespace Befunge.Windows
{
	public class CodeWindow : IWindow
	{
		private InstructionPointer _ip;
		private Stack<long> _stack;
		private Field _field;

		public void Run()
		{
			if (_ip    == null) _ip    = new InstructionPointer();
			if (_stack == null) _stack = new Stack<long>();
			if (_field == null) _field = new Field();

			while (true)
			{
				if (HandleKey())
					break;
			}
			Console.Clear();
			Console.SetCursorPosition(0, 0);
			_field.Print();
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

				// win have problems with arrow+alt,
				// so here are alternative key bindings
				case ConsoleKey.L when k.Modifiers == ConsoleModifiers.Alt:
					_ip.Direction = InstructionPointerDirection.Right;
					break;
				case ConsoleKey.J when k.Modifiers == ConsoleModifiers.Alt:
					_ip.Direction = InstructionPointerDirection.Left;
					break;
				case ConsoleKey.I when k.Modifiers == ConsoleModifiers.Alt:
					_ip.Direction = InstructionPointerDirection.Up;
					break;
				case ConsoleKey.K when k.Modifiers == ConsoleModifiers.Alt:
					_ip.Direction = InstructionPointerDirection.Down;
					break;

				// actual input
				default:
				{
					char c = k.KeyChar;
					_field[_ip.X, _ip.Y].ChangeTo(Convert.ToInt64(c));
					Console.Write(Helpers.ToPrintable(c));
					_ip.Move();
					Console.SetCursorPosition(_ip.X, _ip.Y);
					break;
				}
			}
			return false;
		}

		public WindowType Type => WindowType.Code;
	}
}
