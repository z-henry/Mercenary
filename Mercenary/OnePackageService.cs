using Assets;
using Hearthstone.DataModels;
using Mercenary.DefaultTeam;
using System;
using System.Collections.Generic;

namespace Mercenary
{
	public class StageInfo
	{
		public StageInfo(Mode mode, int mapid, List<Type> teamTypes, int teamtotal = 6, int targetCoinNeeded = -1)
		{
			m_mode = mode;
			m_mapid = mapid;
			m_teamTypes = teamTypes;
			m_teamTotal = teamtotal;
			m_TargetCoinNeeded = targetCoinNeeded;
		}

		public Mode m_mode;
		public int m_mapid;
		public List<Type> m_teamTypes;
		public int m_teamTotal;
		public int m_TargetCoinNeeded;// 对于预设队伍，小于此硬币需求的佣兵，不再携带。默认-1必须携带
	}

	public static class OnePackageService
	{
		public enum STAGE
		{
			UNINIT = -1,
			满级_初始四人 = 0,
			获得_萨穆罗,
			满级_初始五人,
			解锁_2杠6,
			获得_AOE队,
			刷满_AOE,
			获得_大德,
			获得_自然队,
			任务_大德装备3,
			刷满_自然队,
			解锁_3杠6,
			获得_拉格,
			获得_迦顿,
			获得_安东尼,
			任务_迦顿装备2,
			任务_拉格装备3,
			刷满_小火焰队,
			获得_紫色配合,
			任务_除泰瑞尔,
			获得_预设卡组,
			刷满_预设卡组,
			解锁_5杠5,
			获得_冰火装备,
			获得_全佣兵,
			任务_泰瑞尔,
			解锁_主线,
			解锁_全装备,
			刷满_全佣兵,
			广积粮,
		}

		public static STAGE Stage
		{
			get
			{
				if (false == m_hasInit)
					m_stage = ClacStage();
				return m_stage;
			}
			private set { m_stage = value; }
		}

		public static void UpdateStage()
		{
			m_stage = ClacStage();
			Out.Log($"[一条龙阶段] 阶段{(int)Stage} {Stage}");
		}

		private static STAGE ClacStage()
		{
			m_hasInit = true;
			try
			{
				//初始队伍，凯瑞尔，豪斯，泰兰德，泽瑞拉，4个人，是否全部30级
				foreach (var iter in TeamManager.GetTeam(typeof(Origin0)).GetMember())
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.IsReadyForCrafting()
						&& mercenary.m_owned
						&& !mercenary.IsMaxLevel())
					{
						return STAGE.满级_初始四人;
					}
				}

				//剑圣是否拥有
				LettuceMercenary mercenary_temp = HsGameUtils.GetMercenary(MercConst.剑圣萨穆罗);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_萨穆罗;
				}

