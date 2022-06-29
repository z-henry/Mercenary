using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HsMercenaryStrategy;

namespace Mercenary
{
	// Token: 0x02000008 RID: 8
	public class StrategyHelper
	{
		// Token: 0x06000043 RID: 67 RVA: 0x000045A8 File Offset: 0x000027A8
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

		// Token: 0x06000044 RID: 68 RVA: 0x00004623 File Offset: 0x00002823
		public static List<string> GetAllStrategiesName()
		{
			return StrategyHelper.StrategiesDict.Keys.ToList<string>().FindAll((string i) => !i.Equals("_Sys_Default"));
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004658 File Offset: 0x00002858
		public static IStrategy GetStrategy(string name)
		{
			return StrategyHelper.StrategiesDict[name];
		}

		// Token: 0x0400002C RID: 44
		private static readonly Dictionary<string, IStrategy> StrategiesDict = new Dictionary<string, IStrategy>();
	}
}
