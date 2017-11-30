using System;

namespace Befunge.Lang
{
	public enum Instructions
	{
		Undefined       = 0,

		// absolute flow control
		GoRight         = 62,  // <
		GoLeft          = 60,  // >
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
		D0 = 48, // 0
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
		D10 = 97,
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
		Begin           = 123, // {}
		End             = 125, // }


		// fingerprints manipulation
		LoadSem         = 40,  // (
		UnloadSem       = 41,  // )

		// misc instructions
		Space           = 32,
		Stop            = 64,  // @
		Stringmode      = 34,  // "
		Nop             = 122, // z
		Exec            = 61,  // =
		Quit            = 113, // q
		Split           = 116, // t
		GetSysInfo      = 121, // y
	}
}