using Assets;
using Hearthstone.DataModels;
using System;
using System.Collections.Generic;

namespace Mercenary
{
	internal class QuestManager
	{
		public QuestManager()
		{
			m_RollTime = DateTime.Now.AddHours(interval);
		}

		public static QuestManager Instance
		{
			get { return m_Instance ?? (m_Instance = new QuestManager()); }
		}

		public void RollAQuest()
		{
			Out.Log("[任务调整]");
			// 			if (DateTime.Now < m_RollTime)
			// 				return;
			//
			// 			m_RollTime = m_RollTime.Date.AddDays(1).AddHours(interval);
			// 			Out.Log(string.Format("[任务调整] 下次检测时间为{0}",
			// 				m_RollTime.ToString("G")));

			Random random = new Random();
			Hearthstone.Progression.QuestManager quest = Hearthstone.Progression.QuestManager.Get();
			if (quest != null)
			{
				{
					//获取所有每日任务
					QuestListDataModel questDay = quest.CreateActiveQuestsDataModel(
						QuestPool.QuestPoolType.DAILY, QuestPool.RewardTrackType.GLOBAL, true);

					//筛选0进度每日任务
					List<QuestDataModel> questValidDay = new List<QuestDataModel>();
					foreach (QuestDataModel item in questDay.Quests)
					{
						if (item == null) break;
						if (item.QuestId > 0 && item.Progress == 0 &&
							item.Quota > 0 && item.RerollCount > 0)
						{
							questValidDay.Add(item);
						}
					}

					//随机更换1个任务
					if (questValidDay.Count > 0)
					{
						int idx = random.Next(questValidDay.Count);
						QuestDataModel questRe = questValidDay[idx];
						quest.RerollQuest(questRe.QuestId);
						Out.Log(string.Format("[任务调整] 随机更换无法完成的每日任务{0}：{1}",
							questRe.QuestId, questRe.Description));
					}
				}

				{
					//获取所有每周任务
					QuestListDataModel quesWeek = quest.CreateActiveQuestsDataModel(
						QuestPool.QuestPoolType.WEEKLY, QuestPool.RewardTrackType.GLOBAL, true);

					//筛选0进度每周任务
					List<QuestDataModel> questValidWeek = new List<QuestDataModel>();
					foreach (QuestDataModel item in quesWeek.Quests)
					{
						if (item == null) break;
						if (item.QuestId > 0 && item.Progress == 0 &&
							item.Quota > 0 && item.RerollCount > 0)
						{
							questValidWeek.Add(item);
						}
					}

					//随机更换1个任务
					if (questValidWeek.Count > 0)
					{
						int idx = random.Next(questValidWeek.Count);
						QuestDataModel questRe = questValidWeek[idx];
						quest.RerollQuest(questRe.QuestId);
						Out.Log(string.Format("[任务调整] 随机更换无法完成的每周任务{0}：{1}",
							questRe.QuestId, questRe.Description));
					}
				}
			}
		}

		private const int interval = 2;

		private static QuestManager m_Instance;
		private DateTime m_RollTime;
	}
}