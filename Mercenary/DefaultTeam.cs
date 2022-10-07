using System.Collections.Generic;

namespace Mercenary.DefaultTeam
{
	public interface IDefaultTeam
	{
		List<(int id, int equipIndex)> GetTeamInfo();
	}
	public class TeamSnow : IDefaultTeam
	{
		public List<(int id, int equipIndex)> GetTeamInfo()
		{
			return new List<(int id, int equipIndex)>() {
				(MercConst.巴琳达_斯通赫尔斯, 1),
				(MercConst.瓦尔登_晨拥, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.吉安娜_普罗德摩尔, 2),
				(MercConst.厨师曲奇, 2),
				(MercConst.魔像师卡扎库斯, 2)
			};
		}
	}
	public class TeamIceFire : IDefaultTeam
	{
		public List<(int id, int equipIndex)> GetTeamInfo()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.巴琳达_斯通赫尔斯, 1),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
				(MercConst.瓦尔登_晨拥, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.吉安娜_普罗德摩尔, 2),
			};
		}
	}
	public class TeamFireKill : IDefaultTeam
	{
		public List<(int id, int equipIndex)> GetTeamInfo()
		{
			return new List<(int id, int equipIndex)>()

			{
				(MercConst.巴琳达_斯通赫尔斯, 1),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
				(MercConst.瓦莉拉_萨古纳尔, 2),
				(MercConst.魔像师卡扎库斯, 2),
				(MercConst.变装大师, 1)
			};
		}
	}
	public class TeamPirateSnake : IDefaultTeam
	{
		public List<(int id, int equipIndex)> GetTeamInfo()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.海盗帕奇斯, 1),
				(MercConst.鞭笞者特里高雷, 1),
				(MercConst.尤朵拉, 1),
				(MercConst.瓦莉拉_萨古纳尔, 2),
				(MercConst.变装大师, 1),
				(MercConst.重拳先生, 1)
			};
		}
	}
	public class TeamNature : IDefaultTeam
	{
		public List<(int id, int equipIndex)> GetTeamInfo()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.玛法里奥_怒风, 2),
				(MercConst.古夫_符文图腾, 2),
				(MercConst.布鲁坎, 0),
				(MercConst.安娜科德拉, 0),
				(MercConst.厨师曲奇, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
			};
		}
	}
}
