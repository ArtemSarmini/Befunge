using System;
using System.Collections.Generic;

namespace Befunge.Interpreter
{
	public static class CodeRunner
	{
		private static Stack<long> _stack;
	}

	public class InstructionPointer
	{
		//     z     y
		//     |   /
		//     |  /
		//     | /
		// ____|/___ x
		//     /
		//    /|
		//   / |
		//  /  |
		// /   |

		private InstructionPointerDirection _direction;

		private int _x, // left-right
		            _y, // forward-backward
		            _z; // up-down

		public InstructionPointerDirection InstructionPointerDirection
		{
			get => _direction;
			set => _direction = value;
		}

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

		public int Z
		{
			get => _z;
			set => _z = value;
		}
	}

	public enum InstructionPointerDirection
	{
		Left,
		Right,
		Up,
		Down,
		Forward,
		Backward
	}
}
