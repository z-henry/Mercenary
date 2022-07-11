using System;
using System.Collections.Generic;
using HsMercenaryStrategy;

namespace Mercenary
{
	
	public class DefaultStrategy : IStrategy
	{
		
		public List<BattleTarget> GetBattleTargets(List<HsMercenaryStrategy.Mercenary> mercenaries, List<Target> targets_opposite, List<Target> targets_friendly)
		{
// 			Out.Log("default" + targets_opposite.Count.ToString());
			List<BattleTarget> list = new List<BattleTarget>();
			Target target_opposite = (targets_opposite.Count > 1) ? targets_opposite[1] : ((targets_opposite.Count == 1) ? targets_opposite[0] : null);
			Target target_friend = StrategyUtils.FindMaxLossHealthTarget(targets_friendly);
			if (target_friend == null)
				target_friend = StrategyUtils.FindMinHealthTarget(targets_friendly);

			foreach (HsMercenaryStrategy.Mercenary mercenary in mercenaries)
			{
				BattleTarget battleTarget = new BattleTarget();

				//先 全自动做任务队列
				foreach (Skill skill in mercenary.Skills)
				{
					if (TaskUtils.HasSkill(skill.Name))
					{
						battleTarget.SkillId = skill.Id;
						battleTarget.TargetType = TaskUtils.FindSkillTargetType(skill.Name);
						break;
					}
				}

				//再 设置的优先级队列FirstAbilityName
				if (battleTarget.SkillId == -1)
				{
					foreach (Skill skill in mercenary.Skills)
					{
						if (DefaultStrategy.FirstAbilityName.Contains(skill.Name))
						{
							battleTarget.SkillId = skill.Id;
							break;
						}
					}
				}
				//最后 用第一个得了
				if (battleTarget.SkillId == -1)
				{
					battleTarget.SkillId = ((mercenary.Skills.Count > 0) ? mercenary.Skills[0].Id : -1);
				}

				//设置目标
				if (battleTarget.TargetType == HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				{
					battleTarget.TargetId = ((target_friend != null) ? target_friend.Id : -1);
				}
				else
				{
					battleTarget.TargetId = ((target_opposite != null) ? target_opposite.Id : -1);
				}


				list.Add(battleTarget);
			}
			return list;
		}

		
		public string Name()
		{
			return "_Sys_Default";
		}

		
		public const string DefaultName = "_Sys_Default";

		
		private static readonly List<string> FirstAbilityName = new List<string>
		{
			"地狱火",
			"死吧，虫子",
			"振奋之歌",
			"烈焰之刺"
		};
	}
}
