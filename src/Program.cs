using System;
using System.IO;

using Befunge.Windows;

namespace Befunge
{
	class Program
	{
		internal static IWindow _currentWindow;
		internal static IWindow _codeWindow;

		static void Main(string[] args)
		{
			try
			{
				Directory.CreateDirectory("logs");

				_currentWindow = new MainWindow();
				_currentWindow.Run();
			}
			catch (Exception e)
			{
				Console.Clear();
				Console.WriteLine(
					  $"Something went wrong. Exception {e.GetType().Name} was cought."
					+ Environment.NewLine
					+ "See logs/err_log.txt for details.");
				File.WriteAllText(
					"logs/err_log.txt",
					e.ToString() + Environment.NewLine);
				Console.ReadLine();
			}
			Console.Clear();
		}
	}
}
