using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures
{
	class Program
	{
		static void Main(string[] args)
		{
			var n1 = new GraphNode<int>(1);
			var n2 = new GraphNode<int>(2);
			var n20 = new GraphNode<int>(20);
			var n30 = new GraphNode<int>(200);
			var n3 = new GraphNode<int>(3);
			var wut = new GraphNode<int>(-1);
			var wuuut = new GraphNode<int>(-2);
			n1.Connect(n2);
			n2.Connect(n20);
			n20.Connect(n30);
			n3.Connect(n2);
			n3.Connect(n1);
			var a = n1.FindPathTo(n30);
			var b = new List<GraphNode<int>> {n1, n2, n20, n30, n3, wut, wuuut};
			var c = GraphUtils.FindSubgraphsCount(b);
			var d = n1.FindCycle();
			{
			}
		}
	}
}