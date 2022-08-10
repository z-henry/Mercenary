using System;
using System.Collections.Generic;
using HsMercenaryStrategy;

namespace Mercenary
{
	
	public class DefaultStrategy : IStrategy
	{
		public (int hand_index, int play_index) GetEnterOrder(List<HsMercenaryStrategy.Mercenary> hand_mercenaries, List<HsMercenaryStrategy.Mercenary> play_mercenaries)
		{
			return (0, play_mercenaries.Count);
		}


		public List<BattleTarget> GetBattleTargets(List<HsMercenaryStrategy.Mercenary> mercenaries, List<Target> targets_opposite, List<Target> targets_friendly)
		{
// 			Out.Log("default" + targets_opposite.Count.ToString());
			List<BattleTarget> battleTargets = new List<BattleTarget>();

			Target target_opposite = (targets_opposite.Count > 1) ? targets_opposite[1] : ((targets_opposite.Count == 1) ? targets_opposite[0] : null);
			Target target_friend = StrategyUtils.FindMaxLossHealthTarget(targets_friendly);
			if (target_friend == null)
				target_friend = StrategyUtils.FindMinHealthTarget(targets_friendly);

			foreach (HsMercenaryStrategy.Mercenary mercenary in mercenaries)
			{
				List<BattleTarget> merc_battleTargets = new List<BattleTarget>();

				//先 任务有的技能都要添加进来
				List<MercenaryEntity> taskMercenarys = TaskUtils.GetTaskMercenaries(mercenary.Name);
// 				if (taskMercenarys.Count < 1)
// 					Out.Log(string.Format("[策略未命中] [MNAME:{0}]", mercenary.Name));
// 
				foreach (MercenaryEntity taskMercenary in taskMercenarys)
				{
					Skill skill = mercenary.Skills.Find((Skill i) => i.Name == taskMercenary.Skill);
					if (skill != null)
					{
						merc_battleTargets.Add(new BattleTarget()
						{
							SkillId = skill.Id, 
							SkillName = skill.Name, 
							SubSkillIndex = taskMercenary.SubSkillIndex, 
							TargetType = taskMercenary.TargetType
						});
					}
				}


				//再 设置的优先级队列FirstAbilityName
				foreach (Skill skill in mercenary.Skills)
				{
					if (DefaultStrategy.FirstAbilityName.Contains(skill.Name))
					{
						merc_battleTargets.Add(new BattleTarget()
						{
							SkillId = skill.Id,
							SkillName = skill.Name
						});

					}
				}
				//最后 用第一个得了
				if (merc_battleTargets.Count <= 0)
				{
					if (mercenary.Skills.Count > 0)
						merc_battleTargets.Add(new BattleTarget()
						{
							SkillId = mercenary.Skills[0].Id,
							SkillName = mercenary.Skills[0].Name
						});
					else
						merc_battleTargets.Add(new BattleTarget()
						{
							SkillId = -1,
							SkillName = ""
						});

				}

				battleTargets.AddRange(merc_battleTargets);
			}

			//设置目标
			using (List<BattleTarget>.Enumerator enumerator = battleTargets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleTarget battleTarget = enumerator.Current;
					if (battleTarget.TargetType == HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
						battleTarget.TargetId = ((target_friend != null) ? target_friend.Id : -1);
					else
						battleTarget.TargetId = ((target_opposite != null) ? target_opposite.Id : -1);
				}
			}

			return battleTargets;
		}

		
		public string Name()
		{
			return DefaultName;
		}

		
		public const string DefaultName = "_Sys_Default";

		
		private static readonly List<string> FirstAbilityName = new List<string>
		{
		};
	}
}
