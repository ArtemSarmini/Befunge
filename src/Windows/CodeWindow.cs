using System;
using System.Collections.Generic;

using Befunge.Interpreter;

namespace Befunge.Windows
{
	public class CodeWindow : IWindow
	{
		private InstructionPointer _ip;
		private Field _field;

		public void Run()
		{
			if (_ip    == null) _ip    = new InstructionPointer();
			if (_field == null) _field = new Field();

			while (true)
			{
				if (HandleKey())
					break;
			}
			Console.Clear();
			Console.SetCursorPosition(0, 0);
			_field.Print();
			Console.ReadLine();

			var runner = new CodeRunner(_field);
			runner.Run();
			Console.WriteLine(Environment.NewLine + "Done!");
		}

		// returns true if Esc was pressed
		private bool HandleKey()
		{
			var k = Console.ReadKey(true);
			switch (k.Key)
			{
				case ConsoleKey.Escape:
					return true;

				case ConsoleKey.Backspace:
					_ip.MoveBack();
					Console.SetCursorPosition(_ip.X, _ip.Y);
					_field[_ip] = ' ';
					Console.Write(' ');
					Console.SetCursorPosition(_ip.X, _ip.Y);
					break;

				case ConsoleKey.RightArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Right;
					else if (_ip.X < Field.Width)
					{
						_ip.X++;
						Console.SetCursorPosition(_ip.X, _ip.Y);
					}
					break;
				case ConsoleKey.LeftArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Left;
					else if (_ip.X > 0)
					{
						_ip.X--;
						Console.SetCursorPosition(_ip.X, _ip.Y);
					}
					break;
				case ConsoleKey.UpArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Up;
					else if (_ip.Y > 0)
					{
						_ip.Y--;
						Console.SetCursorPosition(_ip.X, _ip.Y);
					}
					break;
				case ConsoleKey.DownArrow:
					if (k.Modifiers == ConsoleModifiers.Alt)
						_ip.Direction = InstructionPointerDirection.Down;
					else if (_ip.Y < Field.Heigth)
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
					_field[_ip] = Convert.ToInt64(c);
					Console.Write(Helpers.ToPrintable(c));
					_ip.MoveSafe();
					Console.SetCursorPosition(_ip.X, _ip.Y);
					break;
				}
			}
			return false;
		}

		public WindowType Type => WindowType.Code;
	}
}
