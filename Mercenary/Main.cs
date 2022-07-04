using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Hearthstone;
using Hearthstone.Progression;
using HsMercenaryStrategy;
using PegasusLettuce;
using PegasusShared;
using PegasusUtil;
using UnityEngine;

namespace Mercenary
{
	// Token: 0x02000004 RID: 4
	[BepInPlugin("io.github.jimowushuang.hs", "佣兵挂机插件[改]", "3.0.1")]
	public class Main : BaseUnityPlugin
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002274 File Offset: 0x00000474
		private void OnGUI()
		{
			if (!Main.isRunning)
			{
				return;
			}
			GUILayout.Label(new GUIContent("插件版本[改] 3.0.1"), new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022D0 File Offset: 0x000004D0
		private void Awake()
		{
			this._harmony.PatchAll(typeof(Main));
			Main.modeConf = base.Config.Bind<string>("配置", "插件运行模式", "刷图", new ConfigDescription("插件运行模式", new AcceptableValueList<string>(new string[]
			{
				"刷图",
				"刷神秘人",
				"全自动接任务做任务",
				"自动解锁地图",
				"Pvp",
				"挂机收菜"
			}), Array.Empty<object>()));
			Main.teamNameConf = base.Config.Bind<string>("配置", "使用的队伍名称", "PVE", "使用的队伍名称");
			Main.strategyConf = base.Config.Bind<string>("配置", "战斗策略", FireStrategy.StrategyName, new ConfigDescription("使用的策略,注意只有在非全自动化模式下才会生效", new AcceptableValueList<string>(StrategyHelper.GetAllStrategiesName().ToArray()), Array.Empty<object>()));
			Main.mapConf = base.Config.Bind<string>(new ConfigDefinition("配置", "要刷的地图"), "2-5", new ConfigDescription("要刷的地图", new AcceptableValueList<string>(MapUtils.GetMapNameList()), Array.Empty<object>()));
			Main.autoUpdateSkillConf = base.Config.Bind<bool>("配置", "是否自动升级技能", true, "是否自动升级技能");
			Main.autoCraftConf = base.Config.Bind<bool>("配置", "是否自动制作佣兵", true, "是否自动制作佣兵");
			Main.teamNumConf = base.Config.Bind<int>("配置", "总队伍人数", 6, new ConfigDescription("总队伍人数（PVE下生效）", new AcceptableValueRange<int>(1, 6), Array.Empty<object>()));
			Main.coreTeamNumConf = base.Config.Bind<int>("配置", "队伍核心人数", 0, new ConfigDescription("前n个佣兵不会被自动换掉（PVE下生效）", new AcceptableValueRange<int>(0, 6), Array.Empty<object>()));
			Main.runningConf = base.Config.Bind<bool>("配置", "插件开关", true, new ConfigDescription("插件开关", null, new object[]
			{
				"Advanced"
			}));
			Main.cleanTaskConf = base.Config.Bind<string>(new ConfigDefinition("配置", "自动清理任务时间"), "不开启", new ConfigDescription("会定时清理长时间没完成的任务（全自动模式生效）", new AcceptableValueList<string>(new List<string>(TaskUtils.CleanConf.Keys).ToArray()), Array.Empty<object>()));
			Main.awakeTimeConf = base.Config.Bind<string>("配置", "唤醒时间", "1999/1/1 0:0:0", "挂机收菜下的唤醒时间（无需更改）");
			Main.awakeTimeIntervalConf = base.Config.Bind<int>("配置", "唤醒时间间隔", 22, "挂机收菜下的唤醒时间间隔");

			
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002513 File Offset: 0x00000713
		private void Start()
		{
			Out.Log("启动");
			Main.isRunning = Main.runningConf.Value;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002534 File Offset: 0x00000734
		[HarmonyPrefix]
		[HarmonyPatch(typeof(RewardTrackManager), "UpdateStatus")]
		public static bool _PreUpdateStatus(int rewardTrackId, int level, RewardTrackManager.RewardStatus status, bool forPaidTrack, List<RewardItemOutput> rewardItemOutput)
		{
			if (!Main.isRunning || status != RewardTrackManager.RewardStatus.GRANTED)
			{
				return true;
			}
			RewardTrackManager.Get().AckRewardTrackReward(rewardTrackId, level, forPaidTrack);
			return false;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002552 File Offset: 0x00000752
		[HarmonyPrefix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "OnVisitorSelectionResponseReceived")]
		public static bool _PreOnVisitorSelectionResponseReceived()
		{
			if (!Main.isRunning)
			{
				return true;
			}
			Network.Get().GetMercenariesMapVisitorSelectionResponse();
			return false;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002569 File Offset: 0x00000769
		[HarmonyPrefix]
		[HarmonyPatch(typeof(SplashScreen), "GetRatingsScreenRegion")]
		[HarmonyPatch(typeof(QuestPopups), "ShowNextQuestNotification")]
		[HarmonyPatch(typeof(EnemyEmoteHandler), "IsSquelched")]
		[HarmonyPatch(typeof(EndGameScreen), "ShowMercenariesExperienceRewards")]
		public static bool _PreIntercept()
		{
			return !Main.isRunning;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002569 File Offset: 0x00000769
		[HarmonyPrefix]
		[HarmonyPatch(typeof(HearthstoneApplication), "OnApplicationFocus")]
		public static bool _PreFocus(bool focus)
		{
			return !Main.isRunning;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002575 File Offset: 0x00000775
		[HarmonyPostfix]
		[HarmonyPatch(typeof(LettuceMissionEntity), "ShiftPlayZoneForGamePhase")]
		public static void _PostShiftPlayZoneForGamePhase(int phase)
		{
// 			Out.Log("_PostShiftPlayZoneForGamePhase phase " + phase.ToString());
			Main.phaseID = phase;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002593 File Offset: 0x00000793
		[HarmonyPrefix]
		[HarmonyPatch(typeof(DialogManager), "ShowReconnectHelperDialog")]
		[HarmonyPatch(typeof(ReconnectHelperDialog), "Show")]
		[HarmonyPatch(typeof(Network), "OnFatalBnetError")]
		public static bool _PreError()
		{
			Out.Log("_PreError");
			if (!Main.isRunning)
			{
				return true;
			}
			Application.Quit();
			return false;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000025AE File Offset: 0x000007AE
		[HarmonyPrefix]
		[HarmonyPatch(typeof(GraphicsResolution), "IsAspectRatioWithinLimit")]
		public static bool _PreIsAspectRatioWithinLimit(ref bool __result, int width, int height, bool isWindowedMode)
		{
			if (!Main.isRunning)
			{
				return true;
			}
			__result = true;
			return false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025BD File Offset: 0x000007BD
		[HarmonyPrefix]
		[HarmonyPatch(typeof(AlertPopup), "Show")]
		public static bool _PreAlertPopupShow()
		{
			Out.Log("AlertPopup.Show");
			return !Main.isRunning;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025D1 File Offset: 0x000007D1
		[HarmonyPostfix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "ShouldShowVisitorSelection")]
		public static void PostShouldShowVisitorSelection(PegasusLettuce.LettuceMap map, ref bool __result)
		{
			if (!Main.isRunning || map == null)
			{
				return;
			}
			__result = false;
			if (map.HasPendingVisitorSelection && map.PendingVisitorSelection.VisitorOptions.Count > 0)
			{
				Network.Get().MakeMercenariesMapVisitorSelection(0);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002607 File Offset: 0x00000807
		[HarmonyPrefix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "DisplayNewlyGrantedAnomalyCards")]
		public static bool _PreDisplayNewlyGrantedAnomalyCards(global::LettuceMap lettuceMap, int completedNodeId)
		{
			Out.Log("_PreDisplayNewlyGrantedAnomalyCards");
			return !Main.isRunning;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000261C File Offset: 0x0000081C
		[HarmonyPostfix]
		[HarmonyPatch(typeof(global::LettuceMap), "CreateMapFromProto")]
		public static void _PostCreateMapFromProto(PegasusLettuce.LettuceMap lettuceMap)
		{
			Out.Log("_PostCreateMapFromProto");
			if (!Main.isRunning || lettuceMap == null)
			{
				return;
			}
			NetCache.NetCacheMercenariesVillageVisitorInfo netObject = NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
			if (netObject != null)
			{
				foreach (MercenariesVisitorState mercenariesVisitorState in netObject.VisitorStates)
				{
					if (mercenariesVisitorState.ActiveTaskState.Status_ == MercenariesTaskState.Status.COMPLETE)
					{
						Network.Get().ClaimMercenaryTask(mercenariesVisitorState.ActiveTaskState.TaskId);
					}
				}
			}
			foreach (LettuceMapNode lettuceMapNode in lettuceMap.Nodes)
			{
				if (GameUtils.IsFinalBossNodeType((int)lettuceMapNode.NodeTypeId) && lettuceMapNode.NodeState_ == LettuceMapNode.NodeState.COMPLETE)
				{
					SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_BOUNTY_BOARD, SceneMgr.TransitionHandlerType.NEXT_SCENE, null, null);
					return;
				}
			}
			if (Main.IsTreasure(lettuceMap))
			{
				Out.Log("选择第一个宝藏");
				foreach (int num in lettuceMap.PendingTreasureSelection.TreasureOptions)
				{
					Out.Log("宝藏： " + num.ToString());
				}
				Network.Get().MakeMercenariesMapTreasureSelection(0);
			}
			if (Main.IsVisitor(lettuceMap))
			{
				Out.Log("选择第一个来访者");
				Network.Get().MakeMercenariesMapVisitorSelection(0);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000027A8 File Offset: 0x000009A8
		private static void SelectNextNode()
		{
			if (modeConf.Value == "挂机收菜" && readyToHang == false)
			{
				Out.Log("挂机收菜 只刷一关");
				Network.Get().RetireLettuceMap();
				Main.Sleep(2);
				return;
			}
			List<LettuceMapNode> nodes = NetCache.Get().GetNetObject<NetCache.NetCacheLettuceMap>().Map.Nodes;
			ValueTuple<LettuceMapNode, int> nextNode = Main.GetNextNode(nodes.FindAll((LettuceMapNode n) => n.NodeState_ == LettuceMapNode.NodeState.UNLOCKED), nodes);
			LettuceMapNode lettuceMapNode = nextNode.Item1;
			int item = nextNode.Item2;
			if (lettuceMapNode == null)
			{
				Out.Log("没有找到对应的node重开");
				Network.Get().RetireLettuceMap();
				Main.Sleep(2);
				return;
			}
			if (!Main.NeedCompleted() && item > 2)
			{
				Out.Log("节点太多重开");
				Network.Get().RetireLettuceMap();
				Main.Sleep(2);
				return;
			}
			while (!HsGameUtils.IsMonster(lettuceMapNode.NodeTypeId))
			{
				Network.Get().ChooseLettuceMapNode(lettuceMapNode.NodeId);
				if (HsGameUtils.IsMysteryNode(lettuceMapNode.NodeTypeId))
				{
					HsGameUtils.GotoSceneVillage();
					Main.Sleep(2);
					return;
				}
				lettuceMapNode = Main._GetNextNode(lettuceMapNode.ChildNodeIds, nodes);
			}
			GameMgr gameMgr = GameMgr.Get();
			GameType gameType = GameType.GT_MERCENARIES_PVE;
			FormatType formatType = FormatType.FT_WILD;
			int missionId = 3790;
			int brawlLibraryItemId = 0;
			long deckId = 0L;
			string aiDeck = null;
			int? lettuceMapNodeId = new int?((int)lettuceMapNode.NodeId);
			gameMgr.FindGame(gameType, formatType, missionId, brawlLibraryItemId, deckId, aiDeck, null, false, null, lettuceMapNodeId, 0L, GameType.GT_UNKNOWN);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000028BC File Offset: 0x00000ABC
		[HarmonyPostfix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "TryAutoNextSelectCoin")]
		public static void _PostTryAutoNextSelectCoin()
		{
			Out.Log("TryAutoNextSelectCoin");
			if (!Main.isRunning)
			{
				return;
			}
			Main.ResetIdle();
			if (Main.modeConf.Value == "全自动接任务做任务")
			{
				TaskUtils.UpdateTask();
			}
			Main.SelectNextNode();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000028F8 File Offset: 0x00000AF8
		[HarmonyPrefix]
		[HarmonyPatch(typeof(MercenariesSeasonRewardsDialog), "ShowWhenReady")]
		public static bool _PreShowWhenReady(MercenariesSeasonRewardsDialog __instance)
		{
			Out.Log("显示天梯奖励");
			if (!Main.isRunning)
			{
				return true;
			}
			MercenariesSeasonRewardsDialog.Info info = (MercenariesSeasonRewardsDialog.Info)Traverse.Create(__instance).Field("m_info").GetValue();
			Network.Get().AckNotice(info.m_noticeId);
			return false;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002944 File Offset: 0x00000B44
		[HarmonyPrefix]
		[HarmonyPatch(typeof(RewardPopups), "ShowMercenariesRewards")]
		public static bool _PreShowMercenariesRewards(ref bool autoOpenChest, ref NetCache.ProfileNoticeMercenariesRewards rewardNotice, Action doneCallback = null)
		{
			Out.Log("显示奖励");
			if (!Main.isRunning)
			{
				return true;
			}
			autoOpenChest = true;
			if (rewardNotice.RewardType != ProfileNoticeMercenariesRewards.RewardType.REWARD_TYPE_PVE_CONSOLATION)
			{
				return true;
			}
			Network.Get().AckNotice(rewardNotice.NoticeID);
			if (doneCallback != null)
			{
				doneCallback();
			}
			return false;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002983 File Offset: 0x00000B83
		[HarmonyPostfix]
		[HarmonyPatch(typeof(RewardBoxesDisplay), "RewardPackageOnComplete")]
		public static void _PostRewardPackageOnComplete(RewardBoxesDisplay.RewardBoxData boxData)
		{
			Out.Log("点击奖励");
			if (!Main.isRunning)
			{
				return;
			}
			Main.Sleep(3);
			boxData.m_RewardPackage.TriggerPress();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029B4 File Offset: 0x00000BB4
		[HarmonyPostfix]
		[HarmonyPatch(typeof(RewardBoxesDisplay), "OnDoneButtonShown")]
		public static void _PostOnDoneButtonShown(Spell spell, object userData)
		{
			Out.Log("点击完成按钮");
			if (!Main.isRunning)
			{
				return;
			}
			Main.Sleep(2);
			RewardBoxesDisplay.Get().m_DoneButton.TriggerPress();
			RewardBoxesDisplay.Get().m_DoneButton.TriggerRelease();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A01 File Offset: 0x00000C01
		private static bool IsTreasure(PegasusLettuce.LettuceMap map)
		{
			return map.HasPendingTreasureSelection && map.PendingTreasureSelection.TreasureOptions.Count > 0;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A20 File Offset: 0x00000C20
		private static bool IsVisitor(PegasusLettuce.LettuceMap map)
		{
			return map.HasPendingVisitorSelection && map.PendingVisitorSelection.VisitorOptions.Count > 0;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002A40 File Offset: 0x00000C40
		private void AutoChangeTeam()
		{
			Out.Log("[队伍编辑]");

			global::LettuceTeam lettuceTeam = HsGameUtils.GetAllTeams().Find((global::LettuceTeam t) => t.Name.Equals(Main.teamNameConf.Value));
			if (lettuceTeam == null)
			{
				return;
			}
			List<LettuceMercenary> mercs = lettuceTeam.GetMercs();
			List<int> list = new List<int>();
			int num = 0;
			foreach (LettuceMercenary lettuceMercenary in mercs)
			{
				if (num < Main.coreTeamNumConf.Value)
				{
					num++;
				}
				else
				{
					list.Add(lettuceMercenary.ID);
				}
			}
			foreach (int mercId in list)
			{
				lettuceTeam.RemoveMerc(mercId);
			}
			if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
			{
				// 先考虑全自动模式
				if (Main.modeConf.Value == "全自动接任务做任务")
				{
					foreach (Task task in TaskUtils.GetTasks())
					{
						foreach (MercenaryEntity mercenaryEntity in task.Mercenaries)
						{
							LettuceMercenary mercenary = CollectionManager.Get().GetMercenary((long)mercenaryEntity.ID, false, true);
							if (mercenary != null && mercenary.m_owned && !lettuceTeam.IsMercInTeam(mercenaryEntity.ID, true))
							{
								HsGameUtils.UpdateEq(mercenaryEntity.ID, mercenaryEntity.Equipment);
								lettuceTeam.AddMerc(mercenary, -1, null);
								Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][EID:{1}]，因为全自动做任务",
									mercenaryEntity.ID, mercenaryEntity.Equipment));
								if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
								{
									break;
								}
							}
						}
						if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
						{
							break;
						}
					}
				}
				List<LettuceMercenary> mercenaries = CollectionManager.Get().FindOrderedMercenaries(null, new bool?(true), null, null, null).m_mercenaries;
				if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
				{
					foreach (LettuceMercenary lettuceMercenary2 in mercenaries)
					{
						if (!lettuceTeam.IsMercInTeam(lettuceMercenary2.ID, true) && !lettuceMercenary2.IsReadyForCrafting() && !lettuceMercenary2.IsMaxLevel())
						{
							lettuceTeam.AddMerc(lettuceMercenary2, -1, null);
							Out.Log(string.Format("[队伍编辑] 添加[MID:{0}]，因为未满级",
								lettuceMercenary2.ID));
							if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
							{
								break;
							}
						}
					}
				}
				if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
				{
					foreach (int num2 in MercConst.First)
					{
						LettuceMercenary mercenary2 = HsGameUtils.GetMercenary(num2);
						if (!lettuceTeam.IsMercInTeam(num2, true) && !mercenary2.IsReadyForCrafting() && !mercenary2.m_isFullyUpgraded && mercenary2.m_owned)
						{
							lettuceTeam.AddMerc(mercenary2, -1, null);
							Out.Log(string.Format("[队伍编辑] 添加[MID:{0}]，因为满级优先级设置高",
								mercenary2.ID));
							if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
							{
								break;
							}
						}
					}
				}
				if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
				{
					foreach (LettuceMercenary lettuceMercenary3 in mercenaries)
					{
						if (!lettuceTeam.IsMercInTeam(lettuceMercenary3.ID, true) && !lettuceMercenary3.IsReadyForCrafting() && !lettuceMercenary3.m_isFullyUpgraded && !MercConst.Ignore.Contains(lettuceMercenary3.ID))
						{
							lettuceTeam.AddMerc(lettuceMercenary3, -1, null);
							Out.Log(string.Format("[队伍编辑] 添加[MID:{0}]，满级",
								lettuceMercenary3.ID));
							if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
							{
								break;
							}
						}
					}
				}
				if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
				{
					foreach (LettuceMercenary lettuceMercenary4 in mercenaries)
					{
						if (!lettuceTeam.IsMercInTeam(lettuceMercenary4.ID, true) && !lettuceMercenary4.IsReadyForCrafting())
						{
							lettuceTeam.AddMerc(lettuceMercenary4, -1, null);
							Out.Log(string.Format("[队伍编辑] 添加[MID:{0}]，满级优先级设置低",
								lettuceMercenary4.ID));
							if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
							{
								break;
							}
						}
					}
				}
				lettuceTeam.SendChanges();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002EC8 File Offset: 0x000010C8
		private static void Sleep(int time)
		{
			Main.sleepTime += (float)time;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002ED7 File Offset: 0x000010D7
		private void GameInit()
		{
			if (InactivePlayerKicker.Get() != null)
			{
				Main.initialize = true;
				InactivePlayerKicker.Get().SetKickSec(180000f);
				Main.Sleep(8);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002EFC File Offset: 0x000010FC
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.F9))
			{
				Main.isRunning = !Main.isRunning;
				UIStatus.Get().AddInfo(Main.isRunning ? "插件启动" : "插件关闭");
				Main.runningConf.Value = Main.isRunning;
			}
			if (Input.GetKeyUp(KeyCode.F3))
			{
				Out.Log("F3查询");

// 				Network.Get().UpgradeMercenaryEquipment(18, 139);
				CollectionManager.FindMercenariesResult result = CollectionManager.Get().FindOrderedMercenaries(null, true);
				foreach (LettuceMercenary mercy in result.m_mercenaries)
				{
					foreach (LettuceAbility ability in mercy.m_abilityList)
					{
						string upgrade = "不可升级";
						if (mercy.IsCardReadyForUpgrade(ability.ID, CollectionUtils.MercenariesModeCardType.Ability))
							upgrade = "可升级";
						Out.Log(string.Format("[佣兵;{0}][佣兵ID:{1}][技能:{2}][技能ID:{3}][{4}]",
							mercy.m_mercName, mercy.ID, ability.GetCardName(), ability.ID, upgrade));
					}
					foreach (LettuceAbility ability in mercy.m_equipmentList)
					{
						string upgrade = "不可升级";
						if (mercy.IsCardReadyForUpgrade(ability.ID, CollectionUtils.MercenariesModeCardType.Equipment))
						{
							Network.Get().UpgradeMercenaryEquipment(mercy.ID, ability.ID);
							upgrade = "可升级";
						}
						Out.Log(string.Format("[佣兵;{0}][佣兵ID:{1}][技能:{2}][装备ID:{3}][{4}]",
							mercy.m_mercName, mercy.ID, ability.GetCardName(), ability.ID, upgrade));
					}
				}
			}

			if (!Main.isRunning)
			{
				return;
			}
			this.CheckIdleTime();
			if ((double)(Time.realtimeSinceStartup - Main.sleepTime) <= 1.0)
			{
				return;
			}
			Main.sleepTime = Time.realtimeSinceStartup;
			if (!Main.initialize)
			{
				this.GameInit();
				return;
			}
			GameMgr gameMgr = GameMgr.Get();
			GameType gameType = gameMgr.GetGameType();
			SceneMgr sceneMgr = SceneMgr.Get();
			SceneMgr.Mode mode = sceneMgr.GetMode();
			GameState gameState = GameState.Get();

			#region 查找比赛
			if (gameMgr.IsFindingGame())
			{
				Out.Log("[状态] 查找比赛，休息3秒");
				Main.Sleep(3);
				return;
			}
			#endregion

			#region 村子或者角斗场
			if (gameType == GameType.GT_UNKNOWN && (mode == SceneMgr.Mode.LETTUCE_VILLAGE || mode == SceneMgr.Mode.LETTUCE_PLAY) && gameState == null)
			{
				if (!(Main.modeConf.Value == "Pvp"))
				{
					Out.Log("[状态] 目前处于村庄/角斗场，切换到地图，休息5秒");
					HsGameUtils.GotoSceneMap();
					Main.Sleep(5);
					Main.ResetIdle();
					return;
				}
				List<global::LettuceTeam> teams = CollectionManager.Get().GetTeams();
				if (teams.Count == 0)
				{
					UIStatus.Get().AddInfo("请先创建队伍并在设置里选择队伍！");
					Out.Log("未创建过队伍，插件暂停");
					Main.isRunning = false;
					return;
				}
				global::LettuceTeam lettuceTeam = teams.FindLast((global::LettuceTeam t) => t.Name == Main.teamNameConf.Value);
				if (lettuceTeam == null)
				{
					UIStatus.Get().AddInfo("请先在设置里选择队伍！");
					Out.Log("没有预设名称对应的队伍，插件暂停");
					Main.isRunning = false;
					return;
				}
				Out.Log("[状态] 目前处于村庄/角斗场，切换到PVP模式");
				GameMgr.Get().FindGame(GameType.GT_MERCENARIES_PVP, FormatType.FT_WILD, 3743, 0, 0L, null, null, false, null, null, lettuceTeam.ID, GameType.GT_UNKNOWN);
				return;
			}
			#endregion

			#region 主界面
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.HUB && gameState == null)
			{
				Out.Log("[状态] 目前处于主界面，切换到村庄，休息5秒");
				HsGameUtils.GotoSceneVillage();
				Main.Sleep(5);
				return;
			}
			#endregion 

			#region 悬赏面板
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_BOUNTY_BOARD && gameState == null)
			{
				Out.Log(string.Format("[状态] 目前处于悬赏面板，切换到队伍选择，选择[MAPID:{0}]，休息6秒", GetMapId()));
				HsGameUtils.SelectBoss(this.GetMapId());
				Main.ResetIdle();
				Main.Sleep(6);
				return;
			}
			#endregion
			
			#region 队伍选择
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT && gameState == null)
			{
				this.AutoUpdateSkill();
				this.AutoCraft();
				if (Main.modeConf.Value == "全自动接任务做任务")
				{
					TaskUtils.UpdateTask();
					foreach (Task task in TaskUtils.GetTasks())
					{
						Out.Log(string.Format("[TID:{0}] 已持续：{1}s",
							task.Id, (TaskUtils.Current() - task.StartAt)));
						if (TaskUtils.CleanConf[Main.cleanTaskConf.Value] != -1 && TaskUtils.Current() - task.StartAt > (long)TaskUtils.CleanConf[Main.cleanTaskConf.Value])
						{
							Out.Log(string.Format("[TID:{0}] 已过期，放弃",
								task.Id));
							HsGameUtils.CleanTask(task.Id);
						}
					}
				}
				this.AutoChangeTeam();
				if ((double)Main.idleTime > 20.0)
				{
					HsGameUtils.GotoSceneVillage();
					return;
				}
				Main.Sleep(5);
				List<global::LettuceTeam> teams2 = CollectionManager.Get().GetTeams();
				if (teams2.Count == 0)
				{
					UIStatus.Get().AddInfo("请先创建队伍并在设置里选择队伍！");
					Out.Log("未创建过队伍，插件暂停");
					Main.isRunning = false;
					return;
				}
				global::LettuceTeam lettuceTeam2 = teams2.FindLast((global::LettuceTeam t) => t.Name == Main.teamNameConf.Value);
				if (lettuceTeam2 == null)
				{
					UIStatus.Get().AddInfo("请先在设置里选择队伍！");
					Out.Log("没有预设名称对应的队伍，插件暂停");
					Main.isRunning = false;
					return;
				}
				LettuceVillageDisplay.LettuceSceneTransitionPayload lettuceSceneTransitionPayload = new LettuceVillageDisplay.LettuceSceneTransitionPayload();
				int mapId = this.GetMapId();
				LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(mapId);
				lettuceSceneTransitionPayload.m_TeamId = lettuceTeam2.ID;
				lettuceSceneTransitionPayload.m_SelectedBounty = record;
				lettuceSceneTransitionPayload.m_SelectedBountySet = record.BountySetRecord;
				lettuceSceneTransitionPayload.m_IsHeroic = record.Heroic;
				Out.Log(string.Format("[状态] 目前处于队伍选择，选择[MAPID:{0}]", mapId));
				if (Main.modeConf.Value == "挂机收菜")
				{
					readyToHang = true;
					Out.Log(string.Format("[状态] 下局战斗将写入收菜时间"));
				}
				SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_MAP, SceneMgr.TransitionHandlerType.CURRENT_SCENE, null, lettuceSceneTransitionPayload);
				return;
			}
			#endregion

			#region 地图
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_MAP && gameState == null && (double)Main.idleTime > 20.0)
			{
				Out.Log("[状态] 目前处于地图，空闲时间超过20s，返回村庄");
				sceneMgr.SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE, SceneMgr.TransitionHandlerType.SCENEMGR, null, null);
				Main.Sleep(5);
				return;
			}
			#endregion

			#region 排队中
			if (gameState != null && ((gameType != GameType.GT_MERCENARIES_PVE && gameType != GameType.GT_MERCENARIES_PVP) || sceneMgr.GetMode() != SceneMgr.Mode.GAMEPLAY || !gameState.IsGameCreatedOrCreating()))
			{
				Out.Log("[状态] 排队中");
				Main.Sleep(1);
				return;
			}
			#endregion

			#region 游戏结束
			if (gameState == null || gameState.IsGameOver())
			{
				if (EndGameScreen.Get())
				{
					PegUIElement hitbox = EndGameScreen.Get().m_hitbox;
					if (hitbox != null)
					{
						hitbox.TriggerPress();
						hitbox.TriggerRelease();
						Out.Log("[对局结束] 游戏结束，点击");
						Main.Sleep(10);
						Main.ResetIdle();
					}
				}
				return;
			}
			#endregion


			Main.Sleep(1);
			this.HandlePlay();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003444 File Offset: 0x00001644
		private void AutoCraft()
		{
			if (!Main.autoCraftConf.Value)
			{
				return;
			}
			Out.Log("[制作佣兵]");
			foreach (LettuceMercenary lettuceMercenary in CollectionManager.Get().FindOrderedMercenaries(null, new bool?(true), null, null, null).m_mercenaries)
			{
				if (lettuceMercenary.IsReadyForCrafting())
				{
					Network.Get().CraftMercenary(lettuceMercenary.ID);
					Out.Log(string.Format("[制作佣兵] [MID:{0}]", lettuceMercenary.ID));
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000034F8 File Offset: 0x000016F8
		private void AutoUpdateSkill()
		{
			Out.Log("[升级技能]");
			if (!Main.autoUpdateSkillConf.Value)
			{
				return;
			}
			HsGameUtils.UpdateAllSkill();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000350C File Offset: 0x0000170C
		private int EnsureMapHasUnlock(int id)
		{
			LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(id);
			if (record.RequiredCompletedBounty > 0 && !MercenariesDataUtil.IsBountyComplete(record.RequiredCompletedBounty))
			{
				return MapUtils.GetMapId("1-1");
			}
			return id;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003548 File Offset: 0x00001748
		private int GetMapId()
		{
			int result = 57;
			if (Main.modeConf.Value == "自动解锁地图")
			{
				return MapUtils.GetUnCompleteMap();
			}
			if (Main.modeConf.Value == "全自动接任务做任务")
			{
				int taskMap = TaskUtils.GetTaskMap();
				if (taskMap != -1)
				{
					return this.EnsureMapHasUnlock(taskMap);
				}
				return this.EnsureMapHasUnlock(MapUtils.GetMapId("2-5"));
			}
			else
			{
				Map map = MapUtils.GetMap(Main.mapConf.Value);
				if (map == null)
				{
					UIStatus.Get().AddInfo("地图查找失败，自动关闭插件");
					Main.isRunning = false;
					return result;
				}
				LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(map.ID);
				if (record == null)
				{
					UIStatus.Get().AddInfo("地图查找失败，自动关闭插件");
					Main.isRunning = false;
					return result;
				}
				while (record.RequiredCompletedBounty > 0 && !MercenariesDataUtil.IsBountyComplete(record.RequiredCompletedBounty))
				{
					record = GameDbf.LettuceBounty.GetRecord(record.RequiredCompletedBounty);
				}
				return this.EnsureMapHasUnlock(record.ID);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003638 File Offset: 0x00001838
		private void CheckIdleTime()
		{
			Main.idleTime += Time.deltaTime;
			if (Main.idleTime > 70f && Main.modeConf.Value != "Pvp")
			{
				if (Main.idleTime > 90f)
				{
					Application.Quit();
				}
				if (GameState.Get() != null)
				{
					Out.Log("[IDLE]240s 投降");
					GameState.Get().Concede();
				}
			}
			if (Main.idleTime > 240f)
			{
				if (Main.idleTime > 300f)
				{
					Out.Log("[IDLE] 300s 游戏关闭");
					Application.Quit();
				}
				if (GameState.Get() != null)
				{
					Out.Log("[IDLE] 240s 投降");
					GameState.Get().Concede();
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000036EC File Offset: 0x000018EC
		private static void ResetIdle()
		{
			Out.Log("[IDLE] reset");
			Main.idleTime = 0f;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003702 File Offset: 0x00001902
		private void OnDestroy()
		{
			this._harmony.UnpatchSelf();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003710 File Offset: 0x00001910
		private void HandlePlay()
		{
			if (Main.phaseID == 3)
			{
				return;
			}
			Out.Log("[状态] 对局进行中");
			if (Main.phaseID == 0)
			{
				Out.Log("[对局中] 回合结束");
				InputManager.Get().DoEndTurnButton();
				return;
			}

			if (EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_NO_MORE_PLAYS)
			{
				Out.Log("[对局中] 点击结束按钮");
				InputManager.Get().DoEndTurnButton();
				return;
			}

			// 策略计算
			List<BattleTarget> battleTargets = ((Main.modeConf.Value == "全自动接任务做任务") ? StrategyHelper.GetStrategy("_Sys_Default") : StrategyHelper.GetStrategy(Main.strategyConf.Value)).GetBattleTargets(this.BuildDefaultTargetMercenaries(), this.BuildTargetEntity(Player.Side.OPPOSING), this.BuildTargetEntity(Player.Side.FRIENDLY));
			Dictionary<int, BattleTarget> dict = battleTargets.FindAll((BattleTarget i) => i.SkillId != -1).ToDictionary((BattleTarget i) => i.SkillId, (BattleTarget i) => i);

			// 选择目标阶段
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.OPTION_TARGET)
			{
				foreach (BattleTarget battleTarget in battleTargets)
					Out.Log(string.Format("[对局中] 策略判断 [SID:{0}] [TID:{1}] [TTYPE:{2}]",
							battleTarget.SkillId, battleTarget.TargetId, battleTarget.TargetType));

				// 				Out.Log("GameState.Get().GetTurn() + " + GameState.Get().GetTurn().ToString());
				List<Card> cards_opposite = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards().FindAll((Card i) => (i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER) && !i.GetEntity().IsStealthed());
				List<Card> cards_friend = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards().FindAll((Card i) => (i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER));
				string strlog = "";
				foreach (Card card1 in cards_opposite)
					strlog += string.Format("{0}({1},{2})\t",
						card1.GetEntity().GetEntityId(), card1.GetEntity().GetCurrentHealth(), card1.GetEntity().GetDefHealth());
				Out.Log(string.Format("[对局中] 场面：敌方 {0}", strlog));
				strlog = "";
				foreach (Card card1 in cards_friend)
					strlog += string.Format("{0}({1},{2})\t",
						card1.GetEntity().GetEntityId(), card1.GetEntity().GetCurrentHealth(), card1.GetEntity().GetDefHealth());
				Out.Log(string.Format("[对局中] 场面：友方 {0}", strlog));


				//这个是当前停留的技能id
				Network.Options.Option.SubOption networkSubOption = GameState.Get().GetSelectedNetworkSubOption();
				Out.Log(string.Format("[对局中] 技能目标 当前技能 [SID:{0}]",
					networkSubOption.ID));

				Card card = null;
				// 先
				if (dict.ContainsKey(networkSubOption.ID) && dict[networkSubOption.ID].TargetId != -1)
				{
					Out.Log("[对局中] 技能目标 匹配策略"); 
					if (dict[networkSubOption.ID].TargetType == TARGETTYPE.UNSPECIFIED)
					{
						card = cards_opposite.Find((Card i) => i.GetEntity().GetEntityId() == dict[networkSubOption.ID].TargetId);
						if (card == null)
							card = cards_friend.Find((Card i) => i.GetEntity().GetEntityId() == dict[networkSubOption.ID].TargetId);
					}
					else if (dict[networkSubOption.ID].TargetType == TARGETTYPE.FRIENDLY)
					{
						card = cards_friend.Find((Card i) => i.GetEntity().GetEntityId() == dict[networkSubOption.ID].TargetId);
					}
					
				}

				if (card == null && cards_opposite.Count != 0)
				{
					Out.Log("[对局中] 技能目标 敌方首位");
					card = cards_opposite[0];
				}
				if (card == null && cards_friend.Count != 0)
				{
					Out.Log("[对局中] 技能目标 友方首位");
					card = cards_friend[0];
				}
				if (card == null)
				{
					Out.Log("[对局中] 技能目标 无可用目标 过");
					InputManager.Get().DoEndTurnButton();
				}
				Traverse.Create(InputManager.Get()).Method("DoNetworkOptionTarget", new object[]
				{
					card.GetEntity()
				}).GetValue();
			}
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.OPTION)
			{
				ZonePlay zonePlay = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY);
				if (Main.phaseID == 1 && EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_YOUR_TURN)
				{
					InputManager.Get().DoEndTurnButton();
					Out.Log("[对局中] 上怪");
					return;
				}
				if (Main.phaseID == 2)
				{
					if (modeConf.Value == "挂机收菜" && readyToHang == true)
					{
						Out.Log(string.Format("[战斗中] 初次进入战斗，休息{0}min后再见~", awakeTimeIntervalConf.Value));
						awakeTimeConf.Value = DateTime.Now.AddMinutes(awakeTimeIntervalConf.Value).ToString("G");
						Application.Quit();
					}

					ZonePlay zonePlay2 = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING);
					if (zonePlay2.GetCardCount() == 1 && zonePlay2.GetFirstCard().GetEntity().IsStealthed())
					{
						Out.Log("buzhidao gansha ");
						InputManager.Get().DoEndTurnButton();
						return;
					}
					if (ZoneMgr.Get().GetLettuceAbilitiesSourceEntity() == null)
					{
						using (List<Card>.Enumerator enumerator2 = zonePlay.GetCards().GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Card card2 = enumerator2.Current;
								Entity entity = card2.GetEntity();
								if (!entity.HasSelectedLettuceAbility() || !entity.HasTag(GAME_TAG.LETTUCE_HAS_MANUALLY_SELECTED_ABILITY))
								{
									ZoneMgr.Get().DisplayLettuceAbilitiesForEntity(entity);
									Main.ResetIdle();
									return;
								}
							}
							goto IL_52B;
						}
					}
					HashSet<int> skillSet = Enumerable.ToHashSet<int>(from i in battleTargets select i.SkillId);
					List<Card> displayedLettuceAbilityCards = ZoneMgr.Get().GetLettuceZoneController().GetDisplayedLettuceAbilityCards();
					Card card3 = displayedLettuceAbilityCards.Find((Card i) => skillSet.Contains(i.GetEntity().GetEntityId()) && GameState.Get().HasResponse(i.GetEntity(), new bool?(false)));
					Entity entity2;
					if (card3 != null)
					{
						entity2 = card3.GetEntity();
						Out.Log("[对局中] 技能选择 找到技能 " + card3.name);
					}
					else
					{
						card3 = displayedLettuceAbilityCards.Find((Card i) => GameState.Get().HasResponse(i.GetEntity(), new bool?(false)));
						entity2 = card3.GetEntity();
						Out.Log("[对局中] 技能选择 未找到技能 使用 " + card3.name);
					}
					Traverse.Create(InputManager.Get()).Method("HandleClickOnCardInBattlefield", new object[]
					{
						entity2,
						true
					}).GetValue();
					Main.ResetIdle();
					return;
				}
			}
		IL_52B:
			if (GameState.Get().GetResponseMode() != GameState.ResponseMode.SUB_OPTION)
			{
				return;
			}
			List<Card> friendlyCards = ChoiceCardMgr.Get().GetFriendlyCards();
			InputManager.Get().HandleClickOnSubOption(friendlyCards[friendlyCards.Count - 1].GetEntity(), false);
			Main.ResetIdle();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003CAC File Offset: 0x00001EAC
		private List<Target> BuildTargetEntity(Player.Side side)
		{
			List<Target> list = new List<Target>();
			foreach (Card card2 in ZoneMgr.Get().FindZoneOfType<ZonePlay>(side).GetCards().FindAll((Card card) => (card.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || card.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER) && !card.GetEntity().IsStealthed()))
			{
				Entity entity = card2.GetEntity();
				Target item = new Target
				{
					Name = entity.GetName(),
					Id = entity.GetEntityId(),
					Health = entity.GetCurrentHealth(),
					Speed = card2.GetPreparedLettuceAbilitySpeedValue(),
					DefHealth = entity.GetDefHealth()
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003D74 File Offset: 0x00001F74
		private List<HsMercenaryStrategy.Mercenary> BuildDefaultTargetMercenaries()
		{
			List<Card> cards = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards();
			List<HsMercenaryStrategy.Mercenary> list = new List<HsMercenaryStrategy.Mercenary>();
			foreach (Card card in cards)
			{
				Entity entity = card.GetEntity();
				HsMercenaryStrategy.Mercenary mercenary = new HsMercenaryStrategy.Mercenary
				{
					Name = entity.GetName()
				};
				List<Skill> list2 = new List<Skill>();
				foreach (int id in card.GetEntity().GetLettuceAbilityEntityIDs())
				{
					Entity entity2 = GameState.Get().GetEntity(id);
					if (entity2 != null && !entity2.IsLettuceEquipment())
					{
						list2.Add(new Skill
						{
							Name = entity2.GetName().Substring(0, entity2.GetName().Length - 1),
							Id = entity2.GetEntityId()
						});
					}
				}
				mercenary.Skills = list2;
				list.Add(mercenary);
			}
			return list;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003E9C File Offset: 0x0000209C
		private static LettuceMapNode _GetNextNode(List<uint> nodeIds, List<LettuceMapNode> allNodes)
		{
			List<LettuceMapNode> list = new List<LettuceMapNode>();
			foreach (uint index in nodeIds)
			{
				list.Add(allNodes[(int)index]);
			}
			return Main.GetNextNode(list, allNodes).Item1;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003F04 File Offset: 0x00002104
		private static ValueTuple<LettuceMapNode, int> GetNextNode(List<LettuceMapNode> nodes, List<LettuceMapNode> allNodes)
		{
			int num = int.MaxValue;
			LettuceMapNode item = nodes[0];
			foreach (LettuceMapNode lettuceMapNode in nodes)
			{
				int minNode = Main.GetMinNode(lettuceMapNode, 0, allNodes);
				if (minNode != -1 && minNode < num)
				{
					item = lettuceMapNode;
					num = minNode;
				}
			}
			if (num != 2147483647)
			{
				return new ValueTuple<LettuceMapNode, int>(item, num);
			}
			return new ValueTuple<LettuceMapNode, int>(null, 0);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003F8C File Offset: 0x0000218C
		private static bool NeedCompleted()
		{
			return Main.modeConf.Value == "自动解锁地图" || Main.modeConf.Value == "刷图" || Main.modeConf.Value == "挂机收菜" || TaskUtils.GetTaskMap() != -1;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003FC8 File Offset: 0x000021C8
		// 最短路径
		private static int GetMinNode(LettuceMapNode node, int value, List<LettuceMapNode> nodes)
		{
			if (!Main.NeedCompleted())
			{
				if (HsGameUtils.IsMysteryNode(node.NodeTypeId))
				{
					return value;
				}
				if (HsGameUtils.IsBoss(node.NodeTypeId))
				{
					return -1;
				}
			}
			else
			{
				if (HsGameUtils.IsBoss(node.NodeTypeId))
				{
					return value;
				}
			}
			int num = (!HsGameUtils.IsMonster(node.NodeTypeId)) ? 0 : 1;
			if (node.ChildNodeIds.Count == 1)
			{
				return Main.GetMinNode(nodes[(int)node.ChildNodeIds[0]], value + num, nodes);
			}
			int minNode = Main.GetMinNode(nodes[(int)node.ChildNodeIds[0]], value + num, nodes);
			int minNode2 = Main.GetMinNode(nodes[(int)node.ChildNodeIds[1]], value + num, nodes);
			if (minNode == -1)
			{
				return minNode2;
			}
			if (minNode2 == -1)
			{
				return minNode;
			}
			return Math.Min(minNode, minNode2);
		}

		// Token: 0x04000008 RID: 8
		private readonly Harmony _harmony = new Harmony("hs.patch");

		// Token: 0x04000009 RID: 9
		private static bool isRunning = true;

		// Token: 0x0400000A RID: 10
		private static ConfigEntry<string> mapConf;

		// Token: 0x0400000B RID: 11
		private static ConfigEntry<string> teamNameConf;

		// Token: 0x0400000C RID: 12
		private static ConfigEntry<bool> autoUpdateSkillConf;

		// Token: 0x0400000D RID: 13
		private static ConfigEntry<bool> autoCraftConf;

		// Token: 0x0400000E RID: 14
		private static ConfigEntry<int> coreTeamNumConf;

		// Token: 0x0400000F RID: 15
		private static ConfigEntry<int> teamNumConf;

		// Token: 0x04000010 RID: 16
		private static ConfigEntry<string> modeConf;

		// Token: 0x04000011 RID: 17
		private static ConfigEntry<bool> runningConf;

		// Token: 0x04000012 RID: 18
		private static ConfigEntry<string> strategyConf;

		// Token: 0x04000013 RID: 19
		private static ConfigEntry<string> cleanTaskConf;

		private static ConfigEntry<string> awakeTimeConf;

		private static ConfigEntry<int> awakeTimeIntervalConf;
		// Token: 0x04000014 RID: 20
		private static float sleepTime;

		// Token: 0x04000015 RID: 21
		private static float idleTime;

		// Token: 0x04000016 RID: 22
		private static bool initialize;

		// Token: 0x04000017 RID: 23
		private static int phaseID;

		// 挂机收菜模式 下次战斗准备挂机
		private static bool readyToHang = false;
	}
}
