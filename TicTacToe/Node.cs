using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	abstract class Node<T>
	{
		public static int UnlimitedValue = 1000;

		public int? Score { get; set; }
		public Node<T> Parent { get; set; }
		public List<Node<T>> Children { get; } = new List<Node<T>>();
		public T Data { get; set; }

		public abstract void SetUnlimitedValue();
		public abstract Node<T> CreateNode(T data);
		public abstract void Compare(int childScore);
	}
}
