using System;
using System.Collections.Generic;
using Assets;
using HarmonyLib;
using PegasusLettuce;

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

		public static bool IsUnknownMysteryNode(uint nodeType)
		{
			return Array.IndexOf<uint>(new uint[]
			{
				0U//未知
			}, nodeType) > -1;
		}


		
		public static bool IsJumpNode(uint nodeType)
		{
			return nodeType == 18U;
		}

		
		public static bool IsBoss(uint nodeType)
		{
			return nodeType == 3U;
		}

		
		public static bool IsMonster(uint nodeType)
		{
			return Array.IndexOf<uint>(new uint[]
			{
				1U,//蓝
				2U,
				3U,//boss
				22U//红
			}, nodeType) > -1;
		}

		public static bool IsSep(uint nodeType)
		{
			return Array.IndexOf<uint>(new uint[]
			{
				45U,//医者
				30U,//斗士
			}, nodeType) > -1;
		}

		
		public static global::LettuceMercenary GetMercenary(int id)
		{
			return CollectionManager.Get().GetMercenary((long)id, false, true);
		}

		
		public static void SelectBoss(int mapId)
		{
			LettuceVillageDisplay.LettuceSceneTransitionPayload lettuceSceneTransitionPayload = new LettuceVillageDisplay.LettuceSceneTransitionPayload();
			LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(mapId);
			lettuceSceneTransitionPayload.m_SelectedBounty = record;
			lettuceSceneTransitionPayload.m_SelectedBountySet = record.BountySetRecord;
			lettuceSceneTransitionPayload.m_IsHeroic = record.Heroic;
			SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT, SceneMgr.TransitionHandlerType.CURRENT_SCENE, null, lettuceSceneTransitionPayload);
		}

		
		public static void UpdateAllSkill()
		{
			foreach (global::LettuceMercenary lettuceMercenary in CollectionManager.Get().FindOrderedMercenaries(null, new bool?(true), null, null, null).m_mercenaries)
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
		}

		
		public static List<global::LettuceTeam> GetAllTeams()
		{
			return CollectionManager.Get().GetTeams();
		}

		
		public static List<Task> GetTasks()
		{
			NetCache.NetCacheMercenariesVillageVisitorInfo netObject = NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
			List<Task> list = new List<Task>();
			// 因为新任务是添加在第一条，所以倒序做任务，先进先做
			for (int i = netObject.VisitorStates.Count; i>0; --i)
// 			foreach (MercenariesVisitorState mercenariesVisitorState in netObject.VisitorStates)
			{
				VisitorTaskDbfRecord taskRecordByID = LettuceVillageDataUtil.GetTaskRecordByID(netObject.VisitorStates[i-1].ActiveTaskState.TaskId);
				if ((MercenaryVisitor.VillageVisitorType)Traverse.Create(LettuceVillageDataUtil.GetVisitorRecordByID(taskRecordByID.MercenaryVisitorId)).Field("m_visitorType").GetValue() == MercenaryVisitor.VillageVisitorType.STANDARD)
				{
					HsGameUtils.SetTask(taskRecordByID, list);
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

		
		private static void SetTask(VisitorTaskDbfRecord task, List<Task> tasks)
		{
			MercenaryVisitorDbfRecord visitorRecordByID = LettuceVillageDataUtil.GetVisitorRecordByID(task.MercenaryVisitorId);
			TaskAdapter.SetTask(task.ID, visitorRecordByID.MercenaryId, task.TaskTitle.GetString(true), task.TaskDescription.GetString(true), tasks);
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
