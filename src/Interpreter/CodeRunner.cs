using System;
using System.Collections.Generic;

using Befunge.Lang;

namespace Befunge.Interpreter
{
	public enum InterpreterMode
	{
		Normal,
		String
	}

	public class CodeRunner
	{
		private Field _field;
		private InstructionPointer _ip;
		private InterpreterMode _mode;
		private Stack<long> _stack;
		private Random _rnd;

		public CodeRunner()
		{
			_field = new Field();
			_ip    = new InstructionPointer();
			_mode  = InterpreterMode.Normal;
			_stack = new Stack<long>();

			_rnd = new Random();
		}

		private void GoRight()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = InstructionPointerDirection.Right;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GoRight]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void GoLeft()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = InstructionPointerDirection.Left;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GoLeft]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void GoUp()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = InstructionPointerDirection.Up;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GoUp]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void GoDown()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = InstructionPointerDirection.Up;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GoDown]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void GoForward()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = InstructionPointerDirection.Forward;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GoForward]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void GoBackward()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = InstructionPointerDirection.Forward;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GoBackward]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}
		private void GoRand()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					// TODO replace 4 with 6 for 3D
					_ip.Direction = (InstructionPointerDirection)_rnd.Next(4);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GoRand]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Trampoline()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Move(2);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Trampoline]);
					_ip.Move();
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
		}

		private void TurnLeft()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					TurnLeftImpl();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.TurnLeft]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void TurnLeftImpl()
		{
			switch (_ip.Direction)
			{
				case InstructionPointerDirection.Right:
					_ip.Direction = InstructionPointerDirection.Up;
					break;
				case InstructionPointerDirection.Left:
					_ip.Direction = InstructionPointerDirection.Down;
					break;
				case InstructionPointerDirection.Up:
					_ip.Direction = InstructionPointerDirection.Left;
					break;
				case InstructionPointerDirection.Down:
					_ip.Direction = InstructionPointerDirection.Right;
					break;
				default:
					// how z-rotation should be performed on z itself?
					throw new NotImplementedException();
			}
		}

		private void TurnRight()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					TurnRightImpl();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.TurnRight]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void TurnRightImpl()
		{
			switch (_ip.Direction)
			{
				case InstructionPointerDirection.Right:
					_ip.Direction = InstructionPointerDirection.Down;
					break;
				case InstructionPointerDirection.Left:
					_ip.Direction = InstructionPointerDirection.Up;
					break;
				case InstructionPointerDirection.Up:
					_ip.Direction = InstructionPointerDirection.Right;
					break;
				case InstructionPointerDirection.Down:
					_ip.Direction = InstructionPointerDirection.Left;
					break;
				default:
					// how z-rotation should be performed on z itself?
					throw new NotImplementedException();
			}
		}

		private void Reflect()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ReflectImpl();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Reflect]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void ReflectImpl()
		{
			switch (_ip.Direction)
			{
				case InstructionPointerDirection.Right:
					_ip.Direction = InstructionPointerDirection.Left;
					break;
				case InstructionPointerDirection.Left:
					_ip.Direction = InstructionPointerDirection.Right;
					break;
				case InstructionPointerDirection.Up:
					_ip.Direction = InstructionPointerDirection.Down;
					break;
				case InstructionPointerDirection.Down:
					_ip.Direction = InstructionPointerDirection.Up;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private void HorIf() =>
			_ip.Direction = _stack.Pop() == 0
				? InstructionPointerDirection.Right
				: InstructionPointerDirection.Left;
		private void VertIf() =>
			_ip.Direction = _stack.Pop() == 0
				? InstructionPointerDirection.Down
				: InstructionPointerDirection.Up;

		private void Cmp()
		{
			long b = _stack.Pop(),
			     a = _stack.Pop();
			if (a < b)
				TurnLeft();
			else if (a > b)
				TurnRight();
		}

		private void Add() => _stack.Push(_stack.Pop() + _stack.Pop());
		private void Mul() => _stack.Push(_stack.Pop() * _stack.Pop());
		private void Sub() => _stack.Push(-_stack.Pop() + _stack.Pop());

		private void Div()
		{
			long b = _stack.Pop(),
			     a = _stack.Pop();
			_stack.Push(a / b);
		}

		private void Rem()
		{
			long b = _stack.Pop(),
			     a = _stack.Pop();
			_stack.Push(a % b);
		}

		private void GreaterThan() =>
			_stack.Push(_stack.Pop() < _stack.Pop() ? 1 : 0);

		private void Not() => _stack.Push(_stack.Pop() == 0 ? 1 : 0);


		private void D0() => _stack.Push(0);
		private void D1() => _stack.Push(1);
		private void D2() => _stack.Push(2);
		private void D3() => _stack.Push(3);
		private void D4() => _stack.Push(4);
		private void D5() => _stack.Push(5);
		private void D6() => _stack.Push(6);
		private void D7() => _stack.Push(7);
		private void D8() => _stack.Push(8);
		private void D9() => _stack.Push(9);

		private void D10() => _stack.Push(10);
		private void D11() => _stack.Push(11);
		private void D12() => _stack.Push(12);
		private void D13() => _stack.Push(13);
		private void D14() => _stack.Push(14);
		private void D15() => _stack.Push(15);
	}
}
