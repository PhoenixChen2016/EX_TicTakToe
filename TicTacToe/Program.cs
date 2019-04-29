using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
	class Program
	{
		static Random random = new Random();
		static void Main(string[] args)
		{
			var checkerBoard = new int[3, 3];

			PrintCheckerBoard(checkerBoard);

			while (true)
			{
				var playerInput = Console.ReadLine();

				if (playerInput.Equals("n", StringComparison.OrdinalIgnoreCase))
				{
					checkerBoard = new int[3, 3];
					PrintCheckerBoard(checkerBoard);
					continue;
				}

				var point = playerInput.Split(',');
				var inputX = int.Parse(point[0]) - 1;
				var inputY = int.Parse(point[1]) - 1;

				checkerBoard[inputY, inputX] = 1;

				PrintCheckerBoard(checkerBoard);

				var bestNode = SelectBest(checkerBoard);
				checkerBoard[bestNode.Data.Y, bestNode.Data.X] = -1;

				PrintCheckerBoard(checkerBoard);

				
			}
		}

		static Node<(int X, int Y, int[,] Board)> SelectBest(int[,] checkerBoard)
		{
			var minNode = new Maximizer<(int X, int Y, int[,] Board)>
			{
				Data = (-1, -1, (int[,])checkerBoard.Clone())
			};
			var steps = new Stack<Node<(int X, int Y, int[,] Board)>>();
			steps.Push(minNode);

			while (steps.Count > 0)
			{
				var node = steps.Peek();

				CalculateScore(node, steps);
			}

			var bestNode = (from node in minNode.Children
							group node by node.Score.Value into g
							orderby g.Key descending
							from node in g.Select(node => (random.Next(), node))
								.OrderBy(t => t.Item1)
								.Select(t => t.Item2)
							select node).First();

			return bestNode;
		}

		static void CalculateScore(
			Node<(int X, int Y, int[,] Board)> node,
			Stack<Node<(int X, int Y, int[,] Board)>> steps)
		{
			if (node.Score == null)
			{
				var board = node.Data.Board;
				var hand = node is Minimizer<(int X, int Y, int[,] Board)> ? 1 : -1;
				node.Score = -CalculateScore(board) * 100;

				if (node.Score == 0)
					for (var y = 0; y < 3; y++)
						for (var x = 0; x < 3; x++)
							if (board[y, x] == 0)
							{
								var cloneBoard = (int[,])board.Clone();

								cloneBoard[y, x] = hand;

								steps.Push(node.CreateNode((x, y, cloneBoard)));
							}

				if (node.Children.Count != 0)
				{
					node.SetUnlimitedValue();
				}
			}
			else
			{
				node.Parent?.Compare(node.Score.Value);
				steps.Pop();
				return;
			}
		}

		static int CalculateScore(int[,] board)
		{
			for (var y = 0; y < 3; y++)
				for (var x = 0; x < 3; x++)
					if (board[y, x] != 0)
					{
						if (y - 1 >= 0 && y + 1 <= 2)
							if (board[y - 1, x] == board[y, x] && board[y, x] == board[y + 1, x])
								return board[y, x];

						if (x - 1 >= 0 && x + 1 <= 2)
							if ((board[y, x - 1], board[y, x]) == (board[y, x], board[y, x + 1]))
								return board[y, x];

						if (x == 1 && y == 1)
						{
							if ((board[y - 1, x - 1], board[y, x]) == (board[y, x], board[y + 1, x + 1]))
								return board[y, x];

							if ((board[y - 1, x + 1], board[y, x]) == (board[y, x], board[y + 1, x - 1]))
								return board[y, x];
						}
					}

			return 0;
		}

		static void PrintCheckerBoard(int[,] board)
		{
			for (var y = 0; y < 3; y++)
			{
				Console.Write('|');
				for (var x = 0; x < 3; x++)
					Console.Write($"{PrintPiece(board[x, y]),1}|");
				Console.WriteLine();
			}
			Console.WriteLine("-------------------------------------------------");
		}

		static string PrintPiece(int piece)
		{
			switch (piece)
			{
				case 1:
					return "O";
				case -1:
					return "X";
				default:
					return string.Empty;
			}
		}
	}
}
