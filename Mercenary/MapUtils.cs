using System.Collections.Generic;
using System.Linq;

namespace Mercenary
{
	public static class MapUtils
	{
		public static int GetMapId(string name)
		{
			Map map = MapUtils.GetMap(name);
			if (map != null)
			{
				return map.ID;
			}
			return -1;
		}

		public static Map GetMap(string name)
		{
			return MapUtils.MapConfList.Find((Map m) => m.Name.Equals(name));
		}

		public static Map GetMapByBoss(string boss)
		{
			return MapUtils.MapConfList.Find((Map m) => m.Boss.Equals(boss));
		}

		public static Map GetMapByID(int id)
		{
			return MapUtils.MapConfList.Find((Map m) => m.ID.Equals(id));
		}

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
			return MapUtils.GetMapId("1-1");
		}

		public static string[] GetMapNameList()
		{
			return (from m in MapUtils.MapConfList
					select m.Name).ToArray<string>();
		}

		private static readonly List<Map> MapConfList = new List<Map>
		{
			new Map(57, "1-1", "残暴的野猪人", typeof(DefaultTeam.Nature)),
			new Map(58, "1-2", "空气元素", typeof(DefaultTeam.Nature)),
			new Map(59, "1-3", "塞瑞娜·血羽", typeof(DefaultTeam.Nature)),
			new Map(60, "1-4", "药剂师赫布瑞姆", typeof(DefaultTeam.Nature)),
			new Map(61, "1-5", "烈日行者傲蹄", typeof(DefaultTeam.Nature)),
			new Map(62, "1-6", "巴拉克·科多班恩", typeof(DefaultTeam.Nature)),
			new Map(63, "1-7", "疯狂投弹者", typeof(DefaultTeam.Nature)),
			new Map(64, "1-8", "腐烂的普雷莫尔", typeof(DefaultTeam.Nature)),
			new Map(65, "1-9", "尼尔鲁·火刃", typeof(DefaultTeam.Nature)),
			new Map(67, "2-1", "猎手拉文", typeof(DefaultTeam.Nature)),
			new Map(68, "2-2", "淬油之刃", typeof(DefaultTeam.Nature)),
			new Map(69, "2-3", "堕落的守卫", typeof(DefaultTeam.Nature)),
			new Map(70, "2-4", "哈拉梵", typeof(DefaultTeam.Nature)),
			new Map(71, "2-5", "腐化的古树", typeof(DefaultTeam.Nature)),
			new Map(72, "2-6", "魔王贝恩霍勒", typeof(DefaultTeam.Nature)),
			new Map(242, "1-10", "奶牛王", typeof(DefaultTeam.Nature)),
			new Map(73, "3-1", "雪爪", typeof(DefaultTeam.Nature)),
			new Map(74, "3-2", "雪人猎手拉尼尔", typeof(DefaultTeam.Nature)),
			new Map(75, "3-3", "雪崩", typeof(DefaultTeam.Nature)),
			new Map(76, "3-4", "厄苏拉·风怒", typeof(DefaultTeam.Nature)),
			new Map(77, "3-5", "冰吼", typeof(DefaultTeam.Nature)),
			new Map(78, "3-6", "冰霜之王埃霍恩", typeof(DefaultTeam.Nature)),
			new Map(79, "4-1", "科林·烈酒", typeof(DefaultTeam.Nature)),
			new Map(80, "4-2", "裁决者格里斯通", typeof(DefaultTeam.Nature)),
			new Map(81, "4-3", "索瑞森大帝", typeof(DefaultTeam.Nature)),
			new Map(82, "4-4", "加尔", typeof(DefaultTeam.Nature)),
			new Map(83, "4-5", "迦顿男爵", typeof(DefaultTeam.Nature)),
			new Map(84, "4-6", "管理者埃克索图斯", typeof(DefaultTeam.Nature)),
			new Map(114, "4-7", "欧莫克大王", typeof(DefaultTeam.Nature)),
			new Map(116, "4-8", "达基萨斯将军", typeof(DefaultTeam.Nature)),
			new Map(118, "4-9", "雷德·黑手", typeof(DefaultTeam.Nature)),
			new Map(123, "4-10", "拉佐格尔", typeof(DefaultTeam.Nature)),
			new Map(121, "4-11", "瓦拉斯塔兹", typeof(DefaultTeam.Nature)),
			new Map(122, "4-12", "克洛玛古斯", typeof(DefaultTeam.Nature)),
			new Map(120, "4-13", "奈法利安", typeof(DefaultTeam.FireKill)),
			new Map(135, "5-1", "拉瓦克·恐怖图腾", typeof(DefaultTeam.Nature)),
			new Map(129, "5-2", "刘易斯·菲利普", typeof(DefaultTeam.Nature)),
			new Map(131, "5-3", "加尔范上尉", typeof(DefaultTeam.Nature)),
			new Map(134, "5-4", "冷饮制冰机", typeof(DefaultTeam.Nature)),
			new Map(128, "5-5", "冰雪之王洛克霍拉", typeof(DefaultTeam.Nature)),
			new Map(132, "5-6", "德雷克塔尔", typeof(DefaultTeam.Nature)),
			new Map(145, "5-7", "洛泰姆中尉", typeof(DefaultTeam.Nature)),
			new Map(144, "5-8", "艾克曼", typeof(DefaultTeam.Nature)),
			new Map(146, "5-9", "巴琳达·斯通赫尔斯", typeof(DefaultTeam.Nature)),
			new Map(141, "5-10", "森林之王伊弗斯", typeof(DefaultTeam.Nature)),
			new Map(140, "5-11", "范达尔·雷矛", typeof(DefaultTeam.Nature)),
			new Map(143, "5-12", "卡扎库斯"),//魔像师卡扎库斯->卡扎库斯
			new Map(154, "6-1", "燃翼"),
			new Map(155, "6-2", "龙骨魔像"),
			new Map(158, "6-3", "军情七处走私贩"),
			new Map(156, "6-4", "奥妮克希亚", typeof(DefaultTeam.Nature)),
			new Map(161, "6-5", "亡灵奥妮克希亚"),
			new Map(153, "6-6", "米达，纯粹虚空", typeof(DefaultTeam.Nature)),
			new Map(168, "7-1", "已腐蚀的鱼人"),//老巨鳍->已腐蚀的鱼人
			new Map(183, "7-2", "深渊女妖"),
			new Map(172, "7-3", "席弗尔斯船长"),
			new Map(169, "7-4", "恩佐斯侍战者"),
			new Map(179, "8-1", "克雷什，群龟之王"),
			new Map(178, "8-2", "珊瑚元素"),
			new Map(180, "8-3", "恩佐斯的鱼"),
			new Map(212, "8-4", "艾萨拉女王"),
			new Map(184, "8-5", "恩佐斯"),
			new Map(210, "8-6", "祝踏岚"),
			new Map(197, "8-7", "毒心者夏克里尔"),
			new Map(193, "8-8", "女皇夏柯扎拉"),
			new Map(196, "8-9", "加尔鲁什·地狱咆哮"),
			new Map(198, "8-10", "亚煞极"),
			new Map(204, "9-1", "马戏领班威特利"),
			new Map(217, "9-2", "暗月先知塞格"),
			new Map(201, "9-3", "暗月兔子"),
			new Map(219, "9-4", "变装大师"),
			new Map(214, "9-5", "希拉斯·暗月"),
			new Map(220, "9-6", "尤格-萨隆"),
			new Map(267, "9-7", "克苏恩"),
			new Map(257, "2-7", "猎手阿图门"),
			new Map(222, "9-8", "呓语魔典"),
			new Map(255, "2-8", "莫罗斯"),
			new Map(231, "9-9", "歌剧院"),
			new Map(244, "9-10", "巴内斯"),
			new Map(259, "4-14", "夜之魇", typeof(DefaultTeam.PirateSnake)),
			new Map(237, "4-15", "馆长"),
			new Map(263, "2-9", "埃兰之影"),
			new Map(251, "4-16", "虚空幽龙"),
			new Map(253, "9-11", "象棋"),//白棋国王->象棋
			new Map(265, "2-10", "玛克扎尔王子"),
			new Map(226, "2-11", "麦迪文的残影", typeof(DefaultTeam.Nature)),
			new Map(85, "H1-1", ""),
			new Map(86, "H1-2", ""),
			new Map(87, "H1-3", ""),
			new Map(88, "H1-4", ""),
			new Map(89, "H1-5", ""),
			new Map(90, "H1-6", ""),
			new Map(91, "H1-7", ""),
			new Map(92, "H1-8", ""),
			new Map(93, "H1-9", ""),
			new Map(243, "H1-10", "", typeof(DefaultTeam.Nature)),
			new Map(94, "H2-1", ""),
			new Map(95, "H2-2", ""),
			new Map(96, "H2-3", ""),
			new Map(97, "H2-4", ""),
			new Map(98, "H2-5", ""),
			new Map(99, "H2-6", ""),
			new Map(258, "H2-7", ""),
			new Map(256, "H2-8", ""),
			new Map(264, "H2-9", ""),
			new Map(266, "H2-10", ""),
			new Map(227, "H2-11", "", typeof(DefaultTeam.Nature)),
			new Map(100, "H3-1", ""),
			new Map(101, "H3-2", ""),
			new Map(102, "H3-3", ""),
			new Map(103, "H3-4", ""),
			new Map(104, "H3-5", ""),
			new Map(105, "H3-6", ""),
			new Map(106, "H4-1", ""),
			new Map(107, "H4-2", "", typeof(DefaultTeam.Nature)),
			new Map(108, "H4-3", "", typeof(DefaultTeam.Nature)),
			new Map(109, "H4-4", ""),
			new Map(110, "H4-5", ""),
			new Map(111, "H4-6", ""),
			new Map(115, "H4-7", ""),
			new Map(117, "H4-8", ""),
			new Map(119, "H4-9", ""),
			new Map(124, "H4-10", ""),
			new Map(126, "H4-11", "", typeof(DefaultTeam.Nature)),
			new Map(125, "H4-12", ""),
			new Map(127, "H4-13", ""),
			new Map(260, "H4-14", "", typeof(DefaultTeam.PirateSnake)),
			new Map(238, "H4-15", ""),
			new Map(252, "H4-16", ""),
			new Map(139, "H5-1", ""),
			new Map(130, "H5-2", ""),
			new Map(137, "H5-3", ""),
			new Map(138, "H5-4", ""),
			new Map(136, "H5-5", ""),
			new Map(133, "H5-6", ""),
			new Map(147, "H5-7", ""),
			new Map(149, "H5-8", "", typeof(DefaultTeam.Nature)),
			new Map(150, "H5-9", "", typeof(DefaultTeam.Nature)),
			new Map(151, "H5-10", "", typeof(DefaultTeam.Nature)),
			new Map(152, "H5-11", ""),
			new Map(148, "H5-12", ""),
			new Map(165, "H6-1", ""),
			new Map(160, "H6-2", ""),
			new Map(159, "H6-3", ""),
			new Map(163, "H6-4", "", typeof(DefaultTeam.FireKill)),
			new Map(162, "H6-5", ""),
			new Map(164, "H6-6", "", typeof(DefaultTeam.Nature)),
			new Map(173, "H7-1", ""),
			new Map(187, "H7-2", ""),
			new Map(174, "H7-3", ""),
			new Map(176, "H7-4", ""),
			new Map(190, "H8-1", ""),
			new Map(181, "H8-2", ""),
			new Map(182, "H8-3", ""),
			new Map(213, "H8-4", ""),
			new Map(205, "H8-5", ""),
			new Map(211, "H8-6", ""),
			new Map(199, "H8-7", ""),
			new Map(194, "H8-8", ""),
			new Map(216, "H8-9", ""),
			new Map(200, "H8-10", ""),
			new Map(206, "H9-1", ""),
			new Map(218, "H9-2", ""),
			new Map(202, "H9-3", ""),
			new Map(230, "H9-4", ""),
			new Map(215, "H9-5", ""),
			new Map(221, "H9-6", "", typeof(DefaultTeam.PirateSnake)),
			new Map(268, "H9-7", "", typeof(DefaultTeam.Ice)),
			new Map(223, "H9-8", ""),
			new Map(233, "H9-9", ""),
			new Map(250, "H9-10", ""),
			new Map(254, "H9-11", "")
		};

		private static int lastTryIndex = 0;
	}
}