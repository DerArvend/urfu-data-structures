using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures
{
	class HashTableOpenAddressed<TKey, TValue> 
	{
		public int Count { get; set; }
		private readonly IEqualityComparer<TKey> comparer;
		private KeyValuePair<TKey, TValue>[] entries;
		private bool[] emptyCells;
		private int capacity;

		public HashTableOpenAddressed(IEqualityComparer<TKey> comparer = null, int capacity = 3)
		{
			this.comparer = comparer;
			this.capacity = capacity;
			entries = new KeyValuePair<TKey, TValue>[capacity];
			this.capacity = capacity;
			emptyCells = Enumerable.Repeat(true, capacity).ToArray();
		}

		public void AddOrUpdate(TKey key, TValue value)
		{
			Count++;
			if (Count > capacity / 2)
				Reallocate();

			var index = key.GetHashCode().Abs() % capacity;
			while (emptyCells[index] == false && !KeysEqual(entries[index].Key, key))
			{
				index = GetNextIndex(index);
			}
			entries[index] = new KeyValuePair<TKey, TValue>(key, value);
			emptyCells[index] = false;
		}

		public TValue Get(TKey key)
		{
			var index = key.GetHashCode().Abs() % capacity;
			if (Count == 0) throw new InvalidOperationException();
			while (true)
			{
				if (!emptyCells[index] && KeysEqual(entries[index].Key, key))
					return entries[index].Value;
				index = GetNextIndex(index);
			}
		}

		public void Remove(TKey key)
		{
			var index = key.GetHashCode().Abs() % capacity;
			var attempts = 0;
			while (true)
			{
				attempts++;
				if (attempts == capacity) throw new InvalidOperationException();

				if (emptyCells[index] == false && KeysEqual(entries[index].Key, key))
				{
					emptyCells[index] = true;
					return;
				}
				
				index = GetNextIndex(index);
			}
		}

		private void Reallocate()
		{
			var newCapacity = capacity * 2;
			var newEntries = new KeyValuePair<TKey, TValue>[newCapacity];
			var newEmptyCells = Enumerable.Repeat(true, newCapacity).ToArray();
			for (int i = 0; i < entries.Length; i++)
			{
				newEntries[i] = entries[i];
				newEmptyCells[i] = emptyCells[i];
			}
			entries = newEntries;
			emptyCells = newEmptyCells;
			capacity = newCapacity;
		}

		private bool KeysEqual(TKey value1, TKey value2) => comparer?.Equals(value1, value2) ?? value1.Equals(value2);

		private int GetNextIndex(int oldIndex)
		{
			return (oldIndex + 1) % entries.Length;
		}
	}
}
