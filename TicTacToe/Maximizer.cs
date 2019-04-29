using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
	class Maximizer<T> : Node<T>
	{
		public override void Compare(int childScore)
		{
			Score = Math.Max(Score.Value, (int)(childScore * 0.9f));
		}

		public override Node<T> CreateNode(T data)
		{
			var node = new Minimizer<T>()
			{
				Data = data
			};

			Children.Add(node);
			node.Parent = this;
			return node;
		}

		public override void SetUnlimitedValue()
		{
			Score = -UnlimitedValue;
		}
	}
}
