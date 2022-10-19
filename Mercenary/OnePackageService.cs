using Assets;
using Hearthstone.DataModels;
using System;
using System.Collections.Generic;

namespace Mercenary
{
	public class StageInfo
	{
		public StageInfo(Mode mode, int mapid, Type teamType, int teamtotal)
		{
			m_mode = mode;
			m_mapid = mapid;
			m_teamType = teamType;
			m_teamTotal = teamtotal;
		}
		public Mode m_mode;
		public int m_mapid;
		public Type m_teamType;
		public int m_teamTotal;
	}
	public static class OnePackageService
	{
		public enum STAGE
		{
			满级_初始四人 = 0,
			获得_萨穆罗,
			满级_初始五人,
			解锁_2杠6,
			获得_AOE队,
			刷满_AOE队,
			获得_大德,
			获得_自然队,
			获得_大德装备3,
			满级_自然队,
			解锁_4杠1,
			获得_拉格纳罗斯,
			获得_迦顿男爵,
			获得_安东尼达斯,
			刷满_初级火焰队,
			获得_紫色配合佣兵,
			刷满任务栏,
			获得_预设卡组,
			刷满_预设卡组,
			解锁_5杠5,
			获得_巴琳达装备2雪王装备1,
			获得_全佣兵,
			解锁_主线,
			解锁_全佣兵装备,
			刷1杠1,

		}
		public static STAGE Stage { get { return m_stage; } private set { m_stage = value; } }

		public static void UpdateStage()
		{
			Stage = ClacStage();
			Out.Log($"[一条龙阶段] 阶段{(int)Stage} {Stage}");
		}
		private static STAGE ClacStage()
		{
			try
			{
				//target1：初始队伍，凯瑞尔，豪斯，泰兰德，泽瑞拉，4个人，是否全部30级
				//一旦有一个未30级
				//返回0 刷图1-1 
				foreach (var iter in DefaultTeam.Origin0.Type.TeamInfo)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.IsReadyForCrafting()
						&& mercenary.m_owned
						&& !mercenary.IsMaxLevel())
					{
						return STAGE.满级_初始四人;
					}
				}

				//target2：剑圣是否拥有
				//如果未拥有返回1 自动主线
				LettuceMercenary mercenary_temp = HsGameUtils.GetMercenary(MercConst.剑圣萨穆罗);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_萨穆罗;
				}

