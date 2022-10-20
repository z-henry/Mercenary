using System;
using System.Collections.Generic;

namespace Mercenary.DefaultTeam
{

	public class TeamUnit
	{
		public TeamUnit(List<(int id, int equipIndex)> teaminfo)
		{
			TeamInfo = teaminfo;
		}
		public static void Register(Type type, TeamUnit teamType)
		{
			_types.Add(type, teamType);
		}

		public static TeamUnit Get(Type type)
		{
			return _types[type];
		}

		public List<(int id, int equipIndex)> TeamInfo { get; private set; }// 队伍列表

		private static IDictionary<Type, TeamUnit> _types = new Dictionary<Type, TeamUnit>();

	}
	public abstract class TeamBase
	{
		public abstract TeamUnit Unit { get; }

	}
	public class Ice : TeamBase
	{
		static Ice() { TeamUnit.Register(typeof(Ice), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
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
		static IceFire() { TeamUnit.Register(typeof(IceFire), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
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
		static FireKill() { TeamUnit.Register(typeof(FireKill), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
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
		static PirateSnake() { TeamUnit.Register(typeof(PirateSnake), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
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
		static Nature() { TeamUnit.Register(typeof(Nature), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
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
		static Origin0() { TeamUnit.Register(typeof(Origin0), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
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
		static Origin() { TeamUnit.Register(typeof(Origin), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
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
		static AOE() { TeamUnit.Register(typeof(AOE), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.瓦尔登_晨拥, 2),
			});
	}
	public class PrimaryFire : TeamBase
	{
		static PrimaryFire() { TeamUnit.Register(typeof(PrimaryFire), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.安东尼达斯, 0),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
			});
	}
	public class DruidsExclusive : TeamBase
	{
		static DruidsExclusive() { TeamUnit.Register(typeof(DruidsExclusive), Type); }
		override public TeamUnit Unit { get { return Type; } }

		public static readonly TeamUnit Type = new TeamUnit(
			new List<(int id, int equipIndex)>()
			{
				(MercConst.玛法里奥_怒风, 2),
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.瓦尔登_晨拥, 2),
			});
	}
}
