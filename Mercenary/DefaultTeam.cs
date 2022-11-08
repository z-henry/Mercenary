using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mercenary.DefaultTeam
{
	public class TeamManager
	{
		static TeamManager()
		{
			DirectoryInfo rootStrategy = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx/plugins/strategy"));
			List<FileInfo> testList = rootStrategy.GetFiles("*.dll", SearchOption.TopDirectoryOnly).ToList();
			foreach (var file in testList)
			{
				Assembly ass = Assembly.LoadFile(file.FullName);
				foreach (var type_iter in ass.GetTypes())
				{
					if (type_iter.GetInterfaces().Contains(typeof(ITeam)) &&
						false == type_iter.Name.Contains(nameof(ITeam)))
					{
						ITeam team = (ITeam)Activator.CreateInstance(type_iter);
						if (!TeamManager.m_teamDict.ContainsKey(team.GetType()))
						{
							TeamManager.m_teamDict.Add(team.GetType(), team);
						}
					}
				}
			}

			foreach (var type_iter in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (type_iter.GetInterfaces().Contains(typeof(ITeam)) &&
					false == type_iter.Name.Contains(nameof(ITeam)))
				{
					ITeam team = (ITeam)Activator.CreateInstance(type_iter);
					if (!TeamManager.m_teamDict.ContainsKey(team.GetType()))
					{
						TeamManager.m_teamDict.Add(team.GetType(), team);
					}
				}
			}
			foreach (var iter in m_teamDict)
			{
				Console.WriteLine($"{iter.Key}");
				Out.Log($"[registered_team] {iter.Key}");
			}
		}

		public static ITeam GetTeam(Type teamtype)
		{
			return m_teamDict[teamtype];
		}

		private static Dictionary<Type, ITeam> m_teamDict = new Dictionary<Type, ITeam>();
	}

	public interface ITeam
	{
		List<(int id, int equipIndex)> GetMember();
	}

	public class Ice : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.巴琳达_斯通赫尔斯, 1),
				(MercConst.瓦尔登_晨拥, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.吉安娜_普罗德摩尔, 2),
				(MercConst.厨师曲奇, 2),
				(MercConst.魔像师卡扎库斯, 2)
			};
		}
	}

	public class IceFire : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
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

	public class FireKill : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
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

	public class PirateSnake : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
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

	public class Nature : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.奈姆希_灵沼, 0),
				(MercConst.玛法里奥_怒风, 2),
				(MercConst.布鲁坎, 0),
				(MercConst.安娜科德拉, 0),
				(MercConst.厨师曲奇, 2),
				(MercConst.冰雪之王洛克霍拉, 0),
			};
		}
	}

	public class Origin0 : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.泽瑞拉, 0),
				(MercConst.泰兰德_语风, 1),
				(MercConst.凯瑞尔_罗姆, 0),
			};
		}
	}

	public class Origin : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.泽瑞拉, 0),
				(MercConst.泰兰德_语风, 1),
				(MercConst.凯瑞尔_罗姆, 0),
				(MercConst.剑圣萨穆罗, 2),
			};
		}
	}

	public class AOE : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.米尔豪斯_法力风暴, 0),
				(MercConst.冰雪之王洛克霍拉, 0),
				(MercConst.瓦尔登_晨拥, 2),
			};
		}
	}

	public class PrimaryFire : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.安东尼达斯, 0),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
			};
		}
	}

	public class DruidsExclusive : ITeam
	{
		public List<(int id, int equipIndex)> GetMember()
		{
			return new List<(int id, int equipIndex)>()
			{
				(MercConst.安东尼达斯, 0),
				(MercConst.迦顿男爵, 1),
				(MercConst.拉格纳罗斯, 2),
			};
		}
	}
}