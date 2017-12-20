using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
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
		private string _separatorLine;
		private string _input, _output;

		public CodeRunner(Field field)
		{
			_field = field;
			_ip    = new InstructionPointer();
			_mode  = InterpreterMode.Normal;
			_stack = new Stack<long>();

			_rnd           = new Random();
			_run           = true;
			//                                 '═'
			_separatorLine = new string((char)0x2550, Field.Width);
			_input         = "";
			_output        = "";
		}

		public void Run()
		{
			Console.CursorVisible = false;
			while (_run)
			{
				Console.Clear();
				Print();

				char instruction = (char)_field[_ip];

				if (!Helpers.IsInstruction(_field[_ip]))
				{
					Reflect();
				}
				// A to Z are reserved and aren't currently supported
				// they can be pushed on stack in stringmode though
				else if (instruction >= 'A' && instruction <= 'Z')
				{
					Reserved(instruction);
				}
				// optimization for instructions 1 to 9 and a to f
				// in this case proper instruction can be called without reflection
				else if (instruction >= '0' && instruction <= '9')
				{
					DN(instruction - '0', InstructionConverter.ToInstruction[instruction]);
				}
				else if (instruction >= 'a' && instruction <= 'f')
				{
					DN(instruction - 'a', InstructionConverter.ToInstruction[instruction]);
				}
				else
				{
					MethodInfo command = typeof(CodeRunner).GetMethod(
						InstructionConverter.ToInstruction[instruction].ToString(),
						BindingFlags.NonPublic | BindingFlags.Instance);
					try
					{
						command.Invoke(this, null);
					}
					catch (TargetInvocationException e)
					{
						if (e.InnerException is NotImplementedException)
							throw e.InnerException;
						else
							throw;
					}
				}
				switch (Program.Speed)
				{
					case Speed.Fast:
						Thread.Sleep(250);
						break;
					case Speed.Slow:
						Thread.Sleep(3000);
						break;
					case Speed.OnKeypress:
						var _ = Console.ReadKey(true);
						break;
					default:
						ThrowHelper.ThrowInvalidEnumValueException(nameof(Program.Speed), nameof(Speed));
						break;
				}
			}
			Console.CursorVisible = true;
		}

		private void Print()
		{
			_field.Print(_ip);
			Console.WriteLine(_separatorLine);
			Console.WriteLine($"IP: x = {_ip.X}, y = {_ip.Y}, direction = {_ip.Direction}");
			Console.WriteLine(_separatorLine);
			Console.WriteLine("Mode: " + _mode);
			Console.WriteLine(_separatorLine);
			Console.WriteLine("Stack: " + string.Join(' ', _stack.Reverse()));
			Console.WriteLine(_separatorLine);
			Console.WriteLine("Input: " + string.Join("", _input));
			Console.WriteLine(_separatorLine);
			Console.WriteLine("Output: " + string.Join("", _output));
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
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.JumpOver);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.JumpOver]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void AbsDelta()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.AbsDelta);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.AbsDelta]);
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
			// no move here
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
					_ip.Direction = _stack.SafePop() == 0
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
					_ip.Direction = _stack.SafePop() == 0
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
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.SagIf);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.SagIf]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Cmp()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					long b = _stack.SafePop(),
			     	     a = _stack.SafePop();
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
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.Jump);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Jump]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Iter()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.Iter);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Iter]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Add()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(_stack.SafePop() + _stack.SafePop());
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
					_stack.Push(_stack.SafePop() * _stack.SafePop());
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
					_stack.Push( - _stack.SafePop() + _stack.SafePop());
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
					long b = _stack.SafePop(),
			     	     a = _stack.SafePop();
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
					long b = _stack.SafePop(),
			     	     a = _stack.SafePop();
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
					_stack.Push(_stack.SafePop() < _stack.SafePop() ? 1 : 0);
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
					_stack.Push(_stack.SafePop() == 0 ? 1 : 0);
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

		private void DN(int n, Instruction instruction)
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(n);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[instruction]);
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
			switch (_mode)
			{
				case InterpreterMode.Normal:
					Console.WriteLine(_separatorLine);
					Console.WriteLine("Waiting for input");
					char c = Console.ReadKey(true).KeyChar;
					if (c < '0' || c > '9')
						throw new InvalidOperationException("Input must be a digit");
					_stack.Push(c - '0');
					_input += c;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.InInt]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void InChar()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					Console.WriteLine(_separatorLine);
					Console.WriteLine("Waiting for input");
					char c = Console.ReadKey(true).KeyChar;
					_stack.Push(c);
					_input += c;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.InChar]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void InFile()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.InFile);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.InFile]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void OutInt()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_output += _stack.SafePop();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.OutInt]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void OutChar()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_output += (char)_stack.SafePop();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.OutChar]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void OutFile()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.OutFile);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.OutFile]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Pop()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.SafePop();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Pop]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Dup()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Push(_stack.Peek());
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Dup]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Swap()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					long b = _stack.SafePop(),
					     a = _stack.SafePop();
					_stack.Push(b);
					_stack.Push(a);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Swap]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Clear()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_stack.Clear();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Clear]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Get()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					long y = _stack.SafePop(),
					     x = _stack.SafePop();
					var cell = new InstructionPointer()
					{
						X = (int)x,
						Y = (int)y
					};
					_stack.Push(_field[cell]);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Get]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Put()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					long y = _stack.SafePop(),
					     x = _stack.SafePop(),
						 v = _stack.SafePop();
					var cell = new InstructionPointer()
					{
						X = (int)x,
						Y = (int)y
					};
					_field[cell] = v;
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Put]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Fetch()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Move();
					_stack.Push(_field[_ip]);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Fetch]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Store()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					_ip.Move();
					_field[_ip] = _stack.SafePop();
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Store]);
					_ip.Move();
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			// no move here
		}

		private void StackUnderStack()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.StackUnderStack);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.StackUnderStack]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Begin()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.Begin);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Begin]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void End()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.End);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.End]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void LoadSem()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.LoadSem);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.LoadSem]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void UnloadSem()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.UnloadSem);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.UnloadSem]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
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
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.Exec);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Exec]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Quit()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.Quit);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Quit]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void Split()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.Split);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.Split]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		private void GetSysInfo()
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					ThrowHelper.ThrowNotImplementedInstructionException(Instruction.GetSysInfo);
					break;
				case InterpreterMode.String:
					_stack.Push(InstructionConverter.ToChar[Instruction.GetSysInfo]);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}

		// A to Z are reserved for fingerprints
		private void Reserved(char n)
		{
			switch (_mode)
			{
				case InterpreterMode.Normal:
					Reflect();
					break;
				case InterpreterMode.String:
					_stack.Push(n);
					break;
				default:
					ThrowHelper.ThrowInvalidEnumValueException(
						nameof(_mode),
						nameof(InstructionPointerDirection));
					break;
			}
			_ip.Move();
		}
	}
}
