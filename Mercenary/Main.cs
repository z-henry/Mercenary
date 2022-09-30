using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Hearthstone.Progression;
using HsMercenaryStrategy;
using PegasusLettuce;
using PegasusShared;
using PegasusUtil;
using UnityEngine;
using System.Text.RegularExpressions;
using Blizzard.GameService.SDK.Client.Integration;
using Blizzard.T5.Core;

namespace Mercenary
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Main : BaseUnityPlugin
	{

		private void OnGUI()
		{
			if (!Main.isRunning)
			{
				return;
			}
			GUILayout.Label(new GUIContent(PluginInfo.PLUGIN_VERSION), new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
		}


		private void Awake()
		{

			Regex regex = new Regex(@"^hsunitid:(.*)$");
			foreach (string argument in Environment.GetCommandLineArgs())
			{
				Match match = regex.Match(argument);
				if (match.Groups.Count == 2)
				{
					hsUnitID = match.Groups[1].Value;
					break;
				}
			}
			ConfigFile confgFile;
			if (hsUnitID.Length <= 0)
				confgFile = base.Config;
			else
				confgFile = new BepInEx.Configuration.ConfigFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx/config", hsUnitID, PluginInfo.PLUGIN_GUID + ".cfg"), false,
					new BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION));

			Main.runningConf = confgFile.Bind<bool>("配置", "插件开关", false, new ConfigDescription("插件开关", null, new object[]
			{
				"Advanced"
			}));
			Main.modeConf = confgFile.Bind<string>("配置", "插件运行模式", Mode.刷图.ToString(), new ConfigDescription("插件运行模式", new AcceptableValueList<string>(new string[]
			{
				Mode.刷图.ToString(),
				Mode.刷神秘人.ToString(),
				Mode.全自动接任务做任务.ToString(),
				Mode.自动解锁地图.ToString(),
				Mode.Pvp.ToString(),
				Mode.挂机收菜.ToString(),
				Mode.自动解锁装备.ToString(),
				Mode.自动主线.ToString()
			}), Array.Empty<object>()));
			Main.teamNameConf = confgFile.Bind<string>("配置", "使用的队伍名称", "初始队伍", "使用的队伍名称");
			Main.strategyConf = confgFile.Bind<string>("配置", "战斗策略", FireStrategy.StrategyName, new ConfigDescription("使用的策略,注意只有在非全自动化模式下才会生效", new AcceptableValueList<string>(StrategyHelper.GetAllStrategiesName().ToArray()), Array.Empty<object>()));
			Main.mapConf = confgFile.Bind<string>(new ConfigDefinition("配置", "要刷的地图"), "1-1", new ConfigDescription("要刷的地图", new AcceptableValueList<string>(MapUtils.GetMapNameList()), Array.Empty<object>()));
			Main.autoUpdateSkillConf = confgFile.Bind<bool>("配置", "是否自动升级技能", false, "是否自动升级技能");
			Main.autoCraftConf = confgFile.Bind<bool>("配置", "是否自动制作佣兵", false, "是否自动制作佣兵");
			Main.teamNumConf = confgFile.Bind<int>("配置", "总队伍人数", 6, new ConfigDescription("总队伍人数（PVE下生效）", new AcceptableValueRange<int>(1, 6), Array.Empty<object>()));
			Main.coreTeamNumConf = confgFile.Bind<int>("配置", "队伍核心人数", 0, new ConfigDescription("前n个佣兵不会被自动换掉（PVE下生效）", new AcceptableValueRange<int>(0, 6), Array.Empty<object>()));
			Main.cleanTaskConf = confgFile.Bind<string>(new ConfigDefinition("配置", "自动清理任务时间"), "不开启", new ConfigDescription("会定时清理长时间没完成的任务（全自动模式生效）", new AcceptableValueList<string>(new List<string>(TaskUtils.CleanConf.Keys).ToArray()), Array.Empty<object>()));
			Main.awakeTimeConf = confgFile.Bind<string>("配置", "唤醒时间", "1999/1/1 0:0:0", "挂机收菜下的唤醒时间（无需更改）");
			Main.awakeTimeIntervalConf = confgFile.Bind<int>("配置", "唤醒时间间隔", 22, "挂机收菜下的唤醒时间间隔");
			Main.autoTimeScaleConf = confgFile.Bind<bool>("配置", "自动齿轮加速", false, "战斗中自动启用齿轮加速");
			Main.pvpConcedeLine = confgFile.Bind<int>("配置", "PVP投降分数线", 99999, "PVP投降分数线");
			Main.autoRerollQuest = confgFile.Bind<bool>("配置", "自动更换日周常任务", false, "自动更换日周常任务");

			Main.isRunning = Main.runningConf.Value;
			if (!isRunning)
			{
				return;
			}
			this._harmony.PatchAll(typeof(Main));
		}


		private void Start()
		{
			Out.Log("启动");
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "OnVisitorSelectionResponseReceived")]
		public static bool _PreOnVisitorSelectionResponseReceived()
		{
			//拦截来访者画面
			if (!Main.isRunning)
			{
				return true;
			}
			Network.Get().GetMercenariesMapVisitorSelectionResponse();
			return false;
		}


		[HarmonyPostfix]
		[HarmonyPatch(typeof(LettuceMissionEntity), "ShiftPlayZoneForGamePhase")]
		public static void _PostShiftPlayZoneForGamePhase(int phase)
		{
			//游戏阶段
			// 			Out.Log("_PostShiftPlayZoneForGamePhase phase " + phase.ToString());
			Main.phaseID = phase;
		}


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


		[HarmonyPrefix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "DisplayNewlyGrantedAnomalyCards")]
		public static bool _PreDisplayNewlyGrantedAnomalyCards(global::LettuceMap lettuceMap, int completedNodeId)
		{
			//"弹出揭示卡"
			// 			Out.Log("_PreDisplayNewlyGrantedAnomalyCards");
			return !Main.isRunning;
		}


		[HarmonyPostfix]
		[HarmonyPatch(typeof(global::LettuceMap), "CreateMapFromProto")]
		public static void _PostCreateMapFromProto(PegasusLettuce.LettuceMap lettuceMap)
		{
			Out.Log("[地图信息识别]");
			if (!Main.isRunning || lettuceMap == null)
			{
				return;
			}
			if (Main.modeConf.Value == Mode.Pvp.ToString())
			{
				Out.Log(string.Format("[地图信息识别] Pvp模式，回到村庄"));
				HsGameUtils.GotoSceneVillage();
				return;
			}

			NetCache.NetCacheMercenariesVillageVisitorInfo netObject = NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
			if (netObject != null)
			{
				foreach (MercenariesVisitorState mercenariesVisitorState in netObject.VisitorStates)
				{
					if (mercenariesVisitorState.ActiveTaskState.Status_ == MercenariesTaskState.Status.COMPLETE)
					{
						Out.Log(string.Format("[地图信息识别] [TID:{0}]完成", mercenariesVisitorState.ActiveTaskState.TaskId));
						Network.Get().ClaimMercenaryTask(mercenariesVisitorState.ActiveTaskState.TaskId);
					}
				}
			}
			foreach (LettuceMapNode lettuceMapNode in lettuceMap.Nodes)
			{
				if (GameUtils.IsFinalBossNodeType((int)lettuceMapNode.NodeTypeId) && lettuceMapNode.NodeState_ == LettuceMapNode.NodeState.COMPLETE)
				{
					Out.Log(string.Format("[地图信息识别] 回到悬赏面板"));
					SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_BOUNTY_BOARD, SceneMgr.TransitionHandlerType.NEXT_SCENE, null, null);
					return;
				}
			}
			if (Main.IsTreasure(lettuceMap))
			{
				Out.Log(string.Format("[地图信息识别] 选择第一个宝藏"));
				Network.Get().MakeMercenariesMapTreasureSelection(0);
			}
			if (Main.IsVisitor(lettuceMap))
			{
				Out.Log(string.Format("[地图信息识别] 选择第一个来访者"));
				Network.Get().MakeMercenariesMapVisitorSelection(0);
			}
		}


		private static void SelectNextNode(LettuceMap map)
		{
			// 			Out.Log("[节点选择]");
			Main.ResetIdle();

			if (Main.modeConf.Value == Mode.全自动接任务做任务.ToString())
			{
				TaskUtils.UpdateMercTask();
			}
			List<LettuceMapNode> nodes = map.NodeData;
			// 			foreach (LettuceMapNode node in nodes)
			// 			{
			// 				string childrendid = "";
			// 				foreach (uint childid in node.ChildNodeIds)
			// 					childrendid += (childid.ToString() + ", ");
			// 				Out.Log(string.Format("[地图结构] [NID:{0}][NTYPE:{1}][NSTATE:{2}][CID:{3}]", node.NodeId, node.NodeTypeId, node.NodeState_, childrendid));
			// 			}
			ValueTuple<LettuceMapNode, int> nextNode = Main.GetNextNode(nodes.FindAll((LettuceMapNode n) => n.NodeState_ == LettuceMapNode.NodeState.UNLOCKED), nodes);
			LettuceMapNode lettuceMapNode = nextNode.Item1;
			int item = nextNode.Item2;
			if (lettuceMapNode == null)
			{
				Out.Log("[节点选择] 没有找到神秘节点或任务赐福节点 重开");
				Network.Get().RetireLettuceMap();
				Main.Sleep(2);
				return;
			}
			if (!Main.NeedCompleted() && item > 2)
			{
				Out.Log(string.Format("[节点选择] 通往神秘节点数或任务赐福节点步长:{0}大于2 重开", item));
				Network.Get().RetireLettuceMap();
				Main.Sleep(2);
				return;
			}
			if (HsGameUtils.IsMonster(lettuceMapNode.NodeTypeId))
			{
				GameMgr gameMgr = GameMgr.Get();
				GameType gameType = GameType.GT_MERCENARIES_PVE;
				FormatType formatType = FormatType.FT_WILD;
				int missionId = 3790;
				int brawlLibraryItemId = 0;
				long deckId = 0L;
				string aiDeck = null;
				int? lettuceMapNodeId = new int?((int)lettuceMapNode.NodeId);
				Out.Log(string.Format("[节点选择] 怪物节点[NID:{0}][NTYPE:{1}] 进入战斗", lettuceMapNode.NodeId, lettuceMapNode.NodeTypeId));
				gameMgr.FindGame(gameType, formatType, missionId, brawlLibraryItemId, deckId, aiDeck, null, false, null, lettuceMapNodeId, 0L, GameType.GT_UNKNOWN);
				if (modeConf.Value == Mode.挂机收菜.ToString())
				{
					readyToHang = true;
					Out.Log("挂机收菜 下局战斗将写入收菜时间");
				}

			}
			else
			{
				Out.Log(string.Format("[节点选择] 普通怪物节点[NID:{0}][NTYPE:{1}]", lettuceMapNode.NodeId, lettuceMapNode.NodeTypeId));
				Network.Get().ChooseLettuceMapNode(lettuceMapNode.NodeId);
			}
		}


		[HarmonyPostfix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "TryAutoNextSelectCoin")]
		public static void _PostTryAutoNextSelectCoin(LettuceMapDisplay __instance)
		{

			if (!Main.isRunning)
			{
				return;
			}
			// 			Out.Log("TryAutoNextSelectCoin");
			LettuceMap map = (global::LettuceMap)Traverse.Create(__instance).Field("m_lettuceMap").GetValue();
			Main.SelectNextNode(map);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(LettuceMapDisplay), "OnLettuceMapChooseNodeResponseReceived")]
		public static void _PostOnLettuceMapChooseNodeResponseReceived()
		{
			// 			Out.Log("OnLettuceMapChooseNodeResponseReceived");
			LettuceMapChooseNodeResponse lettuceMapChooseNodeResponse = Network.Get().GetLettuceMapChooseNodeResponse();
			if (lettuceMapChooseNodeResponse == null ||
				!lettuceMapChooseNodeResponse.Success)
			{
				Out.Log(string.Format("[节点选择] 应答失败，返回村庄重试"));
				Main.Sleep(10);
				HsGameUtils.GotoSceneVillage();
				return;
			}
		}

		//显示完整战网ID
		[HarmonyPrefix]
		[HarmonyPatch(typeof(BnetPlayer), "GetBestName")]
		public static bool PatchBnetPlayerGetBestName(BnetPlayer __instance, ref BnetAccount ___m_account, ref Map<BnetGameAccountId, BnetGameAccount> ___m_gameAccounts, ref BnetGameAccount ___m_hsGameAccount, ref string __result)
		{
			if (__instance != BnetPresenceMgr.Get().GetMyPlayer())
			{
				if (___m_account != null)
				{
					string fullName = ___m_account.GetFullName();
					if (fullName != null)
					{
						__result = fullName;
						return false;
					}
					if (___m_account.GetBattleTag() != null)
					{
						__result = ___m_account.GetBattleTag().GetName();
						return false;
					}
				}
				foreach (KeyValuePair<BnetGameAccountId, BnetGameAccount> gameAccount in ___m_gameAccounts)
				{
					if (gameAccount.Value.GetBattleTag() != (BnetBattleTag)null)
					{
						__result = gameAccount.Value.GetBattleTag().ToString();
						Cache.lastOpponentFullName = gameAccount.Value.GetBattleTag().ToString();
						return false;
					}
				}
				__result = null;
				return false;
			}
			__result = ___m_hsGameAccount.GetBattleTag()?.GetName();
			return false;
		}



		private static bool IsTreasure(PegasusLettuce.LettuceMap map)
		{
			return map.HasPendingTreasureSelection && map.PendingTreasureSelection.TreasureOptions.Count > 0;
		}


		private static bool IsVisitor(PegasusLettuce.LettuceMap map)
		{
			return map.HasPendingVisitorSelection && map.PendingVisitorSelection.VisitorOptions.Count > 0;
		}


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
			// 1. 匹配模式
			if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
			{
				if (Main.modeConf.Value == Mode.全自动接任务做任务.ToString())
				{
					foreach (Task task in TaskUtils.GetTasks())
					{
						foreach (MercenaryEntity mercenaryEntity in task.Mercenaries)
						{
							LettuceMercenary mercenary = HsGameUtils.GetMercenary(mercenaryEntity.ID);
							if (mercenary != null && mercenary.m_owned && !lettuceTeam.IsMercInTeam(mercenaryEntity.ID, true))
							{
								HsGameUtils.UpdateEq(mercenaryEntity.ID, mercenaryEntity.Equipment);
								lettuceTeam.AddMerc(mercenary, -1, null);
								Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}][EID:{2}]，因为全自动做任务",
									mercenaryEntity.ID, mercenaryEntity.Name, mercenaryEntity.Equipment));
								if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
								{
									break;
								}
							}
						}
					}
				}
				else if (Main.modeConf.Value == Mode.自动解锁装备.ToString())
				{
					if (Cache.unlockMercID != -1)
					{
						LettuceMercenary mercenary_boss = HsGameUtils.GetMercenary(Cache.unlockMercID);
						lettuceTeam.AddMerc(mercenary_boss, -1, null);
						Out.Log($"[队伍编辑] 添加[MID:{mercenary_boss.ID}][MNAME:{mercenary_boss.m_mercName}]，因为自动解锁装备_老板");
					}
				}
			}
			List<LettuceMercenary> mercenaries = (
				from x in CollectionManager.Get().FindOrderedMercenaries(null, true, null, null, null).m_mercenaries
				where x.m_owned == true && x.m_isFullyUpgraded == false && HsGameUtils.CalcMercenaryCoinNeed(x) > 0
				orderby (MercConst.First.IndexOf(x.ID) == -1 ? int.MaxValue : MercConst.First.IndexOf(x.ID)) ascending
				select x
				).ToList<global::LettuceMercenary>();
			// 2. 匹配未满级
			if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
			{
				foreach (LettuceMercenary lettuceMercenary2 in mercenaries)
				{
					if (!lettuceTeam.IsMercInTeam(lettuceMercenary2.ID, true) &&
						!lettuceMercenary2.IsMaxLevel())
					{
						lettuceTeam.AddMerc(lettuceMercenary2, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，因为未满级",
							lettuceMercenary2.ID, lettuceMercenary2.m_mercName));
						if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
						{
							break;
						}
					}
				}
			}
			// 3. 匹配优先级高
			if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
			{
				foreach (int mercid in MercConst.First)
				{
					LettuceMercenary mercenary = HsGameUtils.GetMercenary(mercid);
					if (mercenary == null)
					{
						Out.Log(string.Format("[队伍编辑] 无此佣兵[MID:{0}]",
							mercenary.ID));
						continue;
					}
					if (mercenary.m_owned &&
						!lettuceTeam.IsMercInTeam(mercid, true) &&
						!mercenary.m_isFullyUpgraded &&
						HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 0)
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，因为满级优先级设置高",
							mercenary.ID, mercenary.m_mercName));
						if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
						{
							break;
						}
					}
				}
			}
			// 4. 匹配优先级中
			if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
			{
				foreach (LettuceMercenary mercenary in mercenaries)
				{
					if (!lettuceTeam.IsMercInTeam(mercenary.ID, true))
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，满级",
							mercenary.ID, mercenary.m_mercName));
						if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
						{
							break;
						}
					}
				}
			}
			// 5. 匹配优先级低
			if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
			{
				foreach (LettuceMercenary mercenary in mercenaries)
				{
					if (!lettuceTeam.IsMercInTeam(mercenary.ID, true))
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，满级优先级设置低",
							mercenary.ID, mercenary.m_mercName));
						if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
						{
							break;
						}
					}
				}
			}
			// 6. 全满补位
			if (lettuceTeam.GetMercCount() < Main.teamNumConf.Value)
			{
				foreach (LettuceMercenary mercenary in CollectionManager.Get().FindOrderedMercenaries(null, true, null, null, null).m_mercenaries)
				{
					if (!lettuceTeam.IsMercInTeam(mercenary.ID, true))
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，全满补位",
							mercenary.ID, mercenary.m_mercName));
						if (lettuceTeam.GetMercCount() == Main.teamNumConf.Value)
						{
							break;
						}
					}
				}
			}

			lettuceTeam.SendChanges();
		}


		private static void Sleep(float time)
		{
			Main.sleepTime += (float)time;
		}


		private void GameInit()
		{
			if (InactivePlayerKicker.Get() != null)
			{
				Main.initialize = true;
				InactivePlayerKicker.Get().SetKickSec(180000f);
				Main.Sleep(8);
			}
		}


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

				// 查拥有的佣兵情况
				CollectionManager.FindMercenariesResult result = CollectionManager.Get().FindOrderedMercenaries();
				foreach (LettuceMercenary mercy in result.m_mercenaries)
				{
					foreach (LettuceAbility ability in mercy.m_abilityList)
					{
						Out.Log(string.Format("[佣兵;{0}][佣兵ID:{1}][技能:{2}]",
							mercy.m_mercName, mercy.ID, ability.GetCardName()));
					}
					foreach (LettuceAbility ability in mercy.m_equipmentList)
					{
						Out.Log(string.Format("[佣兵;{0}][佣兵ID:{1}][装备:{2}]",
							mercy.m_mercName, mercy.ID, ability.GetCardName()));
					}
				}

				// 查所有地图节点
				List<LettuceMapNodeTypeDbfRecord> results = GameDbf.LettuceMapNodeType.GetRecords();
				foreach (LettuceMapNodeTypeDbfRecord result1 in results)
					Out.Log(result1.GetVar("ID").ToString() + " " +
						result1.VisitLogic + " " +
						result1.GetVar("NOTE_DESC") + " " +
						result1.BossType + " " +
						result1.NodeVisualId 
						);

