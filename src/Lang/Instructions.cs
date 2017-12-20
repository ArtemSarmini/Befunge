using System.Collections.Generic;

namespace Befunge.Lang
{
	public enum Instruction
	{
		// do we need it?
		// Undefined       = 0,

		// absolute flow control
		GoRight         = 62,  // >
		GoLeft          = 60,  // <
		GoUp            = 94,  // ^
		GoDown          = 118, // v
		GoForward       = 104, // h
		GoBackward      = 108, // l
		GoRand          = 63,  // ?
		JumpOver        = 59,  // ;
		AbsDelta        = 120, // x

		// relative flow control
		Trampoline      = 35,  // #
		TurnRight       = 93,  // ]
		TurnLeft        = 91,  // [
		Reflect         = 114, // r

		// conditional flow control
		HorIf           = 95,  // _
		VertIf          = 124, // |
		SagIf           = 109, // m
		Cmp             = 119, // w
		Jump            = 106, // j
		Iter            = 107, // k

		// math and logic
		Add             = 43,  // +
		Sub             = 45,  // -
		Mul             = 42,  // *
		Div             = 47,  // /
		Rem             = 37,  // %
		GreaterThan     = 96,  // `
		Not             = 33,  // !

		// push 0 to 9
		D0              = 48,
		D1,
		D2,
		D3,
		D4,
		D5,
		D6,
		D7,
		D8,
		D9,

		// push 10 to 15
		D10             = 97,
		D11,
		D12,
		D13,
		D14,
		D15,

		// IO
		InInt           = 38,  // &
		InChar          = 126, // ~
		InFile          = 105, // i
		OutInt          = 46,  // .
		OutChar         = 44,  // ,
		OutFile         = 111, // o

		// stack manipulation
		Pop             = 36,  // $
		Dup             = 58,  // :
		Swap            = 92,  // \
		Clear           = 110, // n

		Get             = 103, // g
		Put             = 112, // p

		Fetch           = 39,  // '
		Store           = 115, // s

		// stack stack manipulation
		StackUnderStack = 117, // u
		Begin           = 123, // {
		End             = 125, // }


		// fingerprints manipulation
		LoadSem         = 40,  // (
		UnloadSem       = 41,  // )

		// misc Instructions
		Space           = 32,
		Stop            = 64,  // @
		Stringmode      = 34,  // "
		Nop             = 122, // z
		Exec            = 61,  // =
		Quit            = 113, // q
		Split           = 116, // t
		GetSysInfo      = 121, // y
	}

	public static class InstructionConverter
	{
		public static readonly Dictionary<Instruction, char> ToChar =
			new Dictionary<Instruction, char>( new KeyValuePair<Instruction, char>[] {
				MakePair(Instruction.GoRight,         '>'),
				MakePair(Instruction.GoLeft,          '<'),
				MakePair(Instruction.GoUp,            '^'),
				MakePair(Instruction.GoDown,          'v'),
				MakePair(Instruction.GoForward,       'h'),
				MakePair(Instruction.GoBackward,      'l'),
				MakePair(Instruction.GoRand,          '?'),
				MakePair(Instruction.JumpOver,        ';'),
				MakePair(Instruction.AbsDelta,        'x'),
				MakePair(Instruction.Trampoline,      '#'),
				MakePair(Instruction.TurnRight,       ']'),
				MakePair(Instruction.TurnLeft,        '['),
				MakePair(Instruction.Reflect,         'r'),
				MakePair(Instruction.HorIf,           '_'),
				MakePair(Instruction.VertIf,          '|'),
				MakePair(Instruction.SagIf,           'm'),
				MakePair(Instruction.Cmp,             'w'),
				MakePair(Instruction.Jump,            'j'),
				MakePair(Instruction.Iter,            'k'),
				MakePair(Instruction.Add,             '+'),
				MakePair(Instruction.Sub,             '-'),
				MakePair(Instruction.Mul,             '*'),
				MakePair(Instruction.Div,             '/'),
				MakePair(Instruction.Rem,             '%'),
				MakePair(Instruction.GreaterThan,     '`'),
				MakePair(Instruction.Not,             '!'),
				MakePair(Instruction.D0,              '0'),
				MakePair(Instruction.D1,              '1'),
				MakePair(Instruction.D2,              '2'),
				MakePair(Instruction.D3,              '3'),
				MakePair(Instruction.D4,              '4'),
				MakePair(Instruction.D5,              '5'),
				MakePair(Instruction.D6,              '6'),
				MakePair(Instruction.D7,              '7'),
				MakePair(Instruction.D8,              '8'),
				MakePair(Instruction.D9,              '9'),
				MakePair(Instruction.D10,             'a'),
				MakePair(Instruction.D11,             'b'),
				MakePair(Instruction.D12,             'c'),
				MakePair(Instruction.D13,             'd'),
				MakePair(Instruction.D14,             'e'),
				MakePair(Instruction.D15,             'f'),
				MakePair(Instruction.InInt,           '&'),
				MakePair(Instruction.InChar,          '~'),
				MakePair(Instruction.InFile,          'i'),
				MakePair(Instruction.OutInt,          '.'),
				MakePair(Instruction.OutChar,         ','),
				MakePair(Instruction.OutFile,         'o'),
				MakePair(Instruction.Pop,             '$'),
				MakePair(Instruction.Dup,             ':'),
				MakePair(Instruction.Swap,            '\\'),
				MakePair(Instruction.Clear,           'n'),
				MakePair(Instruction.Get,             'g'),
				MakePair(Instruction.Put,             'p'),
				MakePair(Instruction.Fetch,           '\''),
				MakePair(Instruction.Store,           's'),
				MakePair(Instruction.StackUnderStack, 'u'),
				MakePair(Instruction.Begin,           '{'),
				MakePair(Instruction.End,             '}'),
				MakePair(Instruction.LoadSem,         '('),
				MakePair(Instruction.UnloadSem,       ')'),
				MakePair(Instruction.Space,           ' '),
				MakePair(Instruction.Stop,            '@'),
				MakePair(Instruction.Stringmode,      '\"'),
				MakePair(Instruction.Nop,             'z'),
				MakePair(Instruction.Exec,            '='),
				MakePair(Instruction.Quit,            'q'),
				MakePair(Instruction.Split,           't'),
				MakePair(Instruction.GetSysInfo,      'y') } );

		public static readonly Dictionary<char, Instruction> ToInstruction =
			System.Linq.Enumerable.ToDictionary(ToChar, p => p.Value, p => p.Key);

		// to avoid explicit generic type indication in the code above
		private static KeyValuePair<Instruction, char> MakePair(
			Instruction Instruction, char c)
				=> new KeyValuePair<Instruction, char>(Instruction, c);
	}
}
