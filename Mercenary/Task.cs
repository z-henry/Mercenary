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

		public int water;//流水号，保证同优先级的任务是按照添加顺序排列的（长时间不重启可能会溢出）
		public int Id;// taskid
		public string TaskName;// task标题
		public string TaskDesc;// task细节
		public string ProgressMessage;//task进度

		public int Priority; // 优先级 小数优先级高
		public string Map;//1-1 h2-5 等
		public List<MercenaryEntity> Mercenaries = new List<MercenaryEntity>();// 佣兵以及佣兵技能选择列表
		public long StartAt;//任务进度起始时间
		public int mercID;//任务的佣兵id
		public string mercName;//任务的佣兵名称
	}
}