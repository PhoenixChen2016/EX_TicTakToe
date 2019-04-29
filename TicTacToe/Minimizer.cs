using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
	class Minimizer<T> : Node<T>
	{
		public override void Compare(int childScore)
		{
			Score = Math.Min(Score.Value, (int)(childScore * 0.9f));
		}

		public override Node<T> CreateNode(T data)
		{
			var node = new Maximizer<T>
			{
				Data = data
			};

			Children.Add(node);
			node.Parent = this;
			return node;
		}

		public override void SetUnlimitedValue()
		{
			Score = UnlimitedValue;
		}
	}
}
