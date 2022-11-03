using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercenary
{
	public static class TaskUtils
	{
		public static void UpdateMercTask()
		{
			TaskUtils.ClearTaskSpecialNode();
			TaskUtils.UpdateTaskInfo(HsGameUtils.GetMercTasks());
			foreach (Task task in TaskUtils.GetTasks())
				Out.Log($"[TID:{task.Id}] 已持续：{TaskUtils.Current() - task.StartAt}s");
		}

		public static void UpdateMainLineTask()
		{
			TaskUtils.UpdateTaskInfo(HsGameUtils.GetMainLineTask());
		}

		public static void ClearTaskSpecialNode()
		{
			HaveTaskDocter = false;
			HaveTaskTank = false;
			HaveTaskFighter = false;
			HaveTaskCaster = false;
		}

		public static long Current()
		{
			return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
		}

		private static void UpdateTaskInfo(List<Task> newTasks)
		{
			using (List<Task>.Enumerator enumerator = newTasks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Task task_new = enumerator.Current;
					Task task_old = TaskUtils.tasks.Find((Task x) => x.Id == task_new.Id && x.ProgressMessage == task_new.ProgressMessage);
					task_new.StartAt = ((task_old != null) ? task_old.StartAt : TaskUtils.Current());
				}
			}
			TaskUtils.tasks = newTasks;
		}

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

		public static List<Task> GetTasks()
		{
			List<Task> taskOrder = (from t in TaskUtils.tasks orderby t.Priority, t.water select t).ToList<Task>();
			return taskOrder;
		}

		public static List<MercenaryEntity> GetTaskMercenaries(string mercName)
		{
			List<MercenaryEntity> list = new List<MercenaryEntity>();
			foreach (Task task in tasks)
			{
				foreach (MercenaryEntity item in task.Mercenaries)
				{
					if (item.Name != mercName)
						continue;
					list.Add(item);
				}
			}
			return list;
		}

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

		private static List<Task> tasks = new List<Task>();

		//有对应赐福任务
		public static bool HaveTaskTank { get; set; }

		public static bool HaveTaskFighter { get; set; }
		public static bool HaveTaskCaster { get; set; }
		public static bool HaveTaskDocter { get; set; }
	}
}