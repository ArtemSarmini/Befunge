using System;
using System.IO;
using Befunge.Windows;

namespace Befunge
{
	class Program
	{
		internal static IWindow _currentWindow;
		internal static IWindow _codeWindow;

		internal static Speed _speed;

		public static Speed Speed => _speed;

		static void Main(string[] args)
		{
			try
			{
				Console.Title = "Befunge interpreter";
				Console.WindowWidth = Interpreter.Field.Width + 1;
				Console.WindowHeight = Interpreter.Field.Heigth + 16;
				Console.CursorSize = 40;
				CheckCommandLine();
				Directory.CreateDirectory("logs");

				_currentWindow = new MainWindow();
				_currentWindow.Run();
			}
			catch (NotImplementedException e)
			{
				Console.Clear();
				Console.CursorVisible = true;
				Console.WriteLine(
					  "You tried to use functionality which is not supported yet: "
					+ e.Message
					+ Environment.NewLine
					+ "We are sorry!");
			}
			catch (Exception e)
			{
				Console.Clear();
				Console.CursorVisible = true;
				Console.WriteLine(
					  $"Something went wrong. Exception {e.GetType().Name} was cought."
					+ Environment.NewLine
					+ "See logs/err_log.txt for details.");
				File.WriteAllText(
					"logs/err_log.txt",
					e.ToString() + Environment.NewLine);
			}
			Console.ReadLine();
			Console.Clear();
		}

		static void CheckCommandLine()
		{
			string[] args = Environment.GetCommandLineArgs();
			if (args.Length == 3 && args[1] == "--speed")
			{
				switch (args[2])
				{
					case "fast":
						_speed = Speed.Fast;
						break;
					case "slow":
						_speed = Speed.Slow;
						break;
					case "onkeypress":
						_speed = Speed.OnKeypress;
						break;
					default:
						_speed = Speed.Fast;
						break;
				}
			}
			else
				_speed = Speed.Fast;
		}
	}

	public enum Speed
	{
		Fast,
		Slow,
		OnKeypress
	}
}
