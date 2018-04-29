using System;
using System.Collections;
using System.Collections.Generic;

namespace Structures
{
	public class AVLTree<T> : IEnumerable<T>
		where T : IComparable
	{
		public AVLTree<T> Left { get; private set; }
		public AVLTree<T> Right { get; private set; }
		public T Value { get; private set; }
		public int Height { get; private set; }

		private int BalanceFactor
		{
			get
			{
				var leftHeight = Left?.Height ?? 0;
				var rightHeight = Right?.Height ?? 0;
				return rightHeight - leftHeight;
			}
		}

		public int Count { get; private set; }

		private bool hasValue;

		public AVLTree(T value)
		{
			Value = value;
			hasValue = true;
			Count = 1;
		}

		public AVLTree()
		{
		}

		public T this[int index]
		{
			get
			{
				var nextNode = this;
				while (true)
				{
					if (index >= nextNode.Count || index < 0) throw new IndexOutOfRangeException();

					var leftCount = nextNode.Left?.Count ?? 0;
					if (index == leftCount)
						return nextNode.Value;

					if (index < leftCount)
					{
						nextNode = nextNode.Left;
						continue;
					}

					index = index - leftCount - 1;
					nextNode = nextNode.Right;
				}
			}
		}

		private void RotateRight()
		{
			if (Left == null) throw new InvalidOperationException();
			var oldLeft = Left;
			var oldRight = Right;
			Left = Left.Left;
			Right = new AVLTree<T>(Value);
			Value = oldLeft.Value;
			Right.Left = oldLeft.Right;
			Right.Right = oldRight;
			FixHeight();
			Right.FixHeight();
		}

		private void RotateLeft()
		{
			if (Right == null) throw new InvalidOperationException();
			var oldLeft = Left;
			var oldRight = Right;
			Right = Right.Right;
			Left = new AVLTree<T>(Value);
			Value = oldRight.Value;
			Left.Right = oldRight.Left;
			Left.Left = oldLeft;
			FixHeight();
			Left.FixHeight();
		}

		private void Balance()
		{
			FixHeight();
			if (BalanceFactor == 2)
			{
				if (Right.BalanceFactor < 0) Right.RotateRight();
				RotateLeft();
			}
			else if (BalanceFactor == -2)
			{
				if (Left.BalanceFactor < 0) Left.RotateLeft();
				RotateRight();
			}
		}

		public void Add(T key)
		{
			Count++;
			if (!hasValue)
			{
				Value = key;
				hasValue = true;
				return;
			}

			if (key.CompareTo(Value) < 0)
			{
				if (Left == null) Left = new AVLTree<T>();
				Left.Add(key);
			}
			else
			{
				if (Right == null) Right = new AVLTree<T>();
				Right.Add(key);
			}

			Balance();
		}


		public void FixHeight()
		{
			var leftHeight = Left?.Height ?? 0;
			var rightHeight = Right?.Height ?? 0;
			Height = Math.Max(leftHeight, rightHeight) + 1;
		}

		public bool Contains(T value)
		{
			if (!hasValue) return false;

			var nextNode = this;
			while (true)
			{
				if (value.Equals(nextNode.Value)) return true;
				var node = value.CompareTo(nextNode.Value) > 0 ? nextNode.Right : nextNode.Left;

				if (node == null)
					return false;
				nextNode = node;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return GetTreeValues(this).GetEnumerator();
		}

		private static IEnumerable<T> GetTreeValues(AVLTree<T> node)
		{
			if (node.Left != null)
				foreach (var value in GetTreeValues(node.Left))
					yield return value;
			yield return node.Value;
			if (node.Right != null)
				foreach (var value in GetTreeValues(node.Right))
					yield return value;
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}