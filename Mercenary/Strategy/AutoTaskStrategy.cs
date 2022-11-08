using System.Collections.Generic;

namespace Mercenary.Strategy
{
	public class AutoTaskStrategy : DefaultStrategyPVE
	{
		public override (int hand_index, int play_index) GetEnterOrder(List<Target> hand_mercenaries, List<Target> play_mercenaries, Dictionary<MY_TAG_ROLE, int> dictOppositeRoleCount,
			List<Target> targets_opposite, List<Target> targets_opposite_graveyrad)
		{
			return (0, play_mercenaries.Count);
		}

		public override List<BattleTarget> GetBattleTargets(int turn, List<Target> targets_opposite_all, List<Target> targets_friendly_all, List<Target> targets_opposite_graveyrad)
		{
			var targets_opposite = targets_opposite_all.FindAll((Target t) => t.Enable == true);
			var targets_friendly = targets_friendly_all.FindAll((Target t) => t.Enable == true);

			List<BattleTarget> battleTargets = new List<BattleTarget>();

			foreach (Target mercenary in targets_friendly_all)
			{
				List<BattleTarget> merc_battleTargets = new List<BattleTarget>();

				// 有一些东西最好别动了
				if (mercenary.Name.IndexOf("黄金猿") == 0)
				{
					merc_battleTargets.Add(new BattleTarget()
					{
						MercName = mercenary.Name,
						NeedActive = false,
					});
					continue;
				}

				//先 任务有的技能都要添加进来
				List<MercenaryEntity> taskMercenarys = TaskUtils.GetTaskMercenaries(mercenary.Name);
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
					if (AutoTaskStrategy.FirstAbilityName.Contains(skill.Name))
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
			Target target_opposite = (targets_opposite.Count > 1) ? targets_opposite[1] : ((targets_opposite.Count == 1) ? targets_opposite[0] : null);
			Target target_friend = StrategyUtils.FindMaxLossHealthTarget(targets_friendly) ?? StrategyUtils.FindMinHealthTarget(targets_friendly);
			foreach (BattleTarget battleTarget in battleTargets)
			{
				if (battleTarget.TargetType == TARGETTYPE.FRIENDLY)
				{
					battleTarget.TargetId = target_friend?.Id ?? -1;
					battleTarget.TargetName = target_friend?.Name ?? "";
				}
				else
				{
					battleTarget.TargetId = target_opposite?.Id ?? -1;
					battleTarget.TargetName = target_opposite?.Name ?? "";
				}
			}

			return battleTargets;
		}

		public override string Name
		{
			get { return "佣兵任务策略"; }
		}

		private static readonly List<string> FirstAbilityName = new List<string>
		{
		};
	}
}