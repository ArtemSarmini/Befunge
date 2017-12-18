using System;

namespace Befunge.Interpreter
{
	internal static class Helpers
	{
		internal static bool IsInstruction(char c) =>
			c > 31 && c < 127;

		internal static bool IsInstruction(long l) =>
			l > 31 && l < 127;

		internal static char ToPrintable(char c) =>
			IsInstruction(c) ? c : (char)0xFFFD;

		internal static char ToPrintable(long l) =>
			(char)(IsInstruction(l) ? l : 0xFFFDL);
	}

	internal static class ThrowHelper
	{
		internal static void ThrowInvalidEnumValueException(string variable, string enumName) =>
			throw new InvalidOperationException(
				$"{variable} doesn't represent any possible {enumName} value");
	}
}
