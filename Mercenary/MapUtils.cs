using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercenary
{
	// Token: 0x0200000B RID: 11
	public static class MapUtils
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00005140 File Offset: 0x00003340
		public static int GetMapId(string name)
		{
			Map map = MapUtils.GetMap(name);
			if (map != null)
			{
				return map.ID;
			}
			return -1;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00005160 File Offset: 0x00003360
		public static Map GetMap(string name)
		{
			return MapUtils.MapConfList.Find((Map m) => m.Name.Equals(name));
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005190 File Offset: 0x00003390
		public static int GetUnCompleteMap()
		{
			for (int i = MapUtils.lastTryIndex; i < MapUtils.MapConfList.Count; i++)
			{
				if (!MercenariesDataUtil.IsBountyComplete(MapUtils.MapConfList[i].ID))
				{
					MapUtils.lastTryIndex = i;
					return MapUtils.MapConfList[i].ID;
				}
			}
			return MapUtils.GetMapId("H1-1");
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000051EE File Offset: 0x000033EE
		public static string[] GetMapNameList()
		{
			return (from m in MapUtils.MapConfList
					select m.Name).ToArray<string>();
		}

		// Token: 0x04000036 RID: 54
		private static readonly List<Map> MapConfList = new List<Map>
		{
			new Map(57, "1-1"),
			new Map(58, "1-2"),
			new Map(59, "1-3"),
			new Map(60, "1-4"),
			new Map(61, "1-5"),
			new Map(62, "1-6"),
			new Map(63, "1-7"),
			new Map(64, "1-8"),
			new Map(65, "1-9"),
			new Map(67, "2-1"),
			new Map(68, "2-2"),
			new Map(69, "2-3"),
			new Map(70, "2-4"),
			new Map(71, "2-5"),
			new Map(72, "2-6"),
			new Map(242, "1-10"),
			new Map(73, "3-1"),
			new Map(74, "3-2"),
			new Map(75, "3-3"),
			new Map(76, "3-4"),
			new Map(77, "3-5"),
			new Map(78, "3-6"),
			new Map(79, "4-1"),
			new Map(80, "4-2"),
			new Map(81, "4-3"),
			new Map(82, "4-4"),
			new Map(83, "4-5"),
			new Map(84, "4-6"),
			new Map(114, "4-7"),
			new Map(116, "4-8"),
			new Map(118, "4-9"),
			new Map(123, "4-10"),
			new Map(121, "4-11"),
			new Map(122, "4-12"),
			new Map(120, "4-13"),
			new Map(135, "5-1"),
			new Map(129, "5-2"),
			new Map(131, "5-3"),
			new Map(134, "5-4"),
			new Map(128, "5-5"),
			new Map(132, "5-6"),
			new Map(145, "5-7"),
			new Map(144, "5-8"),
			new Map(146, "5-9"),
			new Map(141, "5-10"),
			new Map(140, "5-11"),
			new Map(143, "5-12"),
			new Map(154, "6-1"),
			new Map(155, "6-2"),
			new Map(158, "6-3"),
			new Map(156, "6-4"),
			new Map(161, "6-5"),
			new Map(153, "6-6"),
			new Map(168, "7-1"),
			new Map(183, "7-2"),
			new Map(172, "7-3"),
			new Map(169, "7-4"),
			new Map(179, "8-1"),
			new Map(178, "8-2"),
			new Map(180, "8-3"),
			new Map(212, "8-4"),
			new Map(184, "8-5"),
			new Map(210, "8-6"),
			new Map(197, "8-7"),
			new Map(193, "8-8"),
			new Map(196, "8-9"),
			new Map(198, "8-10"),
			new Map(85, "H1-1"),
			new Map(86, "H1-2"),
			new Map(87, "H1-3"),
			new Map(88, "H1-4"),
			new Map(89, "H1-5"),
			new Map(90, "H1-6"),
			new Map(91, "H1-7"),
			new Map(92, "H1-8"),
			new Map(93, "H1-9"),
			new Map(243, "H1-10"),
			new Map(94, "H2-1"),
			new Map(95, "H2-2"),
			new Map(96, "H2-3"),
			new Map(97, "H2-4"),
			new Map(98, "H2-5"),
			new Map(99, "H2-6"),
			new Map(100, "H3-1"),
			new Map(101, "H3-2"),
			new Map(102, "H3-3"),
			new Map(103, "H3-4"),
			new Map(104, "H3-5"),
			new Map(105, "H3-6"),
			new Map(106, "H4-1"),
			new Map(107, "H4-2"),
			new Map(108, "H4-3"),
			new Map(109, "H4-4"),
			new Map(110, "H4-5"),
			new Map(111, "H4-6"),
			new Map(115, "H4-7"),
			new Map(117, "H4-8"),
			new Map(119, "H4-9"),
			new Map(124, "H4-10"),
			new Map(126, "H4-11"),
			new Map(125, "H4-12"),
			new Map(127, "H4-13"),
			new Map(129, "H5-1"),
			new Map(130, "H5-2"),
			new Map(137, "H5-3"),
			new Map(138, "H5-4"),
			new Map(136, "H5-5"),
			new Map(133, "H5-6"),
			new Map(147, "H5-7"),
			new Map(149, "H5-8"),
			new Map(150, "H5-9"),
			new Map(151, "H5-10"),
			new Map(152, "H5-11"),
			new Map(148, "H5-12"),
			new Map(165, "H6-1"),
			new Map(160, "H6-2"),
			new Map(159, "H6-3"),
			new Map(163, "H6-4"),
			new Map(162, "H6-5"),
			new Map(164, "H6-6"),
			new Map(173, "H7-1"),
			new Map(187, "H7-2"),
			new Map(174, "H7-3"),
			new Map(176, "H7-4"),
			new Map(190, "H8-1"),
			new Map(181, "H8-2"),
			new Map(182, "H8-3"),
			new Map(213, "H8-4"),
			new Map(205, "H8-5"),
			new Map(211, "H8-6"),
			new Map(199, "H8-7"),
			new Map(194, "H8-8"),
			new Map(216, "H8-9"),
			new Map(200, "H8-10")
		};

		// Token: 0x04000037 RID: 55
		private static int lastTryIndex;
	}
}
