using System;
using System.Collections.Generic;

using Befunge.Interpreter;

namespace Befunge.Windows
{
	public class Codeindow : IWindow
	{
		private InstructionPointer _ip;
		private Stack<long> _stack;
		private List<List<long>> _field;

		public void Run()
		{
			if (_ip    == null) _ip    = new InstructionPointer();
			if (_stack == null) _stack = new Stack<long>();
			if (_field == null) _field = new List<List<long>>();
		}

		public List<List<long>> CreateEmptyField()
		{
			var row = new List<long>(
				new long [] { 32, 32, 32, 32, 32, 32, 32, 32,
				              32, 32, 32, 32, 32, 32, 32, 32 });
			var field = new List<List<long>>();
			for (int i = 0; i < 16; i++)
				field.Add(row);
			return field;
		}

		public WindowType Type => WindowType.Code;
	}
}
