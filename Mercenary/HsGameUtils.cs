using System;
using System.Collections.Generic;
using Assets;
using HarmonyLib;
using PegasusLettuce;

namespace Mercenary
{
	// Token: 0x02000007 RID: 7
	public static class HsGameUtils
	{
		// Token: 0x06000033 RID: 51 RVA: 0x0000418C File Offset: 0x0000238C
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

		// Token: 0x06000034 RID: 52 RVA: 0x000041B6 File Offset: 0x000023B6
		public static void GotoSceneMap()
		{
			SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_MAP, SceneMgr.TransitionHandlerType.NEXT_SCENE, null, null);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000041C7 File Offset: 0x000023C7
		public static void GotoSceneVillage()
		{
			SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE, SceneMgr.TransitionHandlerType.SCENEMGR, null, null);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000041D8 File Offset: 0x000023D8
		public static bool IsMysteryNode(uint nodeType)
		{
			return Array.IndexOf<uint>(new uint[]
			{
				0U,
				14U,
				18U,
				19U,
				23U,
				44U
			}, nodeType) > -1;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000041F4 File Offset: 0x000023F4
		public static bool IsJumpNode(uint nodeType)
		{
			return nodeType == 18U;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000041FB File Offset: 0x000023FB
		public static bool IsBoss(uint nodeType)
		{
			return nodeType == 3U;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004201 File Offset: 0x00002401
		public static bool IsMonster(uint nodeType)
		{
			return Array.IndexOf<uint>(new uint[]
			{
				1U,
				2U,
				3U,
				22U
			}, nodeType) > -1;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000421D File Offset: 0x0000241D
		public static global::LettuceMercenary GetMercenary(int id)
		{
			return CollectionManager.Get().GetMercenary((long)id, false, true);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004230 File Offset: 0x00002430
		public static void SelectBoss(int mapId)
		{
			LettuceVillageDisplay.LettuceSceneTransitionPayload lettuceSceneTransitionPayload = new LettuceVillageDisplay.LettuceSceneTransitionPayload();
			LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(mapId);
			lettuceSceneTransitionPayload.m_SelectedBounty = record;
			lettuceSceneTransitionPayload.m_SelectedBountySet = record.BountySetRecord;
			lettuceSceneTransitionPayload.m_IsHeroic = record.Heroic;
			SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT, SceneMgr.TransitionHandlerType.CURRENT_SCENE, null, lettuceSceneTransitionPayload);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004280 File Offset: 0x00002480
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

		// Token: 0x0600003D RID: 61 RVA: 0x00004390 File Offset: 0x00002590
		public static List<global::LettuceTeam> GetAllTeams()
		{
			return CollectionManager.Get().GetTeams();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000439C File Offset: 0x0000259C
		public static List<Task> GetTasks()
		{
			NetCache.NetCacheMercenariesVillageVisitorInfo netObject = NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
			List<Task> list = new List<Task>();
			// 因为新任务是添加在第一条，所以倒序做任务，先进先做
			for (int i = netObject.VisitorStates.Count; i>1; --i)
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

		// Token: 0x0600003F RID: 63 RVA: 0x00004434 File Offset: 0x00002634
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

		// Token: 0x06000040 RID: 64 RVA: 0x0000449C File Offset: 0x0000269C
		public static bool HasIdleTask()
		{
			int currentTierPropertyForBuilding = LettuceVillageDataUtil.GetCurrentTierPropertyForBuilding(MercenaryBuilding.Mercenarybuildingtype.TASKBOARD, TierProperties.Buildingtierproperty.TASKSLOTS, null);
			return 2 + currentTierPropertyForBuilding - LettuceVillageDataUtil.VisitorStates.Count > 0;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000044C4 File Offset: 0x000026C4
		private static void SetTask(VisitorTaskDbfRecord task, List<Task> tasks)
		{
			MercenaryVisitorDbfRecord visitorRecordByID = LettuceVillageDataUtil.GetVisitorRecordByID(task.MercenaryVisitorId);
			TaskAdapter.SetTask(task.ID, visitorRecordByID.MercenaryId, task.TaskTitle.GetString(true), task.TaskDescription.GetString(true), tasks);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004508 File Offset: 0x00002708
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
