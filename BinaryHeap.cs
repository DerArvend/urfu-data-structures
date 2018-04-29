using System;
using System.Collections.Generic;

namespace Structures
{
	public class BinaryHeap<T>
		where T : IComparable<T>
	{
		public List<T> list;
		public int Count => list.Count;

		public BinaryHeap()
		{
			list = new List<T>();
		}

		public BinaryHeap(IEnumerable<T> source)
		{
			list = new List<T>();
			foreach (var v in source)
			{
				Add(v);
			}
		}

		public void Add(T value)
		{
			list.Add(value);

			var i = list.Count - 1;
			var parentIndex = (i - 1) / 2;
			while (list[parentIndex].CompareTo(list[i]) < 0)
			{
				Swap(parentIndex, i);
				i = parentIndex;
				parentIndex = i % 2 == 0 ? (i - 1) / 2 : i / 2;
			}
		}

		public T PopRoot()
		{
			var root = list[0];
			list[0] = list[list.Count - 1];
			list[list.Count - 1] = root;
			list.RemoveAt(list.Count - 1);
			Heapify();
			return root;
		}

		private void Heapify()
		{
			var i = 0;
			while (i < list.Count / 2)
			{
				if (list.Count < 2 * i + 1)
					Swap(i, 2 * i + 1, () => list[i].CompareTo(list[2 * i + 1]) < 0);
				if (list.Count < 2 * i + 2)
					Swap(i, 2 * i + 2, () => list[i].CompareTo(list[2 * i + 2]) < 0);
				i++;
			}
		}

		private void Swap(int i1, int i2, Func<bool> predicate = null)
		{
			if (predicate == null || predicate())
			{
				var tmp = list[i1];
				list[i1] = list[i2];
				list[i2] = tmp;
			}
		}
	}
}