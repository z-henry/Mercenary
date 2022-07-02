using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercenary
{
	// Token: 0x0200000E RID: 14
	public static class TaskUtils
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00005CDD File Offset: 0x00003EDD
		public static void UpdateTask()
		{
			TaskUtils.UpdateTaskInfo(HsGameUtils.GetTasks());
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005CEC File Offset: 0x00003EEC
		public static long Current()
		{
			return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005D0C File Offset: 0x00003F0C
		private static void UpdateTaskInfo(List<Task> newTasks)
		{
			using (List<Task>.Enumerator enumerator = newTasks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Task task = enumerator.Current;
					Task task2 = TaskUtils.tasks.Find((Task x) => x.Id == task.Id);
					task.StartAt = ((task2 != null) ? task2.StartAt : TaskUtils.Current());
				}
			}
			TaskUtils.tasks = newTasks;
			TaskUtils.skills = TaskUtils.GetTaskSkill(TaskUtils.tasks);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005DA8 File Offset: 0x00003FA8
		public static int GetTaskMap()
		{
			List<Task> taskOrder = (from t in TaskUtils.tasks orderby t.Priority, t.water select t).ToList<Task>();
			foreach (Task task in taskOrder)
			{
				if (task.GetMapId() != -1)
				{
					return task.GetMapId();
				}
			}
			return -1;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005E08 File Offset: 0x00004008
		public static bool HasSkill(string skill)
		{
			return TaskUtils.skills.Contains(skill);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005E15 File Offset: 0x00004015
		public static List<Task> GetTasks()
		{
			List<Task> taskOrder = (from t in TaskUtils.tasks orderby t.Priority, t.water select t).ToList<Task>();
			return taskOrder;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005E48 File Offset: 0x00004048
		private static List<MercenaryEntity> _GetTaskMercenaries(List<Task> allTasks)
		{
			List<MercenaryEntity> list = new List<MercenaryEntity>();
			foreach (Task task in allTasks)
			{
				foreach (MercenaryEntity item in task.Mercenaries)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005ED8 File Offset: 0x000040D8
		private static List<string> GetTaskSkill(List<Task> allTasks)
		{
			List<string> list = new List<string>();
			foreach (MercenaryEntity mercenaryEntity in TaskUtils._GetTaskMercenaries(allTasks))
			{
				if (!string.IsNullOrEmpty(mercenaryEntity.Skill))
				{
					list.Add(mercenaryEntity.Skill);
				}
			}
			return list;
		}

		// Token: 0x0400003A RID: 58
		public static readonly Dictionary<string, int> CleanConf = new Dictionary<string, int>
		{
			{
				"不开启",
				-1
			},
			{
				"20分钟",
				1200
			},
			{
				"40分钟",
				2400
			},
			{
				"60分钟",
				3600
			},
			{
				"120分钟",
				7200
			}
		};

		// Token: 0x0400003B RID: 59
		private static List<Task> tasks = new List<Task>();

		// Token: 0x0400003C RID: 60
		private static List<string> skills = new List<string>();
	}
}