				//target3: 初始队伍，豪斯，泽瑞拉，剑圣，泰兰德，凯瑞尔， 5个人，是否30级
				//返回2 刷图H1-1
				foreach (var iter in DefaultTeam.Origin.Type.TeamInfo)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.IsReadyForCrafting()
						&& mercenary.m_owned
						&& !mercenary.IsMaxLevel())
					{
						return STAGE.满级_初始五人;
					}
				}

				//target4：2-6 是否解锁
				//返回3 自动主线
				if (false == MercenariesDataUtil.IsBountyComplete(72))
				{
					return STAGE.解锁_2杠6;
				}

				//target5: AOE初级队，是否获得，雪王，晨拥，米尔豪斯
				//返回4 2-6  初始队 神秘人 
				foreach (var iter in DefaultTeam.AOE.Type.TeamInfo)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.m_owned)
					{
						return STAGE.获得_AOE队;
					}
				}

				//target6: AOE初级队， 是否碎片够满级  雪王，晨拥，米尔豪斯
				//返回5 H1-1 刷图
				foreach (var iter in DefaultTeam.AOE.Type.TeamInfo)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 2000)
					{
						return STAGE.刷满_AOE队;
					}
				}
				//target7: 大德是否获得 
				//返回6 刷图 2-3 
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.玛法里奥_怒风);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_大德;
				}

				//target8: 自然队是否全拥有
				//返回7 神秘人
				foreach (var iter in DefaultTeam.Nature.Type.TeamInfo)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.m_owned)
					{
						return STAGE.获得_自然队;
					}
				}

				//target9:玛法里奥是否获得了 装备3
				////返回8 刷图H1-1
				LettuceAbility lettuceAbility = mercenary_temp.m_equipmentList[2];
				if (!lettuceAbility.Owned)
				{
					return STAGE.获得_大德装备3;
				}

				//target10：自然队是否全满级
				//返回9 刷图H1-1
				int sum = 0;
				foreach (var iter in DefaultTeam.Nature.Type.TeamInfo)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 1000)
					{
						sum++;
					}
				}
				if (sum > 1)
				{
					return STAGE.满级_自然队;
				}

				//target11：4 - 1 是否解锁
				//返回10 自动主线
				if (false == MercenariesDataUtil.IsBountyComplete(78))
				{
					return STAGE.解锁_4杠1;
				}

				//target12： 是否有初级火焰队
				//返回11 刷图
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.拉格纳罗斯);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_拉格纳罗斯;
				}
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.迦顿男爵);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_迦顿男爵;
				}
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.安东尼达斯);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_安东尼达斯;
				}

				//target13： 初级火焰队是否碎片足够满级
				//返回12 刷图 H1-1
				foreach (var iter in DefaultTeam.PrimaryFire.Type.TeamInfo)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (mercenary.m_owned &&
						HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 1000)
					{
						return STAGE.刷满_初级火焰队;
					}
				}

				//target14：配合紫色佣兵是否集齐
				//返回13 刷神秘人
				sum = 0;
				foreach (int ID in MercConst.CoopTools)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(ID);
					if (!mercenary.m_owned)
					{
						//Out.Log($"{mercenary.m_mercName}未获得");
						sum++;
					}
				}
				if (sum > 8)
				{
					return STAGE.获得_紫色配合佣兵;
				}
				//target15： 当前的佣兵任务栏，是不是空的
				//返回14 佣兵任务
				//TaskUtils.UpdateTask();
				//Out.Log($"{TaskUtils.GetTasks().Count}");

				NetCache.NetCacheMercenariesVillageVisitorInfo netObject =
					NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
				// 因为新任务是添加在第一条，所以倒序做任务，先进先做
				for (int i = netObject.VisitorStates.Count - 1; i >= 0; --i)
				// 			foreach (MercenariesVisitorState mercenariesVisitorState in netObject.VisitorStates)
				{
					MercenaryVillageTaskItemDataModel mercenaryVillageTaskItemDataModel =
						LettuceVillageDataUtil.CreateTaskModelByTaskState(netObject.VisitorStates[i].ActiveTaskState, null, false, false);
					VisitorTaskDbfRecord taskRecordByID = LettuceVillageDataUtil.GetTaskRecordByID(netObject.VisitorStates[i].ActiveTaskState.TaskId);

					if (mercenaryVillageTaskItemDataModel.TaskType == MercenaryVisitor.VillageVisitorType.STANDARD)
					{
						return STAGE.刷满任务栏;
					}
				}

				//target16：预设卡组的所有佣兵是否获得
				//返回15 刷神秘人
				foreach (var iterTeam in DefaultTeam.TeamType.GetAllTeamType())
				{
					foreach (var iterMerc in iterTeam.TeamInfo)
					{
						LettuceMercenary mercenary = HsGameUtils.GetMercenary(iterMerc.id);
						if (!mercenary.m_owned)
						{
							return STAGE.获得_预设卡组;
						}
					}
				}
				//target17：预设卡组的所有佣兵是否全部碎片够+1+5
				//返回16 刷图H1-1
				foreach (var iterTeam in DefaultTeam.TeamType.GetAllTeamType())
				{
					foreach (var iterMerc in iterTeam.TeamInfo)
					{
						LettuceMercenary mercenary = HsGameUtils.GetMercenary(iterMerc.id);
						if (!mercenary.m_owned || HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 0)
						{
							return STAGE.刷满_预设卡组;
						}
					}
				}

				//target18: 冰火队的两个解锁装备的地图是否解锁128
				//返回17 自动主线
				if (false == MercenariesDataUtil.IsBountyComplete(128))
				{
					return STAGE.解锁_5杠5;
				}

				//target19：巴琳达 雪王 是否解锁装备
				//返回18 解锁装备
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.冰雪之王洛克霍拉);
				lettuceAbility = mercenary_temp.m_equipmentList[0];
				if (!lettuceAbility.Owned)
				{
					return STAGE.获得_巴琳达装备2雪王装备1;
				}
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.巴琳达_斯通赫尔斯);
				lettuceAbility = mercenary_temp.m_equipmentList[1];
				if (!lettuceAbility.Owned)
				{
					return STAGE.获得_巴琳达装备2雪王装备1;
				}


				//target20：是否全佣兵
				//返回19 刷神秘人
				List<LettuceMercenary> mercenaries = CollectionManager.Get().
					FindOrderedMercenaries(null, null, null, null, null).m_mercenaries;
				foreach (LettuceMercenary mercenary in mercenaries)
				{
					if (!mercenary.m_owned && !mercenary.IsReadyForCrafting())
					{
						return STAGE.获得_全佣兵;
					}
				}

				//target21：是否完成主线任务
				//返回20 主线任务
				if (HsGameUtils.GetMainLineTask().Count > 0)
				{
					return STAGE.解锁_主线;
				}

				//target22：是否有佣兵未解锁装备
				//返回21
				if (Main.UpdateAutoUnlockEquipInfo() == true)
				{
					return STAGE.解锁_全佣兵装备;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			//以上全部完成返回22
			//0核心刷图H1-1
			return STAGE.刷1杠1;
		}

		public static StageInfo TranslateCurrentStage()
		{
			return m_dictTranslate[m_stage];
		}

		private static Dictionary<STAGE, StageInfo> m_dictTranslate = new Dictionary<STAGE, StageInfo>() {
			{ STAGE.满级_初始四人, new StageInfo(Mode.刷图, 85, typeof(DefaultTeam.Origin0), 6) },
			{ STAGE.获得_萨穆罗, new StageInfo(Mode.自动主线, -1, typeof(DefaultTeam.Origin0), 6) },
			{ STAGE.满级_初始五人, new StageInfo(Mode.刷图, 85, typeof(DefaultTeam.Origin), 6) },
			{ STAGE.解锁_2杠6, new StageInfo(Mode.自动主线, -1, typeof(DefaultTeam.Origin), 6) },
			{ STAGE.获得_AOE队, new StageInfo(Mode.刷神秘人, 72, typeof(DefaultTeam.Origin), 6) },
			{ STAGE.刷满_AOE队, new StageInfo(Mode.刷图, 85, typeof(DefaultTeam.AOE), 6) },
			{ STAGE.获得_大德, new StageInfo(Mode.刷图, 69, typeof(DefaultTeam.AOE), 6) },
			{ STAGE.获得_自然队, new StageInfo(Mode.刷神秘人, 72, typeof(DefaultTeam.AOE), 6) },
			{ STAGE.获得_大德装备3, new StageInfo(Mode.全自动接任务做任务, -1, typeof(DefaultTeam.DruidsExclusive), 3) },
			{ STAGE.满级_自然队, new StageInfo(Mode.刷图, 85, typeof(DefaultTeam.Nature), 6) },
			{ STAGE.解锁_4杠1, new StageInfo(Mode.自动主线, -1, typeof(DefaultTeam.Nature), 6) },
			{ STAGE.获得_拉格纳罗斯, new StageInfo(Mode.刷图, 75, typeof(DefaultTeam.Nature), 6) },
			{ STAGE.获得_迦顿男爵, new StageInfo(Mode.刷图, 74, typeof(DefaultTeam.Nature), 6) },
			{ STAGE.获得_安东尼达斯, new StageInfo(Mode.刷图, 76, typeof(DefaultTeam.Nature), 6) },
			{ STAGE.刷满_初级火焰队, new StageInfo(Mode.刷图, 85, typeof(DefaultTeam.PrimaryFire), 6) },
			{ STAGE.获得_紫色配合佣兵, new StageInfo(Mode.刷神秘人, 72, typeof(DefaultTeam.PrimaryFire), 6) },
			{ STAGE.刷满任务栏, new StageInfo(Mode.全自动接任务做任务, -1, null, 6) },
			{ STAGE.获得_预设卡组, new StageInfo(Mode.刷神秘人, 72, typeof(DefaultTeam.PrimaryFire), 6) },
			{ STAGE.刷满_预设卡组, new StageInfo(Mode.刷图, 85, typeof(DefaultTeam.IceFire), 6) },
			{ STAGE.解锁_5杠5, new StageInfo(Mode.自动主线, -1, null, 6) },
			{ STAGE.获得_巴琳达装备2雪王装备1, new StageInfo(Mode.自动解锁装备, -1, null, 6) },
			{ STAGE.获得_全佣兵, new StageInfo(Mode.刷神秘人, 72, typeof(DefaultTeam.IceFire), 6) },
			{ STAGE.解锁_主线, new StageInfo(Mode.自动主线, -1, null, 6) },
			{ STAGE.解锁_全佣兵装备, new StageInfo(Mode.自动解锁装备, -1, null, 6) },
			{ STAGE.刷1杠1, new StageInfo(Mode.刷图, 85, null, 6) },
		};
		private static STAGE m_stage = STAGE.满级_初始四人;
	}
}
