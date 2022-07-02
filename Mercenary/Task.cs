using System;
using System.Collections.Generic;

namespace Mercenary
{
	// Token: 0x02000009 RID: 9
	public class Task
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00004665 File Offset: 0x00002865
		public Task()
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004678 File Offset: 0x00002878
		public Task(int id)
		{
			this.Mercenaries.Add(new MercenaryEntity(id));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000469C File Offset: 0x0000289C
		public int GetMapId()
		{
			return MapUtils.GetMapId(this.Map);
		}

		//流水号，保证同优先级的任务是按照添加顺序排列的（长时间不重启可能会溢出）
		public int water;

		// Token: 0x0400002D RID: 45
		public int Id;

		// Token: 0x0400002E RID: 46
		public int Priority;

		// Token: 0x0400002F RID: 47
		public string Map;

		// Token: 0x04000030 RID: 48
		public List<MercenaryEntity> Mercenaries = new List<MercenaryEntity>();

		// Token: 0x04000031 RID: 49
		public long StartAt;

	}
}
