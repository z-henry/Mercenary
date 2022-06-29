using System;
using System.Collections.Generic;
using HsMercenaryStrategy;

namespace Mercenary
{
	// Token: 0x02000002 RID: 2
	public class DefaultStrategy : IStrategy
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public List<BattleTarget> GetBattleTargets(List<HsMercenaryStrategy.Mercenary> mercenaries, List<Target> targets)
		{
			Out.Log("default" + targets.Count.ToString());
			List<BattleTarget> list = new List<BattleTarget>();
			Target target = (targets.Count > 1) ? targets[1] : ((targets.Count == 1) ? targets[0] : null);
			foreach (HsMercenaryStrategy.Mercenary mercenary in mercenaries)
			{
				BattleTarget battleTarget = new BattleTarget();
				foreach (Skill skill in mercenary.Skills)
				{
					if (TaskUtils.HasSkill(skill.Name))
					{
						battleTarget.SkillId = skill.Id;
						break;
					}
				}
				if (battleTarget.SkillId == -1)
				{
					foreach (Skill skill2 in mercenary.Skills)
					{
						if (DefaultStrategy.FirstAbilityName.Contains(skill2.Name))
						{
							battleTarget.SkillId = skill2.Id;
							break;
						}
					}
				}
				if (battleTarget.SkillId == -1)
				{
					battleTarget.SkillId = ((mercenary.Skills.Count > 0) ? mercenary.Skills[0].Id : -1);
				}
				battleTarget.TargetId = ((target != null) ? target.Id : -1);
				list.Add(battleTarget);
			}
			return list;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000222C File Offset: 0x0000042C
		public string Name()
		{
			return "_Sys_Default";
		}

		// Token: 0x04000001 RID: 1
		public const string DefaultName = "_Sys_Default";

		// Token: 0x04000002 RID: 2
		private static readonly List<string> FirstAbilityName = new List<string>
		{
			"地狱火",
			"死吧，虫子",
			"振奋之歌",
			"烈焰之刺"
		};
	}
}
