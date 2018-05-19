using System;
using System.Collections;
using System.Collections.Generic;

namespace Structures
{
	public class BinarySearchTree<T> : IEnumerable<T>
		where T : IComparable
	{
		public BinarySearchTree<T> Left { get; private set; }
		public BinarySearchTree<T> Right { get; private set; }
		public T Value { get; private set; }
		public int Count { get; private set; }
		private bool hasValue;

		public BinarySearchTree(T value)
		{
			Value = value;
			hasValue = true;
			Count = 1;
		}

		public BinarySearchTree()
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
			Right = new BinarySearchTree<T>(Value);
			Value = oldLeft.Value;
			Right.Left = oldLeft.Right;
			Right.Right = oldRight;
			
		}

		private void RotateLeft()
		{
			if (Right == null) throw new InvalidOperationException();
			var oldLeft = Left;
			var oldRight = Right;
			Right = Right.Right;
			Left = new BinarySearchTree<T>(Value);
			Value = oldRight.Value;
			Left.Right = oldRight.Left;
			Left.Left = oldLeft;
		}

		public void Add(T key)
		{
			Insert(key, false);
		}

		private void Insert(T key, bool shouldInsertToRoot)
		{
			Count++;
			if (!hasValue)
			{
				Value = key;
				hasValue = true;
			}

			else if (key.CompareTo(Value) < 0)
			{
				if (Left == null) Left = new BinarySearchTree<T>();
				Left.Insert(key, shouldInsertToRoot);
				if(shouldInsertToRoot) RotateRight();
			}
			else
			{
				if (Right == null) Right = new BinarySearchTree<T>();
				Right.Insert(key, shouldInsertToRoot);
				if(shouldInsertToRoot) RotateLeft();
			}
		}

		public void NonRecoursiveAdd(T key)
		{
			Count++;
			if (!hasValue)
			{
				Value = key;
				hasValue = true;
				return;
			}

			var nextNode = this;
			while (true)
			{
				if (key.CompareTo(nextNode.Value) >= 0)
				{
					if (nextNode.Right == null)
					{
						nextNode.Right = new BinarySearchTree<T>(key);
						return;
					}

					nextNode = nextNode.Right;
					nextNode.Count++;
				}

				else
				{
					if (nextNode.Left == null)
					{
						nextNode.Left = new BinarySearchTree<T>(key);
						return;
					}

					nextNode = nextNode.Left;
					nextNode.Count++;
				}
			}
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

		private static IEnumerable<T> GetTreeValues(BinarySearchTree<T> node)
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