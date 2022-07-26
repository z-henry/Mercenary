﻿using System.Collections.Generic;
using Mercenary.Strategy;

namespace Mercenary
{
	public class MercenaryEntity
	{
		public MercenaryEntity(int id)
		{
			this.ID = id;
		}

		public MercenaryEntity(int id, string skill, int eq = 0, int subskillindex = -1, TARGETTYPE targettype = TARGETTYPE.UNSPECIFIED)
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
		public readonly int SubSkillIndex = 0;
		public readonly TARGETTYPE TargetType = TARGETTYPE.UNSPECIFIED;

		public readonly string Name;
	}



	public class MercenaryEntityComparer : IEqualityComparer<MercenaryEntity>
	{
		public bool Equals(MercenaryEntity x, MercenaryEntity y)
		{
			//这里定义比较的逻辑
			return x.ID == y.ID;
		}

		public int GetHashCode(MercenaryEntity obj)
		{
			//返回字段的HashCode，只有HashCode相同才会去比较
			return obj.ID.GetHashCode();
		}
	}
}
