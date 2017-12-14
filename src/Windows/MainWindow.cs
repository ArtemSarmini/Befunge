using System;

namespace Befunge.Windows
{
	public class MainWindow : IWindow
	{
		public void Run()
		{
			Console.Clear();
			Program._currentWindow = new CodeWindow();
			Program._currentWindow.Run();
		}

		public WindowType Type => WindowType.Main;
	}
}
