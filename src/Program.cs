using System;

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
				_currentWindow = new MainWindow();
				_currentWindow.Run();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			Console.Clear();
		}
	}
}
