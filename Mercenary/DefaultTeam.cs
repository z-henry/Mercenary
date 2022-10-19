using System;
using System.Collections.Generic;

namespace Mercenary.DefaultTeam
{
	public class TeamType
	{
		public TeamType(List<(int id, int equipIndex)> teaminfo)
		{
			TeamInfo = teaminfo;
		}
		public static void Register(Type type, TeamType teamType)
		{
			_types.Add(type, teamType);
		}

		public static TeamType Get(Type type)
		{
			return _types[type];
		}
		public static List<TeamType> GetAllTeamType()
		{
			List<TeamType> result = new List<TeamType>();
			foreach (TeamType iter in _types.Values)
				result.Add(iter);
			return result;
		}

		public List<(int id, int equipIndex)> TeamInfo { get; private set; }

		private static IDictionary<Type, TeamType> _types = new Dictionary<Type, TeamType>();

	}
	public abstract class TeamBase
	{
		public abstract TeamType TeamType { get; }

	}
	public class Ice : TeamBase
	{
		static Ice() { TeamType.Register(typeof(Ice), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.巴琳达_斯通赫尔斯, 1),
				(MercConst.瓦尔登_晨拥, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.吉安娜_普罗德摩尔, 2),
				(MercConst.厨师曲奇, 2),
				(MercConst.魔像师卡扎库斯, 2)
			});
	}

	public class IceFire : TeamBase
	{
		static IceFire() { TeamType.Register(typeof(IceFire), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.巴琳达_斯通赫尔斯, 1),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
				(MercConst.瓦尔登_晨拥, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.吉安娜_普罗德摩尔, 2),
			});
	}

	public class FireKill : TeamBase
	{
		static FireKill() { TeamType.Register(typeof(FireKill), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.巴琳达_斯通赫尔斯, 1),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
				(MercConst.瓦莉拉_萨古纳尔, 2),
				(MercConst.魔像师卡扎库斯, 2),
				(MercConst.变装大师, 1)
			});
	}
	public class PirateSnake : TeamBase
	{
		static PirateSnake() { TeamType.Register(typeof(PirateSnake), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.海盗帕奇斯, 1),
				(MercConst.鞭笞者特里高雷, 1),
				(MercConst.尤朵拉, 1),
				(MercConst.瓦莉拉_萨古纳尔, 2),
				(MercConst.变装大师, 1),
				(MercConst.重拳先生, 1)
			});
	}
	public class Nature : TeamBase
	{
		static Nature() { TeamType.Register(typeof(Nature), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.奈姆希_灵沼, 0),
				(MercConst.玛法里奥_怒风, 2),
				(MercConst.布鲁坎, 0),
				(MercConst.安娜科德拉, 0),
				(MercConst.厨师曲奇, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
			});
	}
	public class Origin0 : TeamBase
	{
		static Origin0() { TeamType.Register(typeof(Origin0), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.泽瑞拉, 0),
				(MercConst.泰兰德_语风, 1),
				(MercConst.凯瑞尔_罗姆, 0),
			});
	}
	public class Origin : TeamBase
	{
		static Origin() { TeamType.Register(typeof(Origin), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.泽瑞拉, 0),
				(MercConst.泰兰德_语风, 1),
				(MercConst.凯瑞尔_罗姆, 0),
				(MercConst.剑圣萨穆罗, 2),
			});
	}
	public class AOE : TeamBase
	{
		static AOE() { TeamType.Register(typeof(AOE), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.瓦尔登_晨拥, 2),
			});
	}
	public class PrimaryFire : TeamBase
	{
		static PrimaryFire() { TeamType.Register(typeof(PrimaryFire), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.安东尼达斯, 0),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
			});
	}
	public class DruidsExclusive : TeamBase
	{
		static DruidsExclusive() { TeamType.Register(typeof(DruidsExclusive), Type); }
		override public TeamType TeamType { get { return Type; } }

		public static readonly TeamType Type = new TeamType(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.玛法里奥_怒风, 2),
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.瓦尔登_晨拥, 2),
			});
	}

}
