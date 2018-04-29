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
			var heap = new BinaryHeap<int>(new []{1,2,3,4,5,6,7,8,9,10,11});
			Console.WriteLine(heap.ToString());
		}
	}
}
