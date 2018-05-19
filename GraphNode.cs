using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures
{
	public class GraphNode<T>
	{
		public T Value { get; set; }
		public HashSet<GraphNode<T>> ConnectedNodes { get; }

		public GraphNode(T value)
		{
			Value = value;
			ConnectedNodes = new HashSet<GraphNode<T>>();
		}

		public void Connect(GraphNode<T> nodeToConnect)
		{
			ConnectedNodes.Add(nodeToConnect);
			nodeToConnect.ConnectedNodes.Add(this);
		}

		public override string ToString()
		{
			return Value.ToString();
		}


		/// <summary>
		/// Method uses BFS
		/// </summary>
		public (IEnumerable<GraphNode<T>> path, int distance) FindPathTo(GraphNode<T> nodeToFind)
		{
			var q = new Queue<GraphNode<T>>();
			var path = new Dictionary<GraphNode<T>, GraphNode<T>>();
			path.Add(this, null);
			q.Enqueue(this);
			while (q.Count > 0)
			{
				var parentNode = q.Dequeue();
				foreach (var node in parentNode.ConnectedNodes)
				{
					if (!path.ContainsKey(node))
					{
						q.Enqueue(node);
						path.Add(node, parentNode);
					}

					if (node == nodeToFind) break;
				}
			}

			if (!path.ContainsKey(nodeToFind)) throw new ArgumentException("Nodes are not connected");

			int distance = 0;
			var stack = new Stack<GraphNode<T>>();
			var prev = path[nodeToFind];
			while (prev != null)
			{
				stack.Push(prev);
				prev = path[prev];
				distance++;
			}

			return (stack.AsEnumerable(), distance);
		}

		/// <summary>
		/// Method uses BFS
		/// </summary>
		public IEnumerable<GraphNode<T>> GetAllNodes()
		{
			var visited = new HashSet<GraphNode<T>>();
			var q = new Queue<GraphNode<T>>();
			q.Enqueue(this);
			while (q.Count > 0)
			{
				var parentNode = q.Dequeue();
				foreach (var node in parentNode.ConnectedNodes)
				{
					if (visited.Contains(node)) continue;
					yield return node;
					visited.Add(node);
					q.Enqueue(node);
				}
			}
		}

		public IEnumerable<GraphNode<T>> FindCycle()
		{
			var s = new Stack<GraphNode<T>>();
			var path = new Dictionary<GraphNode<T>, GraphNode<T>> {{this, null}};
			var visited = new List<GraphNode<T>>();
			s.Push(this);
			while (s.Count > 0)
			{
				var parentNode = s.Pop();
				if (visited.Contains(parentNode)) return GetCyclePath(parentNode, path);
				foreach (var node in parentNode.ConnectedNodes)
				{
					if(node == parentNode) continue;
					s.Push(node);
					path[node] = parentNode;
				}
				visited.Add(parentNode);
			}
			return null;
		}

		private static IEnumerable<GraphNode<T>> GetCyclePath(GraphNode<T> parentNode, Dictionary<GraphNode<T>, GraphNode<T>> path)
		{
			var current = parentNode;
			var prev = path[parentNode];
			while (prev != parentNode)
			{
				yield return current;
				current = prev;
				prev = path[prev];
			}
		}
	}
}