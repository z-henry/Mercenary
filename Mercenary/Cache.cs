using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercenary
{
	public static class Cache
	{
		public static string lastOpponentFullName = "";//对手id
		public static HashSet<string> pvpMercTeam = new HashSet<string>();//佣兵阵容的集合
		public static int unlockMercID = -1;// 自动解锁装备-佣兵
		public static int unlockMapID = -1;// 自动解锁装备-地图
	}
	public static class Global
	{
		public static bool matchFirstRecord = true;
	}
}