// 				foreach (AchievementDataModel achieve in
// 					from x in AchievementManager.Get().GetRecentlyCompletedAchievements()
// 					where x.Status == AchievementManager.AchievementStatus.COMPLETED
// 					orderby x.ID ascending
// 					select x
// 					)
// 				{
// 					AchievementManager.Get().AckAchievement(achieve.ID);
// 					// 					Network.Get().AckAchievement(achieve.ID);
// 					Out.Log(string.Format("[{0}]", achieve.ID));
// 					break;
// 				}
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
// 			HsMod.ConfigValue.Get().TimeGearEnable = Main.autoTimeScaleConf.Value;

			#region 查找比赛
			if (gameMgr.IsFindingGame())
			{
				Global.matchFirstRecord = true;
				Out.Log("[状态] 查找比赛");
				return;
			}
			#endregion

			#region 角斗场
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_PLAY && gameState == null)
			{
				Out.Log("[状态] 目前处于角斗场，切换到村庄，休息5秒");
				HsGameUtils.GotoSceneVillage();
				Main.Sleep(5);
				Main.ResetIdle();
				return;
			}
			#endregion

			#region 村子
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_VILLAGE && gameState == null)
			{
				if (Main.modeConf.Value != Mode.Pvp.ToString())
				{
					Out.Log("[状态] 目前处于村庄，切换到地图，休息5秒");
					HsGameUtils.GotoSceneMap();
					Main.Sleep(5);
					Main.ResetIdle();
					return;
				}

				if (Main.autoRerollQuest.Value == true)
					QuestManager.Instance.RollAQuest();


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
				Out.Log("[状态] 目前处于村庄，开始PVP战斗");
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
				if (Main.autoRerollQuest.Value == true)
					QuestManager.Instance.RollAQuest();

				int mapID = GetMapId();
				Out.Log($"[状态] 目前处于悬赏面板，切换到队伍选择，选择[MAP:{MapUtils.GetMapByID(mapID).Name}]，休息3秒");
				HsGameUtils.SelectBoss(mapID);
				Main.ResetIdle();
				Main.Sleep(3);
				return;
			}
			#endregion

			#region 队伍选择
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT && gameState == null)
			{
				// 模式预处理
				if (Main.modeConf.Value == Mode.全自动接任务做任务.ToString())
				{
					TaskUtils.UpdateMercTask();
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
				else if (Main.modeConf.Value == Mode.自动解锁装备.ToString())
				{
					UpdateAutoUnlockEquipInfo();
					coreTeamNumConf.Value = 5;
					teamNumConf.Value = 6;
				}
				else if (Main.modeConf.Value == Mode.自动解锁地图.ToString())
				{
					coreTeamNumConf.Value = 6;
					teamNumConf.Value = 6;
				}
				else if (Main.modeConf.Value == Mode.自动主线.ToString())
				{
					TaskUtils.UpdateMainLineTask();
					coreTeamNumConf.Value = 6;
					teamNumConf.Value = 6;
				}


				this.AutoUpdateSkill();
				this.AutoCraft();
				this.AutoChangeTeam();
				if ((double)Main.idleTime > 20.0)
				{
					HsGameUtils.GotoSceneVillage();
					return;
				}
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
				Out.Log($"[状态] 目前处于队伍选择，选择[MAP:{MapUtils.GetMapByID(mapId).Name}] [BOSS:{MapUtils.GetMapByID(mapId).Boss}]");
				SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_MAP, SceneMgr.TransitionHandlerType.CURRENT_SCENE, null, lettuceSceneTransitionPayload);
				Main.Sleep(5);
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

			// 			#region 排队中
			// 			if (gameState != null && ((gameType != GameType.GT_MERCENARIES_PVE && gameType != GameType.GT_MERCENARIES_PVP) || sceneMgr.GetMode() != SceneMgr.Mode.GAMEPLAY || !gameState.IsGameCreatedOrCreating()))
			// 			{
			// 				Out.Log("[状态] 排队中");
			// 				Main.Sleep(1);
			// 				return;
			// 			}
			// 			#endregion

			#region 对局已生成
			if ((gameType == GameType.GT_MERCENARIES_PVE || gameType == GameType.GT_MERCENARIES_PVP || gameType == GameType.GT_RANKED) &&
				sceneMgr.GetMode() == SceneMgr.Mode.GAMEPLAY &&
				gameState.IsGameCreatedOrCreating())
			{
				// 游戏结束
				if (gameState.IsGameOver())
				{
					//关闭对局内齿轮
					if (HsMod.ConfigValue.Get().TimeGearEnable != false && 
						Main.autoTimeScaleConf.Value == true)
						HsMod.ConfigValue.Get().TimeGearEnable = false;

					EndGameScreen endGameScreen = EndGameScreen.Get();
					if (endGameScreen)
					{
						//记录一些信息
						if (Global.matchFirstRecord == true)
						{
							Global.matchFirstRecord = false;
							
							// 输赢
							string gameResult = "";
							switch (endGameScreen.GetType().Name)
							{
								case "VictoryScreen":
									gameResult = "胜利";
									break;
								case "DefeatScreen":
									gameResult = "失败";
									break;
								default:
									break;
							}

							// 经验
							RewardXpNotificationManager rewardXpNotificationManager = RewardXpNotificationManager.Get();
							if (rewardXpNotificationManager != null)
							{
								List<RewardTrackXpChange> xpChanges = (List<RewardTrackXpChange>)Traverse.Create(rewardXpNotificationManager).Field("m_xpChanges").GetValue();
								foreach (RewardTrackXpChange xpChange in xpChanges)
								{
									Out.Log(string.Format("[对局结束] 战令信息 {0} 等级:{1} 经验:{2} {3} {4}",
										gameResult, xpChange.CurrLevel, xpChange.CurrXp, xpChange.RewardSourceType, xpChange.RewardSourceId));
								}
							}
							if (gameType == GameType.GT_MERCENARIES_PVP)
							{
								// 阵容
								string str_lineup_front = "", str_lineup_back = "";
								int count = 0;
								foreach (string str in Cache.pvpMercTeam)
								{
									if (++count > 3)
										str_lineup_back += (str + ",");
									else
										str_lineup_front += (str + ",");
								}
								Cache.pvpMercTeam.Clear();

								// 分数
								string rateResult = "", rateResultDelta = "";
								if (GameState.Get().GetGameEntity() is LettuceMissionEntity)
								{
									LettuceMissionEntity lettuceGameEntity = (LettuceMissionEntity)GameState.Get().GetGameEntity();
									if (lettuceGameEntity != null && lettuceGameEntity.RatingChangeData != null)
									{
										rateResult = lettuceGameEntity.RatingChangeData.PvpRating.ToString();
										rateResultDelta = lettuceGameEntity.RatingChangeData.Delta.ToString();
									}
								}

								Out.LogGameRecord(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
									rateResultDelta, rateResult, Main.teamNameConf.Value, Cache.lastOpponentFullName, str_lineup_front, str_lineup_back));
							}
						}

						// 点击游戏结束界面
						PegUIElement hitbox = endGameScreen.m_hitbox;
						if (hitbox != null)
						{
							hitbox.TriggerPress();
							hitbox.TriggerRelease();
							Out.Log("[对局结束] 游戏结束，点击");
							Main.Sleep(1);
							Main.ResetIdle();
							return;
						}
					}
				}
				// 游戏中
				else
				{
					// pvp模式
					// 1. 大于投降分数线投降
					if (gameType == GameType.GT_MERCENARIES_PVP)
					{
						if (NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesPlayerInfo>().PvpRating > pvpConcedeLine.Value)
						{
							Out.Log("[对局结束] PVP投降");
							Main.Sleep(5);
							GameState.Get().Concede();
							Main.ResetIdle();
							return;
						}
					}
					// 传统模式
					// 1. 直接投降
					else if (gameType == GameType.GT_RANKED)
					{
						Out.Log("[对局结束] 传统模式投降");
						Main.Sleep(5);
						GameState.Get().Concede();
						Main.ResetIdle();
						return;
					}

					//正常处理对局
					this.HandlePlay();
					Main.Sleep(0.2f);
					return;
				}
			}
			#endregion


			#region 其他模式处理
			// 传统模式选择队伍界面
			if (sceneMgr.GetMode() == SceneMgr.Mode.TOURNAMENT)
			{
				Out.Log("[状态] 目前处于传统对战队伍选择，切换到村庄，休息5秒");
				HsGameUtils.GotoSceneVillage();
				Main.Sleep(5);
				return;
			}
			#endregion

			Main.Sleep(1f);
		}


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


		private void AutoUpdateSkill()
		{
			Out.Log("[升级技能]");
			if (!Main.autoUpdateSkillConf.Value)
			{
				return;
			}
			HsGameUtils.UpdateAllSkill();
		}


		private int EnsureMapHasUnlock(int id)
		{
			LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(id);
			if (record.RequiredCompletedBounty > 0 && !MercenariesDataUtil.IsBountyComplete(record.RequiredCompletedBounty))
			{
				return MapUtils.GetUnCompleteMap();
			}
			return id;
		}


		private int GetMapId()
		{
			int result = 57;
			if (Main.modeConf.Value == Mode.自动解锁地图.ToString())
			{
				return MapUtils.GetUnCompleteMap();
			}
			else if (Main.modeConf.Value == Mode.全自动接任务做任务.ToString() ||
				Main.modeConf.Value == Mode.自动主线.ToString())
			{
				int taskMap = TaskUtils.GetTaskMap();
				if (taskMap != -1)
				{
					return this.EnsureMapHasUnlock(taskMap);
				}
				return this.EnsureMapHasUnlock(MapUtils.GetMapId("2-5"));
			}
			else if (Main.modeConf.Value == Mode.自动解锁装备.ToString())
			{
				int taskMap = Cache.unlockMapID;
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


		private void CheckIdleTime()
		{
			Main.idleTime += Time.deltaTime;
			if (Main.idleTime > 70f && Main.modeConf.Value != Mode.Pvp.ToString())
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


		private static void ResetIdle()
		{
			// 			Out.Log("[IDLE] reset");
			Main.idleTime = 0f;
		}


		private void OnDestroy()
		{
			this._harmony.UnpatchSelf();
		}


		private void HandlePlay()
		{
			if (Main.phaseID == 3)
			{
				return;
			}
			// 			Out.Log("[状态] 对局进行中");
			if (Main.phaseID == 0)
			{
				// 				Out.Log("[对局中] 回合结束");
				// 				InputManager.Get().DoEndTurnButton();
				return;
			}

			if (EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_NO_MORE_PLAYS)
			{
				Out.Log("[对局中] 点击结束按钮");
				InputManager.Get().DoEndTurnButton();
				return;
			}

			// 策略计算
			// 			string strlog = "";
			// 			Out.Log(string.Format("[test] turn {0}", GameState.Get().GetTurn()));
			// 			foreach (Target target_iter in BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING))
			// 				strlog += string.Format("{0}[{1}][{2}]\t", target_iter.Name, target_iter.Enable ? "√" : "×", target_iter.Role.ToString());
			// 			Out.Log(string.Format("[test] 场面：敌方 {0}", strlog));
			// 			strlog = "";
			// 			foreach (Target target_iter in BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards(), Player.Side.FRIENDLY))
			// 				strlog += string.Format("{0}[{1}][{2}]\t", target_iter.Name, target_iter.Enable ? "√" : "×", target_iter.Role.ToString());
			// 			Out.Log(string.Format("[test] 场面：友方 {0}", strlog));
			// 			strlog = "";
			// 			foreach (Target target_iter in BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZoneGraveyard>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING))
			// 				strlog += string.Format("{0}[{1}][{2}]\t", target_iter.Name, target_iter.Enable ? "√" : "×", target_iter.Role.ToString());
			// 			Out.Log(string.Format("[test] 坟场：敌方 {0}", strlog));
			string strategy_name = Main.modeConf.Value == Mode.全自动接任务做任务.ToString() ? "_Sys_Default" : Main.strategyConf.Value;
			List<BattleTarget> battleTargets = StrategyHelper.GetStrategy(strategy_name).GetBattleTargets(
				GameState.Get().GetTurn(),
				this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING),
				this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards(), Player.Side.FRIENDLY),
				this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZoneGraveyard>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING)
				);
			Dictionary<int, BattleTarget> dict = new Dictionary<int, BattleTarget>();
			foreach (BattleTarget battleTarget in battleTargets)
			{
				if (battleTarget.SkillId == -1)
					continue;
				if (!dict.ContainsKey(battleTarget.SkillId))
					dict.Add(battleTarget.SkillId, battleTarget);
			}

			// 选择目标阶段
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.OPTION_TARGET)
			{
				// 				Out.Log("GameState.Get().GetTurn() + " + GameState.Get().GetTurn().ToString());
				List<Card> cards_opposite = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards().FindAll((Card i) => (i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER) && !i.GetEntity().IsStealthed());
				List<Card> cards_friend = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards().FindAll((Card i) => (i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || i.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER));

				//这个是当前停留的技能id
				Network.Options.Option.SubOption networkSubOption = GameState.Get().GetSelectedNetworkSubOption();
				Out.Log(string.Format("[对局中] 技能目标 当前技能 [SID:{0}]",
					networkSubOption.ID));

				foreach (BattleTarget battleTarget in battleTargets)
					Out.Log(string.Format("[对局中] 技能目标 策略判断 [SID:{0}] [SNAME:{1}] [TID:{2}] [TNAME:{3}] [TTYPE:{4}]",
							battleTarget.SkillId, battleTarget.SkillName, battleTarget.TargetId, battleTarget.TargetName, battleTarget.TargetType));

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
				// 上怪
				if (Main.phaseID == 1 &&
					EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_YOUR_TURN)
				{
					if (modeConf.Value == Mode.挂机收菜.ToString() && readyToHang == true)
					{
						Out.Log(string.Format("[战斗中] 初次进入战斗，休息{0}min后再见~", awakeTimeIntervalConf.Value));
						awakeTimeConf.Value = DateTime.Now.AddMinutes(awakeTimeIntervalConf.Value).ToString("G");
						Application.Quit();
						return;
					}
					if (Main.autoTimeScaleConf.Value == true)
					{
						HsMod.ConfigValue.Get().TimeGearEnable = true;
						HsMod.ConfigValue.Get().TimeGearValue = Main.TimeScaleValue;
					}
					ZonePlay zonePlay = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY);
					ZoneHand zoneHand = ZoneMgr.Get().FindZoneOfType<ZoneHand>(Player.Side.FRIENDLY);

					ZoneDeck zoneOppositeDeck = ZoneMgr.Get().FindZoneOfType<ZoneDeck>(Player.Side.OPPOSING);
					ZoneHand zoneOppositeHand = ZoneMgr.Get().FindZoneOfType<ZoneHand>(Player.Side.OPPOSING);

					if (zoneHand != null)
					{
						Dictionary<HsMercenaryStrategy.TAG_ROLE, int> dictOppositeRoleCount = new Dictionary<HsMercenaryStrategy.TAG_ROLE, int>()
						{
							{ HsMercenaryStrategy.TAG_ROLE.CASTER, 0 },
							{ HsMercenaryStrategy.TAG_ROLE.FIGHTER, 0 },
							{ HsMercenaryStrategy.TAG_ROLE.INVALID, 0 },
							{ HsMercenaryStrategy.TAG_ROLE.NEUTRAL, 0 },
							{ HsMercenaryStrategy.TAG_ROLE.TANK, 0 },
						};
						// 						string strlog = "";
						foreach (Card card_iter in zoneOppositeDeck.GetCards())
							dictOppositeRoleCount[(HsMercenaryStrategy.TAG_ROLE)(card_iter.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE))]++;
						foreach (Card card_iter in zoneOppositeHand.GetCards())
							dictOppositeRoleCount[(HsMercenaryStrategy.TAG_ROLE)(card_iter.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE))]++;
						// 						Out.Log(string.Format("[test] 预测{0}", strlog));

						(int hand_index, int play_index) = StrategyHelper.GetStrategy(strategy_name).GetEnterOrder(
							BuildTargetFromCards(zoneHand.GetCards(), Player.Side.FRIENDLY),
							BuildTargetFromCards(zonePlay.GetCards(), Player.Side.FRIENDLY),
							dictOppositeRoleCount,
							this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING),
							this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZoneGraveyard>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING)
							);

						GameState gameState = GameState.Get();
						if (gameState != null)
						{
							Out.Log(string.Format("[佣兵登场] 选择[佣兵{0}:{1}]，位置[{2}]",
								hand_index, zoneHand.GetCardAtIndex(hand_index).GetEntity().GetName(), play_index));
							gameState.SetSelectedOption(hand_index + 1);
							gameState.SetSelectedSubOption(-1);
							gameState.SetSelectedOptionTarget(0);
							gameState.SetSelectedOptionPosition(play_index + 1);
							gameState.SendOption();
							Sleep(0.75f);
						}
						return;
					}

					// 					InputManager.Get().DoEndTurnButton();
					// 					Out.Log("[对局中] 上怪，休息5秒");
					// 					Sleep(5);
					// 					return;
				}
				// 佣兵技能选择
				if (Main.phaseID == 2)
				{
					ZonePlay zonePlay_opposing = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING);
					ZonePlay zonePlay_friendly = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY);
					// PVP模式记录对面佣兵的登场情况
					if (GameMgr.Get().GetGameType() == GameType.GT_MERCENARIES_PVP)
					{
						foreach (Card card in zonePlay_opposing.GetCards())
						{
							if (card.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE) != TAG_ROLE.CASTER &&
								card.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE) != TAG_ROLE.TANK &&
								card.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE) != TAG_ROLE.FIGHTER)
								continue;
							string equipment = card.GetEntity().GetEquipmentEntity()?.GetName() ?? "";
							if (equipment.Length > 0 && Char.IsNumber(equipment[equipment.Length - 1]))
								equipment = equipment.Substring(0, equipment.Length - 1);
							Cache.pvpMercTeam.Add(card.GetEntity().GetName() + '-' + equipment);
						}
					}
					if (null == zonePlay_opposing.GetCards().Find((Card i) => false == i.GetEntity().IsStealthed()))
					{
						Out.Log("[对局中] 他们都藏起来了？？！！");
						InputManager.Get().DoEndTurnButton();
						return;
					}
					Entity currentSelectMerc_Entity = ZoneMgr.Get().GetLettuceAbilitiesSourceEntity();
					if (currentSelectMerc_Entity == null)
					{
						foreach (Card card in zonePlay_friendly.GetCards())
						{
							Entity entity = card.GetEntity();
							if (!entity.HasSelectedLettuceAbility() || !entity.HasTag(GAME_TAG.LETTUCE_HAS_MANUALLY_SELECTED_ABILITY))
							{
								Out.Log(string.Format("[对局中] 佣兵选择 [{0}]的技能界面", entity.GetName()));
								ZoneMgr.Get().DisplayLettuceAbilitiesForEntity(entity);
								// 								Traverse.Create(InputManager.Get()).Method("HandleClickOnCardInBattlefield", new object[]
								// 								{
								// 									entity,
								// 									true
								// 								}).GetValue();
								Main.ResetIdle();
								return;
							}
						}
					}
					else
					{
						BattleTarget currentMerc_BattleTarget = battleTargets.Find((BattleTarget i) => i.MercName == currentSelectMerc_Entity.GetName());
						// 策略规定此佣兵可以操作
						if (currentMerc_BattleTarget == null || currentMerc_BattleTarget.NeedActive == true)
						{
							Out.Log(string.Format("[对局中] 技能选择 操作佣兵 [{0}]", currentSelectMerc_Entity.GetName()));
							Card card = null;
							List<Card> displayedLettuceAbilityCards = ZoneMgr.Get().GetLettuceZoneController().GetDisplayedLettuceAbilityCards();
							foreach (BattleTarget batterTarget in battleTargets)
							{
								card = displayedLettuceAbilityCards.Find((Card i) => i.GetEntity().GetEntityId() == batterTarget.SkillId && GameState.Get().HasResponse(i.GetEntity(), new bool?(false)));
								if (card != null)
									break;
							}
							if (card != null)
							{
								Out.Log(string.Format("[对局中] 技能选择 匹配策略 [{0}]", card.GetEntity().GetName()));
							}
							else
							{
								card = displayedLettuceAbilityCards.Find((Card i) => GameState.Get().HasResponse(i.GetEntity(), new bool?(false)));
								Out.Log(string.Format("[对局中] 技能选择 顺序选择 [{0}]", card.GetEntity().GetName()));
							}
							Traverse.Create(InputManager.Get()).Method("HandleClickOnCardInBattlefield", new object[]
							{
							card.GetEntity(),
							true
							}).GetValue();
							Main.ResetIdle();
							return;
						}
						// 策略规定此佣兵不可以操作
						else
						{
							Out.Log(string.Format("[对局中] 操作佣兵 设置为不操作 [{0}]", currentSelectMerc_Entity.GetName()));
							Dictionary<string, bool> dict_mercactive = new Dictionary<string, bool>();
							foreach (BattleTarget battleTarget in battleTargets)
							{
								if (battleTarget.MercName.Length <= 0)
									continue;
								if (!dict_mercactive.ContainsKey(battleTarget.MercName))
									dict_mercactive.Add(battleTarget.MercName, battleTarget.NeedActive);
							}
							bool result = false;
							Card nextSelectMerc_Card = zonePlay_friendly.GetCards().Find(
								(Card i) =>
								(!i.GetEntity().HasSelectedLettuceAbility() || !i.GetEntity().HasTag(GAME_TAG.LETTUCE_HAS_MANUALLY_SELECTED_ABILITY)) &&
								(false == dict_mercactive.TryGetValue(i.GetEntity().GetName(), out result) || result == true)
								);
							if (nextSelectMerc_Card != null)
							{
								Out.Log(string.Format("[对局中] 操作佣兵 手动选择下一个佣兵 [{0}]", nextSelectMerc_Card.GetEntity().GetName()));
								ZoneMgr.Get().DisplayLettuceAbilitiesForEntity(nextSelectMerc_Card.GetEntity());
								// 								Traverse.Create(InputManager.Get()).Method("HandleClickOnCardInBattlefield", new object[]
								// 								{
								// 									nextSelectMerc_Card.GetEntity(),
								// 									true
								// 								}).GetValue();
								Main.ResetIdle();
								return;
							}
							else
							{
								Out.Log(string.Format("[对局中] 操作佣兵 无可操作佣兵 结束回合"));
								InputManager.Get().DoEndTurnButton();
								Main.ResetIdle();
								return;
							}
						}
					}
				}
			}
			// 抉择
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.SUB_OPTION)
			{
				List<Card> friendlyCards = ChoiceCardMgr.Get().GetFriendlyCards();
				int skill_id = ChoiceCardMgr.Get().GetSubOptionParentCard().GetEntity().GetEntityId();
				int subskill_index = friendlyCards.Count - 1;
				if (dict.ContainsKey(skill_id) && dict[skill_id].SubSkillIndex != -1)
				{
					subskill_index = dict[skill_id].SubSkillIndex;
				}
				Out.Log("[对局中] 技能选择 使用抉择技能index： " + subskill_index.ToString());
				InputManager.Get().HandleClickOnSubOption(friendlyCards[subskill_index].GetEntity(), false);
				Main.ResetIdle();
				return;
			}
		}


		private List<Target> BuildTargetFromCards(List<Card> cards, Player.Side side)
		{
			List<Target> list = new List<Target>();
			foreach (Card card in cards)
			{
				bool flag_avalue = card.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || card.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER;
				if (side != Player.Side.FRIENDLY)
					flag_avalue = flag_avalue && !card.GetEntity().IsStealthed();
				List<Skill> skills = new List<Skill>();
				foreach (int id in card.GetEntity().GetLettuceAbilityEntityIDs())
				{
					Entity entity2 = GameState.Get().GetEntity(id);
					if (entity2 != null && !entity2.IsLettuceEquipment())
					{
						//有些技能没有等级，不删掉后面的数字
						string tmpName = entity2.GetName();
						if (tmpName.Length > 0 &&
							Char.IsNumber(tmpName[tmpName.Length - 1]))
							tmpName = entity2.GetName().Substring(0, entity2.GetName().Length - 1);
						skills.Add(new Skill
						{
							Name = tmpName,
							Id = entity2.GetEntityId()
						});
					}
				}
				Target item = new Target
				{
					Name = card.GetEntity().GetName(),
					Id = card.GetEntity().GetEntityId(),
					Health = card.GetEntity().GetCurrentHealth(),
					Speed = card.GetPreparedLettuceAbilitySpeedValue(),
					DefHealth = card.GetEntity().GetDefHealth(),
					Attack = card.GetEntity().GetATK(),
					Role = (HsMercenaryStrategy.TAG_ROLE)(card.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE)),
					Enable = flag_avalue,
					Skills = skills,
				};
				list.Add(item);
			}
			return list;
		}

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


		private static bool NeedCompleted()
		{
			return Main.modeConf.Value == Mode.自动解锁地图.ToString() ||
				Main.modeConf.Value == Mode.刷图.ToString() || 
				Main.modeConf.Value == Mode.挂机收菜.ToString() ||
				Main.modeConf.Value == Mode.自动解锁装备.ToString() ||
				Main.modeConf.Value == Mode.自动主线.ToString() ||
				TaskUtils.GetTaskMap() != -1;
		}


		// 最短路径
		private static int GetMinNode(LettuceMapNode node, int value, List<LettuceMapNode> nodes)
		{
			//全自动做任务，如果有赐福，需要走对应的点
			if (Main.modeConf.Value == Mode.全自动接任务做任务.ToString())
			{
				if ((TaskUtils.HaveTaskDocter && HsGameUtils.IsDoctor(node.NodeTypeId)) ||
					(TaskUtils.HaveTaskFighter && HsGameUtils.IsFighter(node.NodeTypeId)) ||
					(TaskUtils.HaveTaskCaster && HsGameUtils.IsCaster(node.NodeTypeId)) ||
					(TaskUtils.HaveTaskTank && HsGameUtils.IsTank(node.NodeTypeId)))
					return value;
			}

			// 需要不需要完成地图，只打到神秘人
			if (!Main.NeedCompleted())
			{

				if (HsGameUtils.IsMysteryNode(node.NodeTypeId))
					return value;
				if (HsGameUtils.IsBoss(node.NodeTypeId))
					return -1;
			}
			else
			{
				if (HsGameUtils.IsBoss(node.NodeTypeId))
					return value;
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
		private void UpdateAutoUnlockEquipInfo()
		{
			Cache.unlockMercID = -1;
			Cache.unlockMapID = -1;

			Dictionary<int, int> equipMapId = new Dictionary<int, int>();
			var equipTierByMap = GameDbf.BonusBountyDropChance.GetRecords();
			foreach (var item in equipTierByMap)
			{
				var tierRecord = GameDbf.LettuceEquipmentTier.GetRecord(item.LettuceEquipmentTierId);
				if (tierRecord != null)
				{
					if (EnsureMapHasUnlock(item.LettuceBountyRecord.ID) != 57)
					{
						equipMapId.Add(tierRecord.LettuceEquipmentId, item.LettuceBountyRecord.ID);
					}
				}
			}
			List<LettuceMercenary> mercenaries = CollectionManager.Get().FindOrderedMercenaries(null, new bool?(true), null, null, null).m_mercenaries;
			foreach (var merc in mercenaries) // 遍历所有的佣兵
			{
				foreach (var equip in merc.m_equipmentList) // 遍历当前佣兵的装备列表
				{
					if (!equip.Owned) // 如果当前装备没有获得
					{
						if (equipMapId.ContainsKey(equip.ID)) //如果未获得的装备已经在字典中
						{
							//将佣兵id + 地图id 加入队列
							Cache.unlockMercID = merc.ID;
							Cache.unlockMapID = equipMapId[equip.ID];
							Out.Log($"[自动解锁] 准备 [MNAME:{merc.m_mercName}] [MAPID:{equipMapId[equip.ID]}");
							break;
						}
					}
				}
			}
		}


		private readonly Harmony _harmony = new Harmony("hs.patch");
		private static bool isRunning = true;
		private static ConfigEntry<string> mapConf;
		private static ConfigEntry<string> teamNameConf;
		private static ConfigEntry<bool> autoUpdateSkillConf;
		private static ConfigEntry<bool> autoCraftConf;
		private static ConfigEntry<int> coreTeamNumConf;
		private static ConfigEntry<int> teamNumConf;
		private static ConfigEntry<string> modeConf;
		private static ConfigEntry<bool> runningConf;
		private static ConfigEntry<string> strategyConf;
		private static ConfigEntry<string> cleanTaskConf;
		private static ConfigEntry<string> awakeTimeConf;
		private static ConfigEntry<int> awakeTimeIntervalConf;
		private static ConfigEntry<bool> autoTimeScaleConf;
		private static ConfigEntry<int> pvpConcedeLine;
		private static ConfigEntry<bool> autoRerollQuest;

		private static float sleepTime;
		private static float idleTime;
		private static bool initialize;
		private static int phaseID;
		private static bool readyToHang = false;// 挂机收菜模式 下次战斗准备挂机
		private static int TimeScaleValue = 2;// 齿轮倍数
		public static string hsUnitID = "";
	}
}
