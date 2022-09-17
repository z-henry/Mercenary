using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercenary
{
	public static class Cache
	{
		public static string CacheLastOpponentFullName = "";
		public static HashSet<string> pvpMercTeam = new HashSet<string>();//佣兵阵容的集合
	}
}
