using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercenary
{
	
	public static class TaskUtils
	{
		
		public static void UpdateTask()
		{
			TaskUtils.UpdateTaskInfo(HsGameUtils.GetTasks());
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
					Task task = enumerator.Current;
					Task task2 = TaskUtils.tasks.Find((Task x) => x.Id == task.Id);
					task.StartAt = ((task2 != null) ? task2.StartAt : TaskUtils.Current());
				}
			}
			TaskUtils.tasks = newTasks;
			TaskUtils.dict_skillsTargetType = TaskUtils.GetTaskSkillAndTargetType(TaskUtils.tasks);
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

		public static bool HasSkill(string skill)
		{
			return TaskUtils.dict_skillsTargetType.ContainsKey(skill);
		}


		public static HsMercenaryStrategy.TARGETTYPE FindSkillTargetType(string skill)
		{
			if (!TaskUtils.dict_skillsTargetType.ContainsKey(skill))
			{
				return HsMercenaryStrategy.TARGETTYPE.UNSPECIFIED;
			}
			else
			{
				return dict_skillsTargetType[skill];
			}
		}

		
		public static List<Task> GetTasks()
		{
			List<Task> taskOrder = (from t in TaskUtils.tasks orderby t.Priority, t.water select t).ToList<Task>();
			return taskOrder;
		}

		
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

		
		private static Dictionary<string, HsMercenaryStrategy.TARGETTYPE> GetTaskSkillAndTargetType(List<Task> allTasks)
		{
			Dictionary<string, HsMercenaryStrategy.TARGETTYPE> list = new Dictionary<string, HsMercenaryStrategy.TARGETTYPE>();
			foreach (MercenaryEntity mercenaryEntity in TaskUtils._GetTaskMercenaries(allTasks))
			{
				if (!string.IsNullOrEmpty(mercenaryEntity.Skill))
				{
					if (!list.ContainsKey(mercenaryEntity.Skill))
						list.Add(mercenaryEntity.Skill, mercenaryEntity.TargetType);
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

		
		private static Dictionary<string, HsMercenaryStrategy.TARGETTYPE> dict_skillsTargetType = new Dictionary<string, HsMercenaryStrategy.TARGETTYPE>();
	}
}
