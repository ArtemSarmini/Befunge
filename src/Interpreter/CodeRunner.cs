using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

		private bool _run;

		public CodeRunner(Field field)
		{
			_field = field;
			_ip    = new InstructionPointer();
			_mode  = InterpreterMode.Normal;
			_stack = new Stack<long>();

			_rnd = new Random();

			_run = true;
		}

		public void Run()
		{
			while (_run)
			{
				Console.Clear();
				_field.Print(_ip);
				// optimization for instructions that push number on stack
				// in this case we can call proper instruction without reflection
				char instruction = (char)_field[_ip];
				if (   (instruction >= '0' && instruction <= '9')
					|| (instruction >= 'a' && instruction <= 'f'))
				{
					DN(instruction, InstructionConverter.ToInstruction[instruction]);
				}
				else
				{
					MethodInfo command = typeof(CodeRunner).GetMethod(
						InstructionConverter.ToInstruction[instruction].ToString(),
						BindingFlags.NonPublic | BindingFlags.Instance);
					command.Invoke(this, null);
				}
				System.Threading.Thread.Sleep(200);
			}
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
					_ip.Direction = InstructionPointerDirection.Down;
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

		private void JumpOver()
		{
			throw new NotImplementedException();
		}

		private void AbsDelta()
		{
			throw new NotImplementedException();
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

		private void HorIf()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = _stack.Pop() == 0
						? InstructionPointerDirection.Right
						: InstructionPointerDirection.Left;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.HorIf]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void VertIf()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Direction = _stack.Pop() == 0
						? InstructionPointerDirection.Down
						: InstructionPointerDirection.Up;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.VertIf]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void SagIf()
		{
			throw new NotImplementedException();
		}

		private void Cmp()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					long b = _stack.Pop(),
			     	     a = _stack.Pop();
					if (a < b)
						TurnLeft();
					else if (a > b)
						TurnRight();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Cmp]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Jump()
		{
			throw new NotImplementedException();
		}

		private void Iter()
		{
			throw new NotImplementedException();
		}

		private void Add()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(_stack.Pop() + _stack.Pop());
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Add]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}
		private void Mul()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(_stack.Pop() * _stack.Pop());
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Mul]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Sub()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					// pay attention on minus
					//           v
					_stack.Push( - _stack.Pop() + _stack.Pop());
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Sub]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Div()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					long b = _stack.Pop(),
			     	     a = _stack.Pop();
					_stack.Push(a / b);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Div]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Rem()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					long b = _stack.Pop(),
			     	     a = _stack.Pop();
					_stack.Push(a % b);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Rem]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void GreaterThan()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(_stack.Pop() < _stack.Pop() ? 1 : 0);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GreaterThan]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Not()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(_stack.Pop() == 0 ? 1 : 0);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Not]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		// D0 to D15 were replaced with DN
		/*
		private void D0()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(0);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D0]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D1()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(1);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D1]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D2()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(2);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D2]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D3()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(3);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D3]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D4()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(4);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D4]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D5()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(5);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D5]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D6()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(6);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D6]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D7()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(7);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D7]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D8()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(8);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D8]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D9()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(9);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D9]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D10()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(10);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D10]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D11()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(11);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D11]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D12()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(12);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D12]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D13()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(13);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D13]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D14()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(14);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D14]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void D15()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(15);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D15]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}
		*/

		private void DN(char n, Instruction instruction)
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(n);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.D15]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void InInt()
		{
			throw new NotImplementedException();
		}

		private void InChar()
		{
			throw new NotImplementedException();
		}

		private void InFile()
		{
			throw new NotImplementedException();
		}

		private void OutInt()
		{
			throw new NotImplementedException();
		}

		private void OutChar()
		{
			throw new NotImplementedException();
		}

		private void OutFile()
		{
			throw new NotImplementedException();
		}

		private void Pop()
		{
			throw new NotImplementedException();
		}

		private void Dup()
		{
			throw new NotImplementedException();
		}

		private void Swap()
		{
			throw new NotImplementedException();
		}

		private void Clear()
		{
			throw new NotImplementedException();
		}

		private void Get()
		{
			throw new NotImplementedException();
		}

		private void Put()
		{
			throw new NotImplementedException();
		}

		private void Fetch()
		{
			throw new NotImplementedException();
		}

		private void Store()
		{
			throw new NotImplementedException();
		}

		private void StackUnderStack()
		{
			throw new NotImplementedException();
		}

		private void Begin()
		{
			throw new NotImplementedException();
		}

		private void End()
		{
			throw new NotImplementedException();
		}

		private void LoadSem()
		{
			throw new NotImplementedException();
		}

		private void UnloadSem()
		{
			throw new NotImplementedException();
		}

		private void Space()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					// nop
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Space]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Stop()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_run = false;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Stop]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Stringmode()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_mode = InterpreterMode.String;
					break;
				case InterpreterMode.String:
					_mode = InterpreterMode.Normal;
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Nop()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					// nop
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Nop]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Exec()
		{
			throw new NotImplementedException();
		}

		private void Quit()
		{
			throw new NotImplementedException();
		}

		private void Split()
		{
			throw new NotImplementedException();
		}

		private void GetSysInfo()
		{
			throw new NotImplementedException();
		}
	}
}
