using System;

namespace Befunge.Windows
{
	public interface IWindow
	{
		WindowType Type { get; }
		void Run();
	}

	public enum WindowType
	{
		Main,
		Code
	}
}
