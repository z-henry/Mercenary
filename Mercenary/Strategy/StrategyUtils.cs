using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercenary.Strategy
{

	public static class StrategyUtils
	{

		public static Target FindMercenaryForName(string name, List<Target> mercenaries)
		{
			return mercenaries.Find((Target m) => m.Name == name);
		}
		public static Target FindMercenaryForNameInclude(string name, List<Target> mercenaries)
		{
			return mercenaries.Find((Target m) => m.Name.Contains(name));
		}

		public static int FindMercenaryIndexForName(string name, List<Target> mercenaries)
		{
			return mercenaries.FindIndex((Target m) => m.Name == name);
		}


		public static Skill FindSkillForName(string name, Target mercenary)
		{
			return mercenary.Skills.Find((Skill s) => s.Name == name);
		}


		public static Target FindSlowestTarget(List<Target> targetEntities)
		{
			if (targetEntities.Count > 0)
			{
				int speed = targetEntities.Max((Target i) => i.Speed);
				return targetEntities.Find((Target i) => i.Speed == speed);
			}
			return null;
		}


		public static Target FindMaxHealthTarget(List<Target> targetEntities)
		{
			if (targetEntities.Count > 0)
			{
				int health = targetEntities.Max((Target i) => i.Health);
				return targetEntities.Find((Target i) => i.Health == health);
			}
			return null;
		}


		public static Target FindMinHealthTarget(List<Target> targetEntities, MY_TAG_ROLE? role = null)
		{
			if (targetEntities.Count <= 0)
				return null;

			int health_global = Enumerable.Min<Target>(targetEntities, (Target i) => i.Health);
			var target_global = targetEntities.Find((Target i) => i.Health == health_global);
			if (role == null)
				return target_global;

			var targetEntities_Spec = targetEntities.FindAll((Target i) => i.Role == role);
			if (targetEntities_Spec.Count <= 0)
				return target_global;

			int health_spec = Enumerable.Min<Target>(targetEntities_Spec, (Target i) => i.Health);
			var target_spec = targetEntities_Spec.Find((Target i) => i.Health == health_spec);
			return Math.Floor((double)health_spec / 2) <= health_global ? target_spec : target_global;
		}

		public static Target FindMaxLossHealthTarget(List<Target> targetEntities)
		{
			if (targetEntities.Count > 0)
			{
				int losshealth = Enumerable.Max<Target>(targetEntities, (Target i) => i.DefHealth - i.Health);
				if (losshealth > 0)
					return targetEntities.Find((Target i) => i.DefHealth - i.Health == losshealth);
			}
			return null;
		}
		public static Target FindSpecNameTarget(string name, List<Target> targetEntities)
		{
			if (targetEntities.Count > 0)
			{
				return targetEntities.Find((Target i) => i.Name == name);
			}
			return null;
		}
		public static Target GetTarget(List<Target> targetEntities)
		{
			foreach (var target in targetEntities)
			{
				if (target == null)
					continue;

				if (target.Enable == false)
					continue;

				return target;
			}
			return null;
		}
		public static MY_TAG_ROLE restrain_TAG_ROLE(MY_TAG_ROLE attack)
		{
			if (attack == MY_TAG_ROLE.CASTER)
				return MY_TAG_ROLE.TANK;
			else if (attack == MY_TAG_ROLE.TANK)
				return MY_TAG_ROLE.FIGHTER;
			else 
				return MY_TAG_ROLE.CASTER;
		}

		public static int Relevance_Shadow(List<Target> targetEntities)
		{
			string[] shadowteam =
			{
				"魔像师卡扎库斯","沃金","艾萨拉女王","亚煞极"
			};
			int relevance = 0;
			foreach (var iter in targetEntities)
			{
				if (Array.IndexOf(shadowteam, iter.Name) != -1)
					relevance++;
			}
			return relevance;
		}
		public static int Relevance_ElderGods(List<Target> targetEntities)
		{
			string[] shadowteam =
			{
				"恩佐斯","尤格-萨隆","亚煞极","克苏恩","变装大师"
			};
			int relevance = 0;
			foreach (var iter in targetEntities)
			{
				if (Array.IndexOf(shadowteam, iter.Name) != -1)
					relevance++;
			}
			return relevance;
		}

		public static int Relevance_Pirate(List<Target> targetEntities)
		{
			string[] array = new string[]
			{
				"鞭笞者特里高雷","尤朵拉","伊莉斯·逐星"
			};
			int relevance = 0;
			foreach (var iter in targetEntities)
			{
				if (Array.IndexOf(array, iter.Name) != -1)
					relevance++;
			}
			return relevance;
		}
		public static int Relevance_SnowKing(List<Target> targetEntities)
		{
			string[] array = new string[]
			{
				"冰雪之王洛克霍拉","吉安娜·普罗德摩尔","瓦尔登·晨拥"
			};
			int relevance = 0;
			foreach (var iter in targetEntities)
			{
				if (Array.IndexOf(array, iter.Name) != -1)
					relevance++;
			}
			return relevance;
		}
		public static int Relevance_Snake(List<Target> targetEntities)
		{
			string[] array = new string[]
			{
				"安娜科德拉","游学者周卓","玉珑"
			};
			int relevance = 0;
			foreach (var iter in targetEntities)
			{
				if (Array.IndexOf(array, iter.Name) != -1)
					relevance++;
			}
			return relevance;
		}
		public static int Relevance_NagaAdvanture(List<Target> targetEntities)
		{
			string[] array = new string[]
			{
				"伊莉斯·逐星","艾萨拉女王","雷诺·杰克逊"
			};
			int relevance = 0;
			foreach (var iter in targetEntities)
			{
				if (Array.IndexOf(array, iter.Name) != -1)
					relevance++;
			}
			return relevance;
		}

	}
}
