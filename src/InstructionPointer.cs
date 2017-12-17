namespace Befunge.Interpreter
{
	public class InstructionPointer
	{
		//     y     z
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

		public InstructionPointer()
		{
			_x = 0;
			_y = 0;
			_z = 0;
			_direction = InstructionPointerDirection.Right;
		}

		public InstructionPointerDirection Direction
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

		public void Reset()
		{
			_x = 0;
			_y = 0;
			_z = 0;
			_direction = InstructionPointerDirection.Right;
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
