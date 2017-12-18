using System;
using System.Collections.Generic;

namespace Befunge.Interpreter
{
	public class Field
	{
		List<List<Node>> _field;

		public Field()
		{
			_field = new List<List<Node>>(16);
			for (int i = 0; i < 16; i++)
			{
				_field.Add(new List<Node>()
					{ new Node(32), new Node(32), new Node(32), new Node(32),
					  new Node(32), new Node(32), new Node(32), new Node(32),
                      new Node(32), new Node(32), new Node(32), new Node(32),
                      new Node(32), new Node(32), new Node(32), new Node(32), });
			}
		}

		public Node this [int x, int y]
		{
			get => _field[x][y];
			set => _field[x][y] = value;
		}

		public Node this [InstructionPointer ip]
		{
			get => _field[ip.X][ip.Y];
			set => _field[ip.X][ip.Y] = value;
		}

        public void Print()
        {
            for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					// output is row by row, so it's valid args order
					Console.Write(Helpers.ToPrintable(_field[j][i]));
				}
				Console.WriteLine();
			}
        }
	}

	public class Node
	{
		int _x, _y, _z;
		long _value;

        public Node(long value) => _value = value;

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

		public long Value
		{
			get => _value;
		    set => _value = value;
		}

        public static implicit operator long(Node node) => node.Value;

		public static explicit operator char(Node node) => (char)node.Value;

        public void ChangeTo(long value) => _value = value;
	}
}
