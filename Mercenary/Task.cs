using System;
using System.Collections.Generic;

namespace Mercenary
{
	
	public class Task
	{
		
		public Task()
		{
		}

		
		public Task(int id)
		{
			this.Mercenaries.Add(new MercenaryEntity(id));
		}

		
		public int GetMapId()
		{
			return MapUtils.GetMapId(this.Map);
		}

		//流水号，保证同优先级的任务是按照添加顺序排列的（长时间不重启可能会溢出）
		public int water;

		
		public int Id;

		
		public int Priority;

		
		public string Map;

		
		public List<MercenaryEntity> Mercenaries = new List<MercenaryEntity>();

		
		public long StartAt;

	}
}
