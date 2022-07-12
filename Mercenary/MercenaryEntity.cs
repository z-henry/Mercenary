using System;

namespace Mercenary
{
	
	public class MercenaryEntity
	{
		
		public MercenaryEntity(int id)
		{
			this.ID = id;
		}

		
		public MercenaryEntity(int id, string skill, int eq = 0, int subskillindex = 0, HsMercenaryStrategy.TARGETTYPE targettype = HsMercenaryStrategy.TARGETTYPE.UNSPECIFIED)
		{
			this.ID = id;
			this.Skill = skill;
			this.Equipment = eq;
			this.SubSkillIndex = subskillindex;
			this.TargetType = targettype;
			LettuceMercenary mercenary = CollectionManager.Get().GetMercenary((long)id, true, true);
// 			LettuceMercenaryDbfRecord record = GameDbf.LettuceMercenary.GetRecord(id);
// 			EntityDef entityDef = DefLoader.Get().GetEntityDef(record.ID, true);

			if (mercenary == null)
				this.Name = "";
			else
				this.Name = mercenary.m_mercName;
		}

		
		public readonly int ID;
		public readonly string Skill;
		public readonly int Equipment;
		public readonly int SubSkillIndex=0;
		public readonly HsMercenaryStrategy.TARGETTYPE TargetType = HsMercenaryStrategy.TARGETTYPE.UNSPECIFIED;

		public readonly string Name;
	}
}
