using System;

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

		public void Move()
		{
			switch (_direction)
			{
				case InstructionPointerDirection.Right:
					_x++;
					break;
				case InstructionPointerDirection.Left:
					_x--;
					break;
				case InstructionPointerDirection.Up:
					_y--;
					break;
				case InstructionPointerDirection.Down:
					_y++;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public void MoveSafe()
		{
			switch (_direction)
			{
				case InstructionPointerDirection.Right:
					if (_x < Field.Width)
						_x++;
					break;
				case InstructionPointerDirection.Left:
					if (_x > 0)
						_x--;
					break;
				case InstructionPointerDirection.Up:
					if (_y > 0)
						_y--;
					break;
				case InstructionPointerDirection.Down:
					if (_y < Field.Heigth)
						_y++;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public void MoveBack()
		{
			switch (_direction)
			{
				case InstructionPointerDirection.Right:
					_x--;
					break;
				case InstructionPointerDirection.Left:
					_x++;
					break;
				case InstructionPointerDirection.Up:
					_y++;
					break;
				case InstructionPointerDirection.Down:
					_y--;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public void Move(int steps)
		{
			if (steps < 1)
				throw new ArgumentOutOfRangeException(nameof(steps), "Argument must be positive");

			switch (_direction)
			{
				case InstructionPointerDirection.Right:
					_x += steps;
					break;
				case InstructionPointerDirection.Left:
					_x -= steps;
					break;
				case InstructionPointerDirection.Up:
					_y -= steps;
					break;
				case InstructionPointerDirection.Down:
					_y += steps;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public override string ToString()
		{
			return string.Format(
				$"{{IP: X({_x}), Y({_y}), Z({_z}), Direction({_direction})}}");
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
