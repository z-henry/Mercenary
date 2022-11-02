using HsMercenaryStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mercenary
{
	public class StrategyHelper
	{
		static StrategyHelper()
		{
			Type[] array = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => from t in a.GetTypes()
																							  where t.GetInterfaces().Contains(typeof(IStrategy))
																							  select t).ToArray<Type>();
			for (int i = 0; i < array.Length; i++)
			{
				IStrategy strategy = (IStrategy)Activator.CreateInstance(array[i]);
				if (!StrategyHelper.StrategiesDict.ContainsKey(strategy.Name()))
				{
					StrategyHelper.StrategiesDict.Add(strategy.Name(), strategy);
				}
			}
		}

		public static List<string> GetAllStrategiesName()
		{
			return StrategyHelper.StrategiesDict.Keys.ToList<string>().FindAll((string i) => !i.Equals("_Sys_Default"));
		}

		public static IStrategy GetStrategy(string name)
		{
			return StrategyHelper.StrategiesDict[name];
		}

		private static readonly Dictionary<string, IStrategy> StrategiesDict = new Dictionary<string, IStrategy>();
	}
}