using Mercenary.MyInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mercenary.Strategy
{
	public class StrategyHelper
	{
		static StrategyHelper()
		{
			DirectoryInfo rootStrategy = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx/plugins/strategy")); 
			List<FileInfo> testList = rootStrategy.GetFiles("*.dll", SearchOption.TopDirectoryOnly).ToList();
			foreach (var file in testList)
			{
				Assembly ass = Assembly.LoadFile(file.FullName);
				Console.WriteLine($"{ass.GetName().Name} {ass.GetName().Version}");
				if (ass.GetName().Version < m_versionLimit)
					continue;

				foreach (var type_iter in ass.GetTypes())
				{
					if (type_iter.GetInterfaces().Contains(typeof(IStrategy)) &&
						false == type_iter.Name.Contains(nameof(IStrategy)))
					{
						IStrategy strategy = (IStrategy)Activator.CreateInstance(type_iter);
						if (!StrategyHelper.m_strategiesDict.ContainsKey(strategy.Name))
						{
							StrategyHelper.m_strategiesDict.Add(strategy.Name, strategy);
						}
					}
				}
			}



			foreach (var type_iter in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (type_iter.GetInterfaces().Contains(typeof(IStrategy)) &&
					false == type_iter.Name.Contains(nameof(IStrategy)))
				{
					IStrategy strategy = (IStrategy)Activator.CreateInstance(type_iter);
					if (!StrategyHelper.m_strategiesDict.ContainsKey(strategy.Name))
					{
						StrategyHelper.m_strategiesDict.Add(strategy.Name, strategy);
					}
				}
			}


			foreach (var iter in m_strategiesDict)
			{
				Console.WriteLine($"{iter.Key}");
				Out.Log($"[registered_strategy] {iter.Key}");
			}
		}

		public static List<string> GetAllStrategiesName()
		{
			return StrategyHelper.m_strategiesDict.Keys.ToList<string>().FindAll((string i) => i != "佣兵任务策略" && false == i.Contains("活动_") && false == i.Contains("DefaultStrategy_"));
		}

		public static IStrategy GetStrategy(string name)
		{
			if (StrategyHelper.m_strategiesDict.ContainsKey(name))
				return StrategyHelper.m_strategiesDict[name];
			else
				return null;
		}

		private static readonly Dictionary<string, IStrategy> m_strategiesDict = new Dictionary<string, IStrategy>();
		private static readonly Version m_versionLimit = new Version("4.2.0.0");
	}
}