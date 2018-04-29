using System;
using System.Collections;
using System.Collections.Generic;

namespace Structures
{
	public class RandomizedTree<T> : IEnumerable<T>
		where T : IComparable
	{
		public RandomizedTree<T> Left { get; private set; }
		public RandomizedTree<T> Right { get; private set; }
		public T Value { get; private set; }
		public int Count { get; private set; }
		public int Height
		{
			get
			{
				if (Left == null && Right == null) return 1;
				var leftHeight = Left?.Height ?? 0;
				var rightHeight = Right?.Height ?? 0;
				return 1 + Math.Max(leftHeight, rightHeight);
			}
		}
		private Random random;
		protected bool hasValue;

		public RandomizedTree(T value)
		{
			Value = value;
			hasValue = true;
			Count = 1;
			random = new Random();
		}

		public RandomizedTree()
		{
			random = new Random();
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
			Right = new RandomizedTree<T>(Value);
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
			Left = new RandomizedTree<T>(Value);
			Value = oldRight.Value;
			Left.Right = oldRight.Left;
			Left.Left = oldLeft;
		}

		public void Add(T key)
		{
			Insert(key, false);
		}

		private void Insert(T key, bool insertToRoot)
		{
			Count++;
			if (!hasValue)
			{
				Value = key;
				hasValue = true;
				return;
			}

			if (!insertToRoot) insertToRoot = ShouldInsertToRoot();

			if (key.CompareTo(Value) < 0)
			{
				if (Left == null) Left = new RandomizedTree<T>();
				Left.Insert(key, insertToRoot);
				if (insertToRoot) RotateRight();
			}
			else
			{
				if (Right == null) Right = new RandomizedTree<T>();
				Right.Insert(key, insertToRoot);
				if (insertToRoot) RotateLeft();
			}
		}

		private bool ShouldInsertToRoot()
		{
			return random.NextDouble() < 1d / (Count + 1);
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

		private static IEnumerable<T> GetTreeValues(RandomizedTree<T> node)
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