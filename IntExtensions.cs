using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures
{
	public static class IntExtensions
	{
		public static int Abs(this int number) => number >= 0 ? number : -number;
	}
}
