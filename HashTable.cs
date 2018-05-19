using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures
{
	class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		public int Count { get; set; }
		private readonly IEqualityComparer<TKey> comparer;
		private List<KeyValuePair<TKey, TValue>>[] entries;
		private int capacity;

		public HashTable(IEqualityComparer<TKey> comparer = null, int capacity = 3)
		{
			this.comparer = comparer;
			this.capacity = capacity;
			entries = new List<KeyValuePair<TKey, TValue>>[capacity]
				.Select(x => new List<KeyValuePair<TKey, TValue>>())
				.ToArray();
		}

		public void AddOrUpdate(TKey key, TValue value)
		{
			Count++;
			if (Count > capacity)
				Reallocate();

			var list = entries[key.GetHashCode().Abs() % capacity];
			var occurenceIndex = list.FindIndex(x => KeysEqual(x.Key, key));
			if (occurenceIndex != -1)
				list[occurenceIndex] = new KeyValuePair<TKey, TValue>(key, value);
			else
			{
				list.Add(new KeyValuePair<TKey, TValue>(key, value));
				if(list.Count > capacity / 2) Reallocate();
			}
		}

		public TValue Get(TKey key)
		{
			var list = entries[key.GetHashCode().Abs() % capacity];
			var occurenceIndex = list.FindIndex(x => KeysEqual(x.Key, key));
			if (occurenceIndex == -1) throw new InvalidOperationException("Key was not present in HashTable");
			return list[occurenceIndex].Value;
		}

		private void Reallocate()
		{
			var newCapacity = capacity * 2;
			var newEntries = new List<KeyValuePair<TKey, TValue>>[newCapacity]
				.Select(x => new List<KeyValuePair<TKey, TValue>>())
				.ToArray();
			foreach (var list in entries)
			{
				foreach (var kv in list)
				{
					var index = kv.Value.GetHashCode() % newCapacity;
					newEntries[index].Add(kv);
				}
			}

			capacity = newCapacity;
			entries = newEntries;
		}

		public void Remove(TKey key)
		{
			var list = entries[key.GetHashCode().Abs() % capacity];
			var occurenceIndex = list.FindIndex(x => KeysEqual(x.Key, key));
			if (occurenceIndex != -1)
			{
				list.RemoveAt(occurenceIndex);
				Count--;
			}
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			foreach (var entry in entries)
			{
				foreach (var kv in entry)
				{
					builder.Append($"{kv.Key.ToString()}:{kv.Value.ToString()}; ");
				}
			}

			return builder.ToString();
		}

		private bool KeysEqual(TKey value1, TKey value2)
		{
			return comparer?.Equals(value1, value2) ?? value1.Equals(value2);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return entries.SelectMany(entry => entry).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}