using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalUtilities.Utilities;

namespace Structures
{
	static class GraphUtils
	{
		public static int FindSubgraphsCount<T>(List<GraphNode<T>> nodeList)
		{
			var nodesToCheck = nodeList.Select(x => x).ToList();
			var count = 0;
			while (nodesToCheck.Any())
			{
				var node = nodesToCheck.First();
				nodesToCheck.Remove(node);
				foreach (var n in node.GetAllNodes())
				{
					if (nodesToCheck.Contains(n)) nodesToCheck.Remove(n);
				}
				count++;
			}

			return count;
		}
	}
}
