using System;
using System.Collections.Generic;

namespace Mercenary
{
	// Token: 0x0200000D RID: 13
	public static class TaskAdapter
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00005234 File Offset: 0x00003434
		public static void SetTask(int taskId, int mercenaryId, string title, string desc, List<Task> tasks)
		{
			// 对于优先使用多个英雄配合的，优先级设为0，保证一起出场
			// 未满30级的佣兵 优先级设为10，放后备箱里跟着升级
			// 部分要求地图的基本优先级都是5（正常）挨个排着
			// 英雄悬赏优先度为1，为了能早日排进H1-1的地图
			Out.Log(string.Format("[TID:{0}][MID:{1}] {2} {3}",
				taskId, mercenaryId, title, desc));
			if (title.Contains("势均力敌") || title.Contains("无坚不摧"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "巨型大恶魔", 0),
					TaskAdapter.GetMercenary(MercConst.玛诺洛斯, null, 0)
				}));
				return;
			}
			if (title.Contains("学术道德"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "急速冰冻", 0),
					TaskAdapter.GetMercenary(mercenaryId, "冰枪术", 0)
				}));
				return;
			}
			if (title.Contains("调节温度"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "冰枪术", 0)
				}));
				return;
			}
			if (title.Contains("完美圣光"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "裂解之光", 0),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉, null, 0)
				}));
				return;
			}
			if (title.Contains("全部烧光"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "熔岩冲击", 0),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯, null, 0)
				}));
				return;
			}
			if (title.Contains("圣光治愈一切"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "咒逐", 0),
					TaskAdapter.GetMercenary(mercenaryId, "祈福", 0)
				}));
				return;
			}
			if (title.Contains("可口凡人"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "清理巢穴", 0)
				}));
				return;
			}
			if (title.Contains("阳炎之爪"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "烈焰之歌", 0)
				}));
				return;
			}
			if (title.Contains("王者祝福") || title.Contains("银色北伐军"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "王者祝福", 0, 0, HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}
			if (title.Contains("生命守护者"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "巨龙吐息", 0, 0, HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}
			if (title.Contains("一举两得") || title.Contains("蹈踏火焰"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "烈焰猛击", 0),
					TaskAdapter.GetMercenary(MercConst.玉珑, null, 0)
				}));
				return;
			}
			if (title.Contains("光明使者"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "复仇之怒", 0),
					TaskAdapter.GetMercenary(MercConst.凯瑞尔, "光明圣印", 0),
					TaskAdapter.GetMercenary(MercConst.考内留斯, "牺牲祝福", 0)
				}));
				return;
			}
			if (title.Contains("烧烤弱鸡"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "活体炸弹", 0),
					TaskAdapter.GetMercenary(MercConst.厨师曲奇, "小鱼快冲", 0)
				}));
				return;
			}
			if (title.Contains("手脚并用"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "缴械", 0)
				}));
				return;
			}
			if (title.Contains("现在归我了"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 4, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "攻击", 0),
					TaskAdapter.GetMercenary(mercenaryId, "暗影箭", 0),
					TaskAdapter.GetMercenary(mercenaryId, "锋利尖刺", 0),
					TaskAdapter.GetMercenary(mercenaryId, "快速治疗", 0)
				}));
				return;
			}
			if (title.Contains("消灭入侵者"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "4-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "屠龙射击", 0)
				}));
				return;
			}
			if (title.Contains("治疗朋友"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "妖精之尘", 0)
				}));
				return;
			}
			if (title.Contains("为了暴风城") || title.Contains("屹立不倒"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "反击", 0)
				}));
				return;
			}
			if (title.Contains("船上的厨子"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, null, 0),
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (title.Contains("侵略如火") || title.Contains("翻腾火流"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
					TaskAdapter.GetMercenary(mercenaryId, "热力迸发", 0)
				}));
				return;
			}
			if (title.Contains("雪球滚滚"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "3-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "雪球", 0),
					TaskAdapter.GetMercenary(MercConst.瓦尔登, "急速冰冻", 0)
				}));
				return;
			}
			if (title.Contains("致命一击"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "首脑的悬赏", 0),
					TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, null, 0),
					TaskAdapter.GetMercenary(MercConst.尤朵拉, null, 0)
				}));
				return;
			}
			if (desc.Contains("击败") && desc.Contains("个恶魔"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (desc.Contains("击败") && desc.Contains("条龙"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "4-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (desc.Contains("击败") && desc.Contains("只野兽"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (desc.Contains("击败") && desc.Contains("元素"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (desc.Contains("加快友方技能的速度值总") || desc.Contains("使一个友方技能的速度值加快"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "2-5", TaskAdapter.GetQuickMercenary(mercenaryId)));
				return;
			}
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用恶魔造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.拉索利安, "巨型大恶魔", 0)
				}));
				return;
			}
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用野兽造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.闪狐, null, 0)
				}));
				return;
			}
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用元素造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 0)
				}));
				return;
			}
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("神圣伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉, null, 0)
				}));
				return;
			}
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("冰霜伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.瓦尔登, null, 0)
				}));
				return;
			}
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("暗影伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.塔姆辛, null, 0),
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("邪能伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.玛诺洛斯, null, 0),
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}

			if (desc.Contains("英雄难度"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 1, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (desc.Contains("完成") && desc.Contains("个悬赏"))
			{
				LettuceMercenary mercenary = HsGameUtils.GetMercenary(mercenaryId);
				if (desc.Contains("30级时") && !mercenary.IsMaxLevel())
				{
					tasks.Add(TaskAdapter.GetTask(taskId ,10 , new MercenaryEntity[]
					{
						TaskAdapter.GetMercenary(mercenaryId, null, 0)
					}));
				}
				else
				{
					tasks.Add(TaskAdapter.GetTask(taskId, 5, "H1-1", new MercenaryEntity[]
					{
						TaskAdapter.GetMercenary(mercenaryId, null, 0)
					}));
				}
				return;
			}
			if (desc.Contains("利用赐福"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 10, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			int num = desc.IndexOf("$ability(", StringComparison.Ordinal);
			if (num == -1)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			int num2 = num + "$ability(".Length;
			int num3 = desc.IndexOf(")", num2, StringComparison.Ordinal);
			if (num3 == -1)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			string[] array = desc.Substring(num2, num3 - num2).Split(new char[]
			{
				','
			});
			int num4;
			int num5;
			if (!int.TryParse(array[0], out num4) || !int.TryParse(array[1], out num5))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			LettuceMercenaryDbfRecord record = GameDbf.LettuceMercenary.GetRecord(mercenaryId);
			if (num4 >= record.LettuceMercenarySpecializations.Count)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			LettuceMercenarySpecializationDbfRecord lettuceMercenarySpecializationDbfRecord = record.LettuceMercenarySpecializations[num4];
			if (num5 >= lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities.Count)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			int lettuceAbilityId = lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities[num5].LettuceAbilityId;
			LettuceAbilityDbfRecord record2 = GameDbf.LettuceAbility.GetRecord(lettuceAbilityId);
			tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
			{
				TaskAdapter.GetMercenary(mercenaryId, record2.AbilityName, GetEquipEnHanceSkill(record2.AbilityName))
			}));
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00005BB0 File Offset: 0x00003DB0
		private static MercenaryEntity[] GetQuickMercenary(int mercenaryId)
		{
			List<MercenaryEntity> list = new List<MercenaryEntity>();
			foreach (LettuceAbility lettuceAbility in CollectionManager.Get().GetMercenary((long)mercenaryId, false, true).m_abilityList)
			{
				CardDbfRecord cardRecord = GameDbf.GetIndex().GetCardRecord(lettuceAbility.GetCardId());
				string @string = cardRecord.TextInHand.GetString(true);
				if (@string != null && @string.Contains("速度值") && @string.Contains("加快"))
				{
					string string2 = cardRecord.Name.GetString(true);
					list.Add(TaskAdapter.GetMercenary(mercenaryId, string2.Substring(0, string2.Length - 1), 0));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005C80 File Offset: 0x00003E80
		private static Task GetTask(int id, int priority, string map, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Priority = priority,
				Map = map,
				Mercenaries = new List<MercenaryEntity>(mercenaries),
				Id = id
			};
		}

		private static Task GetTask(int id, int priority, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Id = id,
				Mercenaries = new List<MercenaryEntity>(mercenaries),
				Priority = priority,
			};
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005CA8 File Offset: 0x00003EA8
		private static Task GetTask(int id, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Id = id,
				Mercenaries = new List<MercenaryEntity>(mercenaries),
				Priority = 5
			};
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005CC9 File Offset: 0x00003EC9
		private static MercenaryEntity GetMercenary(int id, string skill = null, int eq = 0, int subskill = 0, HsMercenaryStrategy.TARGETTYPE targettype = HsMercenaryStrategy.TARGETTYPE.UNSPECIFIED)
		{
			return new MercenaryEntity(id, skill, eq, subskill, targettype);
		}

		private static int GetEquipEnHanceSkill(string skill)
		{
			return m_dictSkillEquip.ContainsKey(skill) ? m_dictSkillEquip[skill] : 0;
		}

		private static Dictionary<string, int> m_dictSkillEquip = new Dictionary<string, int>{
			{ "大地践踏", EquipConst.雷霆饰带 },// 凯恩·血蹄|大地践踏|雷霆饰带
			{ "再生头颅", EquipConst.再生之鳞 },// 特里高雷|再生头颅|再生之鳞
			{ "首脑的悬赏", EquipConst.公平分配 } // 迪菲亚|首脑的悬赏|公平分配
		};

		private static int m_globalWater = 0;
	}
}
