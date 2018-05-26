using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures
{
	public class Trie<TValue>
	{
		private readonly TrieNode<TValue> root;

		public Trie()
		{
			root = new TrieNode<TValue>();
		}

		public void Insert(string key, TValue value)
		{
			if (string.IsNullOrEmpty(key)) throw new ArgumentException();
			var currentNode = root;
			foreach (var c in key)
			{
				if (!currentNode.Nodes.ContainsKey(c)) currentNode.Nodes[c] = new TrieNode<TValue>();
				currentNode = currentNode.Nodes[c];
			}
			currentNode.Value = value;
		}

		private (TrieNode<TValue> node, bool wasFound) TryFindNode(string key)
		{
			if (string.IsNullOrEmpty(key)) throw new ArgumentException();
			var currentNode = root;
			foreach (var c in key)
			{
				if (!currentNode.Nodes.ContainsKey(c)) return (null, false);
				currentNode = currentNode.Nodes[c];
			}

			if (!currentNode.HasValue) return (null, false);
			return (currentNode, true);
		}

		public void Remove(string key)
		{
			var result = TryFindNode(key);
			if (!result.wasFound) throw new ArgumentException();
			result.node.HasValue = false;
		}
	}

	class TrieNode<TValue>
	{
		public Dictionary<char, TrieNode<TValue>> Nodes;
		private TValue value;
		public TValue Value
		{
			get => HasValue? value : throw new InvalidOperationException();
			set
			{
				HasValue = true;
				this.value = value;
			}
		}

		public bool HasValue { get; set; }

		public TrieNode()
		{
			HasValue = false;
			Nodes = new Dictionary<char, TrieNode<TValue>>();
		}
	}
}