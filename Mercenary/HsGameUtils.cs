using Assets;
using HarmonyLib;
using Hearthstone.DataModels;
using PegasusLettuce;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercenary
{
	public static class HsGameUtils
	{
		public static void CleanTask(int taskId)
		{
			VisitorTaskDbfRecord taskRecordByID = LettuceVillageDataUtil.GetTaskRecordByID(taskId);
			if (taskRecordByID == null)
			{
				return;
			}
			int mercenaryVisitorId = taskRecordByID.MercenaryVisitorId;
			Network.Get().DismissMercenaryTask(mercenaryVisitorId);
		}

		public static void GotoSceneMap()
		{
			SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_MAP, SceneMgr.TransitionHandlerType.NEXT_SCENE, null, null);
		}

		public static void GotoSceneVillage()
		{
			SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE, SceneMgr.TransitionHandlerType.SCENEMGR, null, null);
		}

		public static bool IsMysteryNode(uint nodeType)
		{
			return Array.IndexOf<uint>(new uint[]
			{
				0U,//未知
				14U,
				18U,//传送门
				19U,
				23U,
				44U
			}, nodeType) > -1;
		}

		public static bool IsBoss(uint nodeType)
		{
			return nodeType == 3U;
		}

		public static bool IsMonster(uint nodeType)
		{
			return Array.IndexOf<uint>(new uint[]
			{
				1U,//小怪
				2U,//精英
				3U,//boss
				22U//小怪
			}, nodeType) > -1;
		}

		//1 NONE MercenaryFight NORMAL_BOSS BOSS
		//2 NONE EliteFight ELITE_BOSS ELITE_BOSS
		//3 NONE FinalBoss FINAL_BOSS FINAL_BOSS
		//4 HEAL_TEAM Spirit Healer Full Team NONE HEALER
		//10 NONE TutorialFinalBoss FINAL_BOSS FINAL_BOSS
		//11 NONE Tutorial4 NONE BOSS
		//14 NONE Hot Potato NONE MYSTERY
		//18 SKIP_TO_FINAL_BOSS Portal NONE MYSTERY
		//19 REASSIGN_MAP_ROLE Sabotage NONE MYSTERY
		//20 VIEW_TASK_LIST Village Campfire NONE CAMPFIRE
		//22 NONE EcologyFight SIMPLE_BOSS BOSS
		//23 DISCOVER_VILLAGER Village Visitor NONE MYSTERY
		//26 NONE Boon: Protectors NONE OPPORTUNITY_PROTECTOR
		//30 NONE Boon: Fighters NONE OPPORTUNITY_FIGHTER
		//34 NONE Boon: Casters NONE OPPORTUNITY_CASTER
		//38 NONE Boon: Protectors NONE OPPORTUNITY_PROTECTOR
		//39 NONE Boon: Fighters NONE OPPORTUNITY_FIGHTER
		//40 NONE Boon: Casters NONE OPPORTUNITY_CASTER
		//41 NONE Boon: Protectors NONE OPPORTUNITY_PROTECTOR
		//42 NONE Boon: Fighters NONE OPPORTUNITY_FIGHTER
		//43 NONE Boon: Casters NONE OPPORTUNITY_CASTER
		//44 NONE Hot Potato Level 30 NONE MYSTERY
		//45 HEAL_TEAM Healer - One Random NONE HEALER
		//46 HEAL_TEAM Healer - Two Random NONE HEALER
		//47 HEAL_TEAM Healer - Three Random NONE HEALER
		//48 HEAL_TEAM Healer - Four Random NONE HEALER
		//49 HEAL_TEAM Healer - Five Random NONE HEALER
		//52 NONE Campaign 1 - Kazakus Bounty FINAL_BOSS ELITE_BOSS
		//56 NONE Campaign 1 - Plaguemaw the Rotting FINAL_BOSS BOSS
		//58 NONE Bonus Loot! NONE MYSTERY
		//59 NONE Cursed Treasure NONE MYSTERY
		//7 NONE Tutorial1 NONE BOSS
		//8 NONE Tutorial2 NONE BOSS
		//9 NONE Tutorial3 NONE BOSS

		//施法
		public static bool IsCaster(uint nodeType)
		{
			LettuceMapNodeTypeDbfRecord result = GameDbf.LettuceMapNodeType.GetRecord((int)nodeType);
			if (result == null)
				return false;
			// 			if (result.NodeVisualId == "OPPORTUNITY_CASTER")
			// 				Out.Log("IsCaster");
			return result.NodeVisualId == "OPPORTUNITY_CASTER";
		}

		//斗士
		public static bool IsFighter(uint nodeType)
		{
			LettuceMapNodeTypeDbfRecord result = GameDbf.LettuceMapNodeType.GetRecord((int)nodeType);
			if (result == null)
				return false;
			// 			if (result.NodeVisualId == "OPPORTUNITY_FIGHTER")
			// 				Out.Log("IsFighter");
			return result.NodeVisualId == "OPPORTUNITY_FIGHTER";
		}

		//护卫
		public static bool IsTank(uint nodeType)
		{
			LettuceMapNodeTypeDbfRecord result = GameDbf.LettuceMapNodeType.GetRecord((int)nodeType);
			if (result == null)
				return false;
			// 			if (result.NodeVisualId == "OPPORTUNITY_PROTECTOR")
			// 				Out.Log("IsTank");
			return result.NodeVisualId == "OPPORTUNITY_PROTECTOR";
		}

		public static bool IsDoctor(uint nodeType)
		{
			LettuceMapNodeTypeDbfRecord result = GameDbf.LettuceMapNodeType.GetRecord((int)nodeType);
			if (result == null)
				return false;
			// 			if (result.NodeVisualId == "HEALER")
			// 				Out.Log("IsDoctor");
			return result.NodeVisualId == "HEALER";
		}

		public static global::LettuceMercenary GetMercenary(int id)
		{
			return CollectionManager.Get().GetMercenary((long)id, false, true);
		}

		public static long CalcMercenaryCoinNeed(LettuceMercenary merc)
		{
			long num = 0L;
			if (!merc.m_owned)
			{
				num += (long)merc.GetCraftingCost();
			}
			foreach (LettuceAbility lettuceAbility in merc.m_abilityList)
			{
				for (int i = lettuceAbility.m_tier; i < lettuceAbility.m_tierList.Length; i++)
				{
					LettuceAbility.AbilityTier abilityTier = lettuceAbility.m_tierList[i];
					if (abilityTier.m_validTier)
					{
						num += (long)abilityTier.m_coinCost;
					}
				}
			}
			foreach (LettuceAbility lettuceAbility in merc.m_equipmentList)
			{
				for (int i = lettuceAbility.m_tier; i < lettuceAbility.m_tierList.Length; i++)
				{
					LettuceAbility.AbilityTier abilityTier = lettuceAbility.m_tierList[i];
					if (abilityTier.m_validTier)
					{
						num += (long)abilityTier.m_coinCost;
					}
				}
			}
			num -= merc.m_currencyAmount;
			return Math.Max(0, num);
		}

		public static void SelectBoss(int mapId)
		{
			LettuceVillageDisplay.LettuceSceneTransitionPayload lettuceSceneTransitionPayload = new LettuceVillageDisplay.LettuceSceneTransitionPayload();
			LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(mapId);
			lettuceSceneTransitionPayload.m_SelectedBounty = record;
			lettuceSceneTransitionPayload.m_SelectedBountySet = record.BountySetRecord;
			lettuceSceneTransitionPayload.m_DifficultyMode = record.DifficultyMode;
			SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT, SceneMgr.TransitionHandlerType.CURRENT_SCENE, null, lettuceSceneTransitionPayload);
		}

		public static void UpdateAllSkill()
		{
			foreach (global::LettuceMercenary lettuceMercenary in CollectionManager.Get().FindMercenaries(null, new bool?(true), null, null, null).m_mercenaries)
			{
				if (!lettuceMercenary.IsReadyForCrafting())
				{
					foreach (LettuceAbility ability in lettuceMercenary.m_abilityList)
					{
						HsGameUtils.UpgradeSkill(lettuceMercenary, ability);
					}
					foreach (LettuceAbility ability2 in lettuceMercenary.m_equipmentList)
					{
						HsGameUtils.UpgradeSkill(lettuceMercenary, ability2);
					}
				}
			}
			Network.Get().MercenariesCollectionRequest();
		}

		public static List<global::LettuceTeam> GetAllTeams()
		{
			return CollectionManager.Get().GetTeams();
		}

		public static List<Task> GetMercTasks()
		{
			NetCache.NetCacheMercenariesVillageVisitorInfo netObject = NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
			List<Task> list = new List<Task>();
			List<MercenaryVillageTaskItemDataModel> list_alltasks = new List<MercenaryVillageTaskItemDataModel>();
			foreach (var iter in netObject.VisitorStates)
			{
				MercenaryVillageTaskItemDataModel mercenaryVillageTaskItemDataModel = LettuceVillageDataUtil.CreateTaskModelFromTaskState(iter.ActiveTaskState, null);
				if (mercenaryVillageTaskItemDataModel.TaskType != MercenaryVisitor.VillageVisitorType.STANDARD)
					continue;

				list_alltasks.Add(mercenaryVillageTaskItemDataModel);
			}
			// 排个序，不然每次netchache都不一样
			foreach (var mercenaryVillageTaskItemDataModel in from x in list_alltasks orderby x.MercenaryId ascending select x)
			{
				if (Main.modeConf.Value == Mode.一条龙.ToString())
				{
					if (OnePackageService.Stage == OnePackageService.STAGE.获得_大德装备3 && mercenaryVillageTaskItemDataModel.MercenaryId != MercConst.玛法里奥_怒风 ||
						OnePackageService.Stage == OnePackageService.STAGE.获得_拉格装备3 && mercenaryVillageTaskItemDataModel.MercenaryId != MercConst.拉格纳罗斯 ||
						OnePackageService.Stage == OnePackageService.STAGE.获得_迦顿装备2 && mercenaryVillageTaskItemDataModel.MercenaryId != MercConst.迦顿男爵)
					{
						continue;
					}
				}
				if (mercenaryVillageTaskItemDataModel.MercenaryId == MercConst.泰瑞尔)
				{
					bool skip = false;
					foreach (var iterMerc in DefaultTeam.IceFire.Member.TeamInfo)
					{
						if (!HsGameUtils.GetMercenary(iterMerc.id).m_owned)
							skip = true;
					}
					if (skip == true)
						continue;
				}

				VisitorTaskDbfRecord taskRecordByID = LettuceVillageDataUtil.GetTaskRecordByID(mercenaryVillageTaskItemDataModel.TaskId);
				TaskAdapter.SetTask(mercenaryVillageTaskItemDataModel.TaskId, mercenaryVillageTaskItemDataModel.MercenaryId, mercenaryVillageTaskItemDataModel.Title,
					taskRecordByID.TaskDescription.GetString(Locale.zhCN), list, mercenaryVillageTaskItemDataModel.ProgressMessage);
				if (list.Sum(x => x.Mercenaries.Distinct(new MercenaryEntityComparer()).Count()) >= 6)
					break;
			}
			return list;
		}

		public static List<Task> GetMainLineTask()
		{
			List<Task> list = new List<Task>();
			foreach (MercenariesVisitorState mercenariesVisitorState in NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>().VisitorStates)
			{
				MercenaryVillageTaskItemDataModel mercenaryVillageTaskItemDataModel = LettuceVillageDataUtil.CreateTaskModelFromTaskState(mercenariesVisitorState.ActiveTaskState, null);
				if (mercenaryVillageTaskItemDataModel.TaskType == MercenaryVisitor.VillageVisitorType.SPECIAL)
				{
					TaskAdapter.SetMainLineTask(list, mercenaryVillageTaskItemDataModel.Description);
					break;
				}
			}
			return list;
		}

		public static void UpdateEq(int mercenaryId, int equipmentIndex)
		{
			global::LettuceMercenary mercenary = CollectionManager.Get().GetMercenary((long)mercenaryId, false, true);
			if (mercenary == null || !mercenary.m_owned)
			{
				return;
			}
			LettuceAbility lettuceAbility = mercenary.m_equipmentList[equipmentIndex];
			if (lettuceAbility == null || !lettuceAbility.Owned)
			{
				return;
			}
			mercenary.SlotEquipment(lettuceAbility.ID);
			if (mercenary.m_equipmentSelectionChanged)
			{
				CollectionManager.Get().SendEquippedMercenaryEquipment(mercenary.ID);
			}
		}

		public static bool HasIdleTask()
		{
			int currentTierPropertyForBuilding = LettuceVillageDataUtil.GetCurrentTierPropertyForBuilding(MercenaryBuilding.Mercenarybuildingtype.TASKBOARD, TierProperties.Buildingtierproperty.TASKSLOTS, null);
			return 2 + currentTierPropertyForBuilding - LettuceVillageDataUtil.VisitorStates.Count > 0;
		}

		private static void UpgradeSkill(global::LettuceMercenary mrc, LettuceAbility ability)
		{
			global::LettuceMercenary mercenary = CollectionManager.Get().GetMercenary((long)mrc.ID, false, true);
			if ((bool)Traverse.Create(mercenary).Method("IsLettuceAbilityUpgradeable", new object[]
			{
				ability
			}).GetValue())
			{
				if (ability.m_cardType == CollectionUtils.MercenariesModeCardType.Ability)
				{
					Out.Log(string.Format("[升级技能] [MID:{0}][SID:{1}]", mercenary.ID, ability.ID));
					Network.Get().UpgradeMercenaryAbility(mercenary.ID, ability.ID);
					return;
				}
				Out.Log(string.Format("[升级技能] [MID:{0}][EID:{1}]", mercenary.ID, ability.ID));
				Network.Get().UpgradeMercenaryEquipment(mercenary.ID, ability.ID);
			}
		}
	}
}