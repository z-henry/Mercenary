using System;

namespace Mercenary
{
	// Token: 0x0200000A RID: 10
	public class MercenaryEntity
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000046A9 File Offset: 0x000028A9
		public MercenaryEntity(int id)
		{
			this.ID = id;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000046B8 File Offset: 0x000028B8
		public MercenaryEntity(int id, string skill, int eq = 0)
		{
			this.ID = id;
			this.Skill = skill;
			this.Equipment = eq;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000046D5 File Offset: 0x000028D5
		public MercenaryEntity(int id, int skillIndex, int eq = 0)
		{
			this.ID = id;
			this.SkillIndex = skillIndex;
			this.Equipment = eq;
		}

		// Token: 0x04000032 RID: 50
		public readonly int ID;

		// Token: 0x04000033 RID: 51
		public readonly string Skill;

		// Token: 0x04000034 RID: 52
		public readonly int Equipment;

		// Token: 0x04000035 RID: 53
		public readonly int SkillIndex;
	}
}