				//初始队伍，豪斯，泽瑞拉，剑圣，泰兰德，凯瑞尔， 5个人，是否30级
				foreach (var iter in TeamManager.GetTeam(typeof(Origin)).GetMember())
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.IsReadyForCrafting()
						&& mercenary.m_owned
						&& !mercenary.IsMaxLevel())
					{
						return STAGE.满级_初始五人;
					}
				}

				//2-6 是否解锁
				if (false == MercenariesDataUtil.IsBountyComplete(72))
				{
					return STAGE.解锁_2杠6;
				}

				//AOE初级队，是否获得，雪王，晨拥，米尔豪斯
				foreach (var iter in TeamManager.GetTeam(typeof(AOE)).GetMember())
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.m_owned)
					{
						return STAGE.获得_AOE队;
					}
				}

				//AOE初级队， 是否碎片够满级  雪王，晨拥，米尔豪斯
				foreach (var iter in TeamManager.GetTeam(typeof(AOE)).GetMember())
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 2000)
					{
						return STAGE.刷满_AOE;
					}
				}
				//大德是否获得
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.玛法里奥_怒风);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_大德;
				}

				//自然队是否全拥有
				foreach (var iter in TeamManager.GetTeam(typeof(Nature)).GetMember())
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (!mercenary.m_owned)
					{
						return STAGE.获得_自然队;
					}
				}

				//玛法里奥是否获得了 装备3
				LettuceAbility lettuceAbility = mercenary_temp.m_equipmentList[2];
				if (!lettuceAbility.Owned)
				{
					return STAGE.任务_大德装备3;
				}

				//自然队两人以上差1000碎片
				int sum = 1;
				foreach (var iter in TeamManager.GetTeam(typeof(Nature)).GetMember())
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 1000)
					{
						if (sum++ > 1)
						{
							return STAGE.刷满_自然队;
						}
					}
				}

				//3 - 6 是否解锁
				if (false == MercenariesDataUtil.IsBountyComplete(78))
				{
					return STAGE.解锁_3杠6;
				}

				//是否有初级火焰队
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.拉格纳罗斯);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_拉格;
				}
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.迦顿男爵);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_迦顿;
				}
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.安东尼达斯);
				if (!mercenary_temp.m_owned)
				{
					return STAGE.获得_安东尼;
				}

				//迦顿是否获得了 装备2
				if (!HsGameUtils.GetMercenary(MercConst.迦顿男爵).m_equipmentList[1].Owned)
				{
					return STAGE.任务_迦顿装备2;
				}

				//拉格纳罗斯是否获得了 装备3
				if (!HsGameUtils.GetMercenary(MercConst.拉格纳罗斯).m_equipmentList[2].Owned)
				{
					return STAGE.任务_拉格装备3;
				}

				//初级火焰队差1000点以上的碎片
				foreach (var iter in TeamManager.GetTeam(typeof(PrimaryFire)).GetMember())
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(iter.id);
					if (mercenary.m_owned &&
						HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 1000)
					{
						return STAGE.刷满_小火焰队;
					}
				}

				//配合紫色佣兵没刷到的多与8个
				sum = 1;
				foreach (int ID in MercConst.CoopTools)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(ID);
					if (!mercenary.m_owned)
					{
						if (sum++ > 8)
							return STAGE.获得_紫色配合;
					}
				}

				//当前的佣兵任务栏，是不是空的
				NetCache.NetCacheMercenariesVillageVisitorInfo netObject =
					NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
				foreach (var iter in netObject.VisitorStates)
				{
					MercenaryVillageTaskItemDataModel mercenaryVillageTaskItemDataModel = LettuceVillageDataUtil.CreateTaskModelFromTaskState(iter.ActiveTaskState, null);
					if (mercenaryVillageTaskItemDataModel.TaskType == MercenaryVisitor.VillageVisitorType.STANDARD)
					{
						if (mercenaryVillageTaskItemDataModel.MercenaryId == MercConst.泰瑞尔)
							continue;

						return STAGE.任务_除泰瑞尔;
					}
				}

				//预设卡组的所有佣兵是否获得
				foreach (var iterType in m_DefaultTeam)
				{
					foreach (var iterMerc in TeamManager.GetTeam(iterType).GetMember())
					{
						LettuceMercenary mercenary = HsGameUtils.GetMercenary(iterMerc.id);
						if (!mercenary.m_owned)
						{
							return STAGE.获得_预设卡组;
						}
					}
				}
				//预设卡组的所有佣兵是否全部碎片够+1+5
				foreach (var iterType in m_DefaultTeam)
				{
					foreach (var iterMerc in TeamManager.GetTeam(iterType).GetMember())
					{
						LettuceMercenary mercenary = HsGameUtils.GetMercenary(iterMerc.id);
						if (!mercenary.m_owned || HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 0)
						{
							return STAGE.刷满_预设卡组;
						}
					}
				}

				//冰火队的两个解锁装备的地图是否解锁128
				if (false == MercenariesDataUtil.IsBountyComplete(128))
				{
					return STAGE.解锁_5杠5;
				}

				//巴琳达 雪王 是否解锁装备
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.冰雪之王洛克霍拉);
				lettuceAbility = mercenary_temp.m_equipmentList[0];
				if (!lettuceAbility.Owned)
				{
					return STAGE.获得_冰火装备;
				}
				mercenary_temp = HsGameUtils.GetMercenary(MercConst.巴琳达_斯通赫尔斯);
				lettuceAbility = mercenary_temp.m_equipmentList[1];
				if (!lettuceAbility.Owned)
				{
					return STAGE.获得_冰火装备;
				}


				//是否全佣兵
				if (CollectionManager.Get().FindMercenaries(isOwned: false, isCraftable: false).m_mercenaries.Count > 0)
				{
					return STAGE.获得_全佣兵;
				}

				//做泰瑞尔任务
				foreach (var iter in netObject.VisitorStates)
				{
					MercenaryVillageTaskItemDataModel mercenaryVillageTaskItemDataModel = LettuceVillageDataUtil.CreateTaskModelFromTaskState(iter.ActiveTaskState, null);
					if (mercenaryVillageTaskItemDataModel.TaskType == MercenaryVisitor.VillageVisitorType.STANDARD)
					{
						if (mercenaryVillageTaskItemDataModel.MercenaryId == MercConst.泰瑞尔)
							return STAGE.任务_泰瑞尔;

					}
				}

				//是否完成主线任务
				if (HsGameUtils.GetMainLineTask().Count > 0)
				{
					return STAGE.解锁_主线;
				}

				//是否有佣兵未解锁装备
				if (Main.UpdateAutoUnlockEquipInfo() == true)
				{
					return STAGE.解锁_全装备;
				}

				//佣兵全满
				foreach (LettuceMercenary mercenary in CollectionManager.Get().FindMercenaries(isOwned: true).m_mercenaries)
				{
					if (HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 0)
					{
						return STAGE.刷满_全佣兵;
					}
				}
				//3火焰队刷图2-6
				return STAGE.广积粮;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			m_hasInit = false;
			return STAGE.UNINIT;
		}

		public static StageInfo TranslateCurrentStage()
		{
			if (false == m_hasInit)
				m_stage = ClacStage();
			return m_dictTranslate[m_stage];
		}

		private static Dictionary<STAGE, StageInfo> m_dictTranslate = new Dictionary<STAGE, StageInfo>() {
			{ STAGE.满级_初始四人, new StageInfo(Mode.刷图, 85, new List<Type> (){typeof(DefaultTeam.Origin0)}) },
			{ STAGE.获得_萨穆罗, new StageInfo(Mode.主线任务, -1, new List<Type> (){typeof(DefaultTeam.Origin0)}) },
			{ STAGE.满级_初始五人, new StageInfo(Mode.刷图, 85, new List<Type> (){typeof(DefaultTeam.Origin)}) },
			{ STAGE.解锁_2杠6, new StageInfo(Mode.主线任务, -1, new List<Type> (){typeof(DefaultTeam.Origin)}) },
			{ STAGE.获得_AOE队, new StageInfo(Mode.神秘人, 72, new List<Type> (){typeof(DefaultTeam.Origin)}) },
			{ STAGE.刷满_AOE, new StageInfo(Mode.刷图, 85, new List<Type> (){typeof(DefaultTeam.AOE)}, targetCoinNeeded:2000) },
			{ STAGE.获得_大德, new StageInfo(Mode.刷图, 69, new List<Type> (){typeof(DefaultTeam.AOE)}) },
			{ STAGE.获得_自然队, new StageInfo(Mode.神秘人, 72, new List<Type> (){typeof(DefaultTeam.AOE)}) },
			{ STAGE.任务_大德装备3, new StageInfo(Mode.佣兵任务, -1, new List<Type> (){typeof(DefaultTeam.DruidsExclusive) }, teamtotal:3) },
			{ STAGE.刷满_自然队, new StageInfo(Mode.刷图, 85, new List<Type> (){typeof(DefaultTeam.Nature)}, targetCoinNeeded:1000) },
			{ STAGE.解锁_3杠6, new StageInfo(Mode.主线任务, -1, new List<Type> (){typeof(DefaultTeam.Nature)}) },
			{ STAGE.获得_拉格, new StageInfo(Mode.刷图, 75, new List<Type> (){typeof(DefaultTeam.Nature)}) },
			{ STAGE.获得_迦顿, new StageInfo(Mode.刷图, 74, new List<Type> (){typeof(DefaultTeam.Nature)}) },
			{ STAGE.获得_安东尼, new StageInfo(Mode.刷图, 76, new List<Type> (){typeof(DefaultTeam.Nature)}) },
			{ STAGE.任务_迦顿装备2, new StageInfo(Mode.佣兵任务, -1, new List<Type> (){typeof(DefaultTeam.PrimaryFire) }, teamtotal:3) },
			{ STAGE.任务_拉格装备3, new StageInfo(Mode.佣兵任务, -1, new List<Type> (){typeof(DefaultTeam.PrimaryFire) }, teamtotal:3) },
			{ STAGE.刷满_小火焰队, new StageInfo(Mode.刷图, 85, new List<Type> (){typeof(DefaultTeam.PrimaryFire)}, targetCoinNeeded:1000, teamtotal:3) },
			{ STAGE.获得_紫色配合, new StageInfo(Mode.神秘人, 72, new List<Type> (){typeof(DefaultTeam.PrimaryFire)}) },
			{ STAGE.任务_除泰瑞尔, new StageInfo(Mode.佣兵任务, -1, null) },
			{ STAGE.获得_预设卡组, new StageInfo(Mode.神秘人, 72, new List<Type> (){typeof(DefaultTeam.PrimaryFire)}) },
			{ STAGE.刷满_预设卡组, new StageInfo(Mode.刷图, 85, m_DefaultTeam, targetCoinNeeded:0) },
			{ STAGE.解锁_5杠5, new StageInfo(Mode.主线任务, -1, null) },
			{ STAGE.获得_冰火装备, new StageInfo(Mode.解锁装备, -1, null) },
			{ STAGE.获得_全佣兵, new StageInfo(Mode.神秘人, 72, new List<Type> (){typeof(DefaultTeam.IceFire) }, teamtotal:3) },
			{ STAGE.任务_泰瑞尔, new StageInfo(Mode.佣兵任务, -1, null) },
			{ STAGE.解锁_主线, new StageInfo(Mode.主线任务, -1, null) },
			{ STAGE.解锁_全装备, new StageInfo(Mode.解锁装备, -1, null) },
			{ STAGE.刷满_全佣兵, new StageInfo(Mode.刷图, 85, null) },
			{ STAGE.广积粮, new StageInfo(Mode.神秘人, 72, new List<Type> (){typeof(DefaultTeam.IceFire) }, teamtotal:3) },
		};

		private static STAGE m_stage = STAGE.满级_初始四人;
		private static bool m_hasInit = false;

		private static List<Type> m_DefaultTeam = new List<Type> {
			typeof(DefaultTeam.Ice), typeof(DefaultTeam.IceFire), typeof(DefaultTeam.PirateSnake),
			typeof(DefaultTeam.FireKill), typeof(DefaultTeam.Nature)
		};
	}
}