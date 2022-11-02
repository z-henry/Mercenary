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
using Blizzard.GameService.SDK.Client.Integration;
using Blizzard.T5.Core;
using Hearthstone.DataModels;

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
			GUILayout.Label(new GUIContent(hsUnitID + ' ' + PluginInfo.PLUGIN_VERSION), new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			if (modeConf.Value == Mode.一条龙.ToString())
				GUILayout.Label(new GUIContent($"阶段{(int)OnePackageService.Stage} {OnePackageService.Stage}"), new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				});
		}


		private void Awake()
		{
			if (UtilsArgu.Instance.Exists("hsunitid")) hsUnitID = UtilsArgu.Instance.Single("hsunitid");
			ConfigFile confgFile;
			if (hsUnitID.Length <= 0)
				confgFile = base.Config;
			else
				confgFile = new BepInEx.Configuration.ConfigFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx/config", hsUnitID, PluginInfo.PLUGIN_GUID + ".cfg"), false,
					new BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION));

			Main.runningConf = confgFile.Bind<bool>("配置", "插件开关", false, new ConfigDescription("插件开关", null, new object[] { "Advanced" }));
			Main.modeConf = confgFile.Bind<string>("配置", "插件运行模式", Mode.刷图.ToString(), new ConfigDescription("插件运行模式", new AcceptableValueList<string>(new string[]
			{
				Mode.刷图.ToString(),
				Mode.神秘人.ToString(),
				Mode.佣兵任务.ToString(),
				Mode.PVP.ToString(),
				Mode.挂机收菜.ToString(),
				Mode.解锁装备.ToString(),
				Mode.主线任务.ToString(),
				Mode.一条龙.ToString()
			}), Array.Empty<object>()));
			Main.teamNameConf = confgFile.Bind<string>("配置", "使用的队伍名称", "初始队伍", "使用的队伍名称");
			Main.strategyConf = confgFile.Bind<string>("配置", "战斗策略", PveNormal.StrategyName, new ConfigDescription("使用的策略,注意只有在非佣兵任务下才会生效", new AcceptableValueList<string>(StrategyHelper.GetAllStrategiesName().ToArray()), Array.Empty<object>()));
			Main.mapConf = confgFile.Bind<string>(new ConfigDefinition("配置", "要刷的地图"), "1-1", new ConfigDescription("要刷的地图", new AcceptableValueList<string>(MapUtils.GetMapNameList()), Array.Empty<object>()));
			Main.autoUpdateSkillConf = confgFile.Bind<bool>("配置", "是否自动升级技能", false, "是否自动升级技能");
			Main.autoCraftConf = confgFile.Bind<bool>("配置", "是否自动制作佣兵", false, "是否自动制作佣兵");
			Main.teamNumConf = confgFile.Bind<int>("配置", "总队伍人数", 6, new ConfigDescription("总队伍人数（PVE下生效）", new AcceptableValueRange<int>(1, 6), Array.Empty<object>()));
			Main.coreTeamNumConf = confgFile.Bind<int>("配置", "队伍核心人数", 0, new ConfigDescription("前n个佣兵不会被自动换掉（PVE下生效）", new AcceptableValueRange<int>(0, 6), Array.Empty<object>()));
			Main.cleanTaskConf = confgFile.Bind<string>(new ConfigDefinition("配置", "自动清理任务时间"), "不开启", new ConfigDescription("会定时清理长时间没完成的任务（佣兵任务生效）", new AcceptableValueList<string>(new List<string>(TaskUtils.CleanConf.Keys).ToArray()), Array.Empty<object>()));
			Main.awakeTimeConf = confgFile.Bind<string>("配置", "唤醒时间", "1999/1/1 0:0:0", new ConfigDescription("挂机收菜下的唤醒时间（只读）", null, new object[] { "Advanced" }));
			Main.awakeTimeIntervalConf = confgFile.Bind<int>("配置", "唤醒时间间隔", 22, new ConfigDescription("挂机收菜下的唤醒时间间隔(15-25随机，只读)", null, new object[] { "Advanced" }));
			Main.autoTimeScaleConf = confgFile.Bind<bool>("配置", "自动齿轮加速", false, "战斗中自动启用齿轮加速");
			Main.pvpConcedeLine = confgFile.Bind<int>("配置", "PVP投降分数线", 99999, "PVP投降分数线");
			Main.autoRerollQuest = confgFile.Bind<bool>("配置", "自动更换日周常任务", false, "自动更换日周常任务");
			Main.mercHasTaskChainConf = confgFile.Bind<int>("配置", "有任务链佣兵数量", 0, new ConfigDescription("来访者检测到的完成任务链的佣兵数量(只读)", null, new object[] { "Advanced" }));

			Main.isRunning = Main.runningConf.Value;
			if (!isRunning)
			{
				return;
			}

			Main.autoTimeScaleConf.SettingChanged += delegate
			{
				if (Main.autoTimeScaleConf.Value)
				{
					HsMod.ConfigValue.Get().TimeGearEnable = true;
					HsMod.ConfigValue.Get().TimeGearValue = TimeScaleValue.outplay;
				}
				else
				{
					HsMod.ConfigValue.Get().TimeGearEnable = false;
					HsMod.ConfigValue.Get().TimeGearValue = 1;
				}
			};
			if (Main.autoTimeScaleConf.Value)
			{
				// 刚启动时为1 稳
				HsMod.ConfigValue.Get().TimeGearEnable = true;
				HsMod.ConfigValue.Get().TimeGearValue = 1;
			}
			else
			{
				HsMod.ConfigValue.Get().TimeGearEnable = false;
				HsMod.ConfigValue.Get().TimeGearValue = 1;
			}


			this._harmony.PatchAll(typeof(Main));
		}


		private void Start()
		{
			if (Main.isRunning)
			{
				Out.Log("启动");
				DefaultTeam.TeamUnit.RegisterAll();
			}
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
				Out.Log("[来访者拦截]");
				//优先选来访者队列
				List<int> findVistor = MercConst.PriorFirst;
				if (Main.modeConf.Value == Mode.一条龙.ToString())
				{
					if (OnePackageService.Stage == OnePackageService.STAGE.获得_大德装备3)
						findVistor = new List<int> { MercConst.玛法里奥_怒风 };
					else if (OnePackageService.Stage == OnePackageService.STAGE.获得_拉格装备3)
						findVistor = new List<int> { MercConst.拉格纳罗斯 };
					else if (OnePackageService.Stage == OnePackageService.STAGE.获得_迦顿装备2)
						findVistor = new List<int> { MercConst.迦顿男爵 };
				}

				// 来访者列表
				List<(int merid, int taskchain)> visitorList = new List<(int merid, int taskchain)>();
				foreach (var iter in map.PendingVisitorSelection.VisitorOptions)
				{
					int mercid = 0;
					if (iter.HasVisitorId)
					{
						mercid = LettuceVillageDataUtil.GetMercenaryIdForVisitor(GameDbf.MercenaryVisitor.GetRecord(iter.VisitorId), null);
					}
					if (mercid == 0 && iter.HasFallbackMercenaryId)
					{
						mercid = iter.FallbackMercenaryId;
					}
					global::LettuceMercenary mercenary = CollectionManager.Get().GetMercenary((long)mercid, false, true);
					int taskChainIndex = GameDbf.GetIndex().GetTaskChainIndexForTask(iter.TaskId);
					Out.Log($"[来访者选择] [{mercid}]{mercenary.m_mercName} index:{taskChainIndex}");
					visitorList.Add((mercid, taskChainIndex));
				}

				//开始选妃
				mercHasTaskChainConf.Value = visitorList.FindAll(((int merid, int taskchain) x) => x.taskchain != -1).Count;
				int findIndex = -1;
				foreach (var iter in findVistor)
				{
					findIndex = visitorList.FindIndex(((int merid, int taskchain) x) => x.taskchain != -1 && x.merid == iter);
					if (findIndex != -1)
						break;
				}
				Out.Log($"[来访者拦截] 选择第{findIndex + 1}个来访者");
				Network.Get().MakeMercenariesMapVisitorSelection(Math.Max(0, findIndex));
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
			if (Main.modeConf.Value == Mode.PVP.ToString())
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
						MercenaryVillageTaskItemDataModel mercenaryVillageTaskItemDataModel = LettuceVillageDataUtil.CreateTaskModelFromTaskState(mercenariesVisitorState.ActiveTaskState, null);

						Out.Log($"[地图信息识别] 任务完成 [TID:{mercenariesVisitorState.ActiveTaskState.TaskId}] [TN:{mercenaryVillageTaskItemDataModel.Description}]");
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
			if (lettuceMap.HasPendingTreasureSelection && lettuceMap.PendingTreasureSelection.TreasureOptions.Count > 0)
			{
				string[] findTreasure =
				{
					//T0
					"刺杀","强化飞刺","冷酷严冬","自然之噬","雷暴之怒","强化闪电箭",
					"洄梦仙酒","月之祝福","自然之杖","元素研究","火焰之杖",
					"火炮轰击","宝箱","灵魂虹吸","吸取灵魂",
					//T1
					"火球齐射","蔓延炸弹","便携冰墙","冰霜之杖","冰霜齐射",
					
					//T2
					"火舌图腾","元素之力","精灵旗帜","冰霜之环",
					"部落的旗帜","联盟战争旗帜","暴风城战袍","血之契印","奥格瑞玛战袍",
					"隐蔽武器","近在眼前",
					//T3
					"毒蛇印记","负向平衡","正向平衡",
					"心能抗原",
					"不许摸","药膏瓶","强韧","萨隆邪铁护甲","防护之戒","火炮弹幕","乔丹法杖"
				};
				List<string> treasureList = new List<string>();
				foreach (int dbId in lettuceMap.PendingTreasureSelection.TreasureOptions)
				{
					string cardId = GameUtils.TranslateDbIdToCardId(dbId, false);
					string name = DefLoader.Get()?.GetEntityDef(cardId)?.GetName();
					Out.Log($"[宝藏选择] {name}");
					if (name.Length > 0 &&
						Char.IsNumber(name[name.Length - 1]))
						name = name.Substring(0, name.Length - 1);
					treasureList.Add(name);
				}

				int findIndex = -1;
				foreach (var iter in findTreasure)
				{
					findIndex = treasureList.IndexOf(iter);
					if (findIndex != -1)
						break;
				}
				Out.Log($"[地图信息识别] 选择第{findIndex+1}个宝藏");
				Network.Get().MakeMercenariesMapTreasureSelection(Math.Max(0, findIndex));
			}
			if (lettuceMap.HasPendingVisitorSelection && lettuceMap.PendingVisitorSelection.VisitorOptions.Count > 0)
			{
				Out.Log(string.Format("[地图信息识别] 选择第一个来访者"));
				Network.Get().MakeMercenariesMapVisitorSelection(0);
			}
		}


		private static void SelectNextNode(LettuceMap map)
		{
			Main.ResetIdle();

			List<LettuceMapNode> nodes = map.NodeData;
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
			if (!Main.NeedCompleted() && item > 3)
			{
				Out.Log(string.Format("[节点选择] 通往神秘节点数或任务赐福节点步长:{0}大于3 重开", item));
				Network.Get().RetireLettuceMap();
				Main.Sleep(2);
				return;
			}
			if (HsGameUtils.IsMonster(lettuceMapNode.NodeTypeId))
			{
				if (Main.modeConf.Value == Mode.佣兵任务.ToString()||
					Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.佣兵任务)
				{
					TaskUtils.UpdateMercTask();
				}

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
				Out.Log(string.Format("[节点选择] 非怪物节点[NID:{0}][NTYPE:{1}]", lettuceMapNode.NodeId, lettuceMapNode.NodeTypeId));
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


		private void AutoChangeTeam(int numCore, int numTotal, List<Type> teamTypes, int mercTargetCoinNeeded)
		{
			Out.Log($"[队伍编辑] 模式:{Main.modeConf.Value} 核心:{numCore} 总数:{numTotal} 预设队伍:{(teamTypes.Count > 0 ? teamTypes.ElementAt(0) : null)} 硬币目标{mercTargetCoinNeeded}");

			global::LettuceTeam lettuceTeam = HsGameUtils.GetAllTeams().Find((global::LettuceTeam t) => t.Name.Equals(Main.teamNameConf.Value));
			if (lettuceTeam == null)
				return;
			List<LettuceMercenary> mercs = lettuceTeam.GetMercs();
			List<int> list = new List<int>();
			int num = 0;
			foreach (LettuceMercenary lettuceMercenary in mercs)
			{
				if (num < numCore)
					num++;
				else
					list.Add(lettuceMercenary.ID); 
			}
			foreach (int mercId in list)
				lettuceTeam.RemoveMerc(mercId);

			// 0. 预设队伍
			if (lettuceTeam.GetMercCount() < numTotal)
			{
				foreach (var iterTeamType in teamTypes)
				{
					foreach (var merc in DefaultTeam.TeamUnit.Get(iterTeamType).TeamInfo)
					{
						if (lettuceTeam.GetMercCount() == numTotal)
							break;

						LettuceMercenary mercenary = HsGameUtils.GetMercenary(merc.id);
						if (mercenary != null && mercenary.m_owned && !lettuceTeam.IsMercInTeam(merc.id, true) &&
							HsGameUtils.CalcMercenaryCoinNeed(mercenary) > mercTargetCoinNeeded)
						{
							HsGameUtils.UpdateEq(merc.id, merc.equipIndex);
							lettuceTeam.AddMerc(mercenary, -1, null);
							Out.Log($"[队伍编辑] 添加[MID:{mercenary.ID}][MNAME:{mercenary.m_mercName}]，因为预设队伍");
						}
						//自动解锁装备特殊处理
						if (numTotal > 0 &&
							lettuceTeam.GetMercCount() == numTotal - 1 &&
							(Main.modeConf.Value == Mode.解锁装备.ToString() || Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.解锁装备))
						{
							if (Cache.unlockMercID != -1)
							{
								LettuceMercenary mercenary_boss = HsGameUtils.GetMercenary(Cache.unlockMercID);
								if (mercenary_boss != null && mercenary_boss.m_owned && !lettuceTeam.IsMercInTeam(mercenary_boss.ID, true))
								{
									lettuceTeam.AddMerc(mercenary_boss, -1, null);
									Out.Log($"[队伍编辑] 添加[MID:{mercenary_boss.ID}][MNAME:{mercenary_boss.m_mercName}]，因为自动解锁装备_老板");
								}
							}
						}
					}
				}
			}
			// 1. 做任务模式
			if (lettuceTeam.GetMercCount() < numTotal)
			{
				if (Main.modeConf.Value == Mode.佣兵任务.ToString() ||
					Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.佣兵任务)
				{
					foreach (Task task in TaskUtils.GetTasks())
					{
						foreach (MercenaryEntity mercenaryEntity in task.Mercenaries)
						{
							if (lettuceTeam.GetMercCount() == numTotal)
								break;

							LettuceMercenary mercenary = HsGameUtils.GetMercenary(mercenaryEntity.ID);
							if (mercenary != null && 
								mercenary.m_owned && 
								!lettuceTeam.IsMercInTeam(mercenaryEntity.ID, true))
							{
								HsGameUtils.UpdateEq(mercenaryEntity.ID, mercenaryEntity.Equipment);
								lettuceTeam.AddMerc(mercenary, -1, null);
								Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}][EID:{2}]，因为佣兵任务",
									mercenaryEntity.ID, mercenaryEntity.Name, mercenaryEntity.Equipment));

							}
						}
					}
				}
			}
			List<LettuceMercenary> mercenaries = (
				from x in CollectionManager.Get().FindMercenaries(null, true, null, null, null).m_mercenaries
				where x.m_owned == true && x.m_isFullyUpgraded == false && HsGameUtils.CalcMercenaryCoinNeed(x) > 0
				orderby (MercConst.PriorFirst.IndexOf(x.ID) == -1 ? int.MaxValue : MercConst.PriorFirst.IndexOf(x.ID)) ascending
				select x
				).ToList<global::LettuceMercenary>();
			// 2. 匹配未满级
			if (lettuceTeam.GetMercCount() < numTotal)
			{
				foreach (LettuceMercenary lettuceMercenary2 in mercenaries)
				{
					if (lettuceTeam.GetMercCount() == numTotal)
						break;

					if (!lettuceTeam.IsMercInTeam(lettuceMercenary2.ID, true) &&
						!lettuceMercenary2.IsMaxLevel())
					{
						lettuceTeam.AddMerc(lettuceMercenary2, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，因为未满级",
							lettuceMercenary2.ID, lettuceMercenary2.m_mercName));
					}
				}
			}
			// 3. 匹配优先级高
			if (lettuceTeam.GetMercCount() < numTotal)
			{
				foreach (int mercid in MercConst.PriorFirst)
				{
					if (lettuceTeam.GetMercCount() == numTotal)
						break;

					LettuceMercenary mercenary = HsGameUtils.GetMercenary(mercid);
					if (mercenary != null &&
						mercenary.m_owned &&
						!lettuceTeam.IsMercInTeam(mercid, true) &&
						!mercenary.m_isFullyUpgraded &&
						HsGameUtils.CalcMercenaryCoinNeed(mercenary) > 0)
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，因为满级优先级设置高",
							mercenary.ID, mercenary.m_mercName));
					}
				}
			}
			// 4. 匹配优先级中
			if (lettuceTeam.GetMercCount() < numTotal)
			{
				foreach (LettuceMercenary mercenary in mercenaries)
				{
					if (lettuceTeam.GetMercCount() == numTotal)
						break;

					if (!lettuceTeam.IsMercInTeam(mercenary.ID, true))
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，满级",
							mercenary.ID, mercenary.m_mercName));
					}
				}
			}
			// 5. 匹配优先级低
			if (lettuceTeam.GetMercCount() < numTotal)
			{
				foreach (LettuceMercenary mercenary in mercenaries)
				{
					if (lettuceTeam.GetMercCount() == numTotal)
						break;

					if (!lettuceTeam.IsMercInTeam(mercenary.ID, true))
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，满级优先级设置低",
							mercenary.ID, mercenary.m_mercName));
					}
				}
			}
			// 6. 全满补位
			if (lettuceTeam.GetMercCount() < numTotal)
			{
				foreach (LettuceMercenary mercenary in CollectionManager.Get().FindMercenaries(null, true, null, null, null).m_mercenaries)
				{
					if (lettuceTeam.GetMercCount() == numTotal)
						break;

					if (!lettuceTeam.IsMercInTeam(mercenary.ID, true))
					{
						lettuceTeam.AddMerc(mercenary, -1, null);
						Out.Log(string.Format("[队伍编辑] 添加[MID:{0}][MNAME:{1}]，全满补位",
							mercenary.ID, mercenary.m_mercName));
					}
				}
			}

			lettuceTeam.SendChanges();
		}


		private static void Sleep(float time)
		{
			Main.sleepTime += (float)time;
			//Out.MyLogger(BepInEx.Logging.LogLevel.Warning, $"sleep {time}");
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
				CollectionManager.FindMercenariesResult result = CollectionManager.Get().FindMercenaries();
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
			if (Input.GetKeyUp(KeyCode.F4))
			{
				try
				{
					Out.Log("=======================================");
					foreach (Target target_iter in BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING))
						Out.Log($"敌方场面 name:{target_iter.Name} ForstEnhance:{target_iter.ForstEnhance} ForstWeak:{target_iter.ForstWeak}");
					Out.Log("---------------------------------------");
					foreach (Target target_iter in BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards(), Player.Side.FRIENDLY))
						Out.Log($"友方场面 name:{target_iter.Name} ForstEnhance:{target_iter.ForstEnhance} ForstWeak:{target_iter.ForstWeak}");
					Out.Log("=======================================");
				}
				catch (Exception ex)
				{
					Console.WriteLine("空间名：" + ex.Source + "；" + '\n' +
						"方法名：" + ex.TargetSite + '\n' +
						"故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
						"错误提示：" + ex.Message + '\n' +
						"XXX：" + ex.ToString()
						);
				}
			}

			if (!Main.isRunning)
			{
				return;
			}
			this.CheckIdleTime();
			if ((double)(Time.realtimeSinceStartup - Main.sleepTime) <= 0.1)
			{
				return;
			}
			Main.sleepTime = Time.realtimeSinceStartup;
			if (!Main.initialize)
			{
				this.GameInit();
				return;
			}

			HandleDialogs();

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
				Sleep(1);
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
				if (Main.modeConf.Value != Mode.PVP.ToString())
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

				Out.Log($"[状态] 目前处于悬赏面板，切换到队伍选择，休息3秒");
				HsGameUtils.SelectBoss(57);
				Main.ResetIdle();
				Main.Sleep(3);
				return;
			}
			#endregion

			#region 队伍选择
			if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT && gameState == null)
			{
				this.AutoUpdateSkill();
				this.AutoCraft();
				this.AckMercFullLevel();


				// 更新缓存，一定要更新一条龙缓存
				if (Main.modeConf.Value == Mode.一条龙.ToString())
				{
					OnePackageService.UpdateStage();
				}
				if (Main.modeConf.Value == Mode.佣兵任务.ToString() ||
					Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.佣兵任务)
				{
					TaskUtils.UpdateMercTask();
				}
				else if (Main.modeConf.Value == Mode.解锁装备.ToString() ||
					Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.解锁装备)
				{
					UpdateAutoUnlockEquipInfo();
				}
				else if (Main.modeConf.Value == Mode.主线任务.ToString() ||
					Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.主线任务)
				{
					TaskUtils.UpdateMainLineTask();
				}

				//参数设置
				int numCore = coreTeamNumConf.Value, numTotal = teamNumConf.Value,
					mapId = this.GetMapId(),
					mercTargetCoinNeeded = -1;// 对于预设队伍，小于此硬币需求的佣兵，不再携带
				List<Type> teamTypes = new List<Type>();// 预设队伍
				if (Main.modeConf.Value == Mode.解锁装备.ToString() ||
					Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.解锁装备 ||
					Main.modeConf.Value == Mode.主线任务.ToString() ||
					Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.主线任务)
				{
					numCore = 0;
					numTotal = 6;
					teamTypes.Add(MapUtils.GetMapByID(mapId).TeamType);
				}
				else if (Main.modeConf.Value == Mode.一条龙.ToString())
				{
					numCore = 0;
					numTotal = OnePackageService.TranslateCurrentStage().m_teamTotal;
					teamTypes = OnePackageService.TranslateCurrentStage().m_teamTypes ?? teamTypes;
					mercTargetCoinNeeded = OnePackageService.TranslateCurrentStage().m_TargetCoinNeeded;
				}

				this.AutoChangeTeam(numCore, numTotal, teamTypes, mercTargetCoinNeeded);
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
				LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(mapId);
				lettuceSceneTransitionPayload.m_TeamId = lettuceTeam2.ID;
				lettuceSceneTransitionPayload.m_SelectedBounty = record;
				lettuceSceneTransitionPayload.m_SelectedBountySet = record.BountySetRecord;
				lettuceSceneTransitionPayload.m_DifficultyMode = record.DifficultyMode;
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
					//关闭对局内齿轮(传统模式设置齿轮，会导致卡结算)
					if (Main.autoTimeScaleConf.Value == true &&
						gameType != GameType.GT_RANKED)
					{
						HsMod.ConfigValue.Get().TimeGearEnable = true;
						HsMod.ConfigValue.Get().TimeGearValue = Main.TimeScaleValue.outplay;
					}

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
									if (xpChange.RewardTrackType != 1)
										continue;
									Out.Log(string.Format("[对局结束] 战令信息 {0} 等级:{1} 经验:{2} {3} {4} {5}",
										gameResult, xpChange.CurrLevel, xpChange.CurrXp, xpChange.RewardSourceType, xpChange.RewardSourceId, xpChange.RewardTrackType));
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
			foreach (LettuceMercenary lettuceMercenary in CollectionManager.Get().FindMercenaries(null, new bool?(true), null, null, null).m_mercenaries)
			{
				if (lettuceMercenary.IsReadyForCrafting())
				{
					Network.Get().CraftMercenary(lettuceMercenary.ID);
					Out.Log(string.Format("[制作佣兵] [MID:{0}]", lettuceMercenary.ID));
					Main.mercHasTaskChainConf.Value++;
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


		private static int EnsureMapHasUnlock(int id)
		{
			LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(id);

			// 以下情况需要顺序解锁
			if (record.RequiredCompletedBounty > 0)
			{
				// 1. 非第一小关，前置没解锁
				if (false == MercenariesDataUtil.IsBountyComplete(record.RequiredCompletedBounty))
					return MapUtils.GetUnCompleteMap();
			}
			else
			{
				// 2. 1-1 h1-1 怎么可能没解锁？
				if (id == 57 || id == 85)
					return id;
				// 3. 2-1 h2-1，1-9没解锁
				if ((id == 67 || id == 94) && false == MercenariesDataUtil.IsBountyComplete(65))
					return MapUtils.GetUnCompleteMap();
				// 4. 3-1 h3-1，2-6没解锁
				if ((id == 73 || id == 100) && false == MercenariesDataUtil.IsBountyComplete(72))
					return MapUtils.GetUnCompleteMap();
				// 4. 3以后的-1，3-6没解锁
				else if (false == MercenariesDataUtil.IsBountyComplete(78))
					return MapUtils.GetUnCompleteMap();
			}
			return id;
		}


		private int GetMapId()
		{
			int result = 57;
			if (Main.modeConf.Value == Mode.佣兵任务.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.佣兵任务)
			{
				int taskMap = TaskUtils.GetTaskMap();
				if (taskMap != -1)
				{
					return EnsureMapHasUnlock(taskMap);
				}
				return EnsureMapHasUnlock(MapUtils.GetMapId("2-5"));
			}
			else if (Main.modeConf.Value == Mode.主线任务.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.主线任务)
			{
				int taskMap = TaskUtils.GetTaskMap();
				if (taskMap != -1)
				{
					return EnsureMapHasUnlock(taskMap);
				}
				return MapUtils.GetUnCompleteMap();
			}
			else if (Main.modeConf.Value == Mode.解锁装备.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.解锁装备)
			{
				int taskMap = Cache.unlockMapID;
				if (taskMap != -1)
				{
					return EnsureMapHasUnlock(taskMap);
				}

				return EnsureMapHasUnlock(MapUtils.GetMapId("1-1"));
			}
			else if (Main.modeConf.Value == Mode.一条龙.ToString())
			{
				int taskMap = OnePackageService.TranslateCurrentStage().m_mapid;
				if (taskMap != -1)
				{
					return EnsureMapHasUnlock(taskMap);
				}

				return EnsureMapHasUnlock(MapUtils.GetMapId("1-1"));
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
				return EnsureMapHasUnlock(record.ID);
			}
		}


		private void CheckIdleTime()
		{
			int scale = Main.autoTimeScaleConf.Value ? Main.TimeScaleValue.inplay : 1;
			Main.idleTime += (Time.deltaTime / scale);
			if (Main.idleTime > 240f)
			{
				if (GameState.Get() != null)
				{
					if (Main.modeConf.Value != Mode.PVP.ToString())
					{
						Out.Log("[IDLE]240s 投降");
						GameState.Get().Concede();
					}
				}
				else
				{
					Application.Quit();
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
			//Out.MyLogger(BepInEx.Logging.LogLevel.Warning, $"{GameState.Get().GetResponseMode()}  {phaseID}");
			if (Main.phaseID == 3 ||
				Main.phaseID == 0)
			{
				Sleep(1);
				return;
			}

			//对局内齿轮
			if (Main.autoTimeScaleConf.Value == true)
			{
				HsMod.ConfigValue.Get().TimeGearEnable = true;
				HsMod.ConfigValue.Get().TimeGearValue = Main.TimeScaleValue.inplay;
			}

			if (EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_NO_MORE_PLAYS)
			{
				Out.Log("[对局中] 点击结束按钮");
				InputManager.Get().DoEndTurnButton();
				Sleep(1);
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
			string strategy_name =  Main.strategyConf.Value;
			if (Main.modeConf.Value == Mode.佣兵任务.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.佣兵任务)
				strategy_name = "_Sys_Default";
			List<BattleTarget> battleTargets = StrategyHelper.GetStrategy(strategy_name).GetBattleTargets(
				GameState.Get().GetTurn(),
				this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING),
				this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards(), Player.Side.FRIENDLY),
				this.BuildTargetFromCards(ZoneMgr.Get().FindZoneOfType<ZoneGraveyard>(Player.Side.OPPOSING).GetCards(), Player.Side.OPPOSING)
				);
			Dictionary<int, BattleTarget> dict = new Dictionary<int, BattleTarget>();// 技能id目标字典
			foreach (BattleTarget battleTarget in battleTargets)
			{
				if (battleTarget.SkillId == -1)
					continue;
				if (!dict.ContainsKey(battleTarget.SkillId))
					dict.Add(battleTarget.SkillId, battleTarget);
			}
			Dictionary<string, bool> dict_mercactive = new Dictionary<string, bool>();// 佣兵静止字典
			foreach (BattleTarget battleTarget in battleTargets)
			{
				if (battleTarget.MercName.Length <= 0)
					continue;
				if (!dict_mercactive.ContainsKey(battleTarget.MercName))
					dict_mercactive.Add(battleTarget.MercName, battleTarget.NeedActive);
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
				Main.ResetIdle();
			}
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.OPTION)
			{
				// 上怪
				if (Main.phaseID == 1 &&
					EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_YOUR_TURN)
				{
					if (modeConf.Value == Mode.挂机收菜.ToString() && readyToHang == true)
					{
						System.Random rd = new System.Random();
						awakeTimeIntervalConf.Value = rd.Next(0, 10) + 15;
						Out.Log(string.Format("[战斗中] 初次进入战斗，休息{0}min后再见~", awakeTimeIntervalConf.Value));
						awakeTimeConf.Value = DateTime.Now.AddMinutes(awakeTimeIntervalConf.Value).ToString("G");
						Application.Quit();
						return;
					}

					// pve上怪
					if (GameMgr.Get().GetGameType() == GameType.GT_MERCENARIES_PVE)
					{
						InputManager.Get().DoEndTurnButton();
						Out.Log("[佣兵登场]");
						Sleep(1);
						return;
					}
					// pvp上怪
					else
					{
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
							foreach (Card card_iter in zoneOppositeDeck.GetCards())
								dictOppositeRoleCount[(HsMercenaryStrategy.TAG_ROLE)(card_iter.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE))]++;
							foreach (Card card_iter in zoneOppositeHand.GetCards())
								dictOppositeRoleCount[(HsMercenaryStrategy.TAG_ROLE)(card_iter.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE))]++;

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
								Sleep(2);
							}
						}
						Main.ResetIdle();
						return;
					}
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
							if (card.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE) == TAG_ROLE.INVALID ||
								card.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE) == TAG_ROLE.NEUTRAL ||
								card.GetEntity().GetName() == "大法师卡德加")
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

					// 操作佣兵
					Entity currentSelectMerc_Entity = ZoneMgr.Get().GetLettuceAbilitiesSourceEntity();
					bool result = false;
					Card nextSelectMerc_Card = zonePlay_friendly.GetCards().Find( (Card i) =>
						(!i.GetEntity().HasSelectedLettuceAbility() || !i.GetEntity().HasTag(GAME_TAG.LETTUCE_HAS_MANUALLY_SELECTED_ABILITY)) &&
						(false == dict_mercactive.TryGetValue(i.GetEntity().GetName(), out result) || result == true)
						);
					// 无可操作佣兵
					if (nextSelectMerc_Card == null)
					{
						Out.Log(string.Format("[对局中] 操作佣兵 无可操作佣兵 结束回合"));
						InputManager.Get().DoEndTurnButton();
						Sleep(1);
						Main.ResetIdle();
						return;
					}
					// 操作的佣兵与选中的顺序目标不一样
					if (currentSelectMerc_Entity == null ||
						currentSelectMerc_Entity.GetEntityId() != nextSelectMerc_Card.GetEntity().GetEntityId())
					{
						Out.Log(string.Format("[对局中] 操作佣兵 手动调整到[{0}]", nextSelectMerc_Card.GetEntity().GetName()));
						ZoneMgr.Get().DisplayLettuceAbilitiesForEntity(nextSelectMerc_Card.GetEntity());
						RemoteActionHandler.Get().NotifyOpponentOfSelection(nextSelectMerc_Card.GetEntity().GetEntityId());
						Main.ResetIdle();
						Sleep(0.5f);
						return;
					}
					// 佣兵技能
					else
					{
						Out.Log(string.Format("[对局中] 操作佣兵 [{0}]", currentSelectMerc_Entity.GetName()));
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
					Role = (HsMercenaryStrategy.TAG_ROLE)card.GetEntity().GetTag<TAG_ROLE>(GAME_TAG.LETTUCE_ROLE),
					Enable = flag_avalue,
					Skills = skills,
					ForstEnhance = card.GetEntity().GetTag(GAME_TAG.SPELLPOWER_FROST),
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
			return Main.modeConf.Value == Mode.刷图.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.刷图 ||
				Main.modeConf.Value == Mode.挂机收菜.ToString() ||
				Main.modeConf.Value == Mode.解锁装备.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.解锁装备 ||
				Main.modeConf.Value == Mode.主线任务.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.主线任务 ||
				Main.modeConf.Value == Mode.佣兵任务.ToString() && TaskUtils.GetTaskMap() != -1 ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.佣兵任务 && TaskUtils.GetTaskMap() != -1;
		}


		// 最短路径
		private static int GetMinNode(LettuceMapNode node, int value, List<LettuceMapNode> nodes)
		{
			//佣兵任务，如果有赐福，需要走对应的点
			if (Main.modeConf.Value == Mode.佣兵任务.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.佣兵任务)
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
			int num = 0;
			if (Main.modeConf.Value == Mode.主线任务.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.主线任务 ||
				Main.modeConf.Value == Mode.解锁装备.ToString() ||
				Main.modeConf.Value == Mode.一条龙.ToString() && OnePackageService.TranslateCurrentStage().m_mode == Mode.解锁装备 ||
				Main.modeConf.Value == Mode.刷图.ToString() && Main.teamNameConf.Value == "刷图")
			{
				if (HsGameUtils.IsCaster(node.NodeTypeId) ||
					HsGameUtils.IsFighter(node.NodeTypeId) ||
					HsGameUtils.IsTank(node.NodeTypeId) ||
					HsGameUtils.IsMysteryNode(node.NodeTypeId))
					num = 99;
				else if (HsGameUtils.IsMonster(node.NodeTypeId))
					num = 1;
				else num = 0;
			}
			else if (HsGameUtils.IsMysteryNode(node.NodeTypeId))
				num = -3;
			else
			{
				num = (!HsGameUtils.IsMonster(node.NodeTypeId)) ? 0 : 1;
			}

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
		public static bool UpdateAutoUnlockEquipInfo()
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
					if (EnsureMapHasUnlock(item.LettuceBountyRecord.ID) == item.LettuceBountyRecord.ID)
					{
						equipMapId.Add(tierRecord.LettuceEquipmentId, item.LettuceBountyRecord.ID);
					}
				}
			}
			List<LettuceMercenary> mercenaries = (
				from x in CollectionManager.Get().FindMercenaries(null, true, null, null, null).m_mercenaries
				where x.m_owned == true
				orderby (MercConst.EquipFirst.IndexOf(x.ID) == -1 ? int.MaxValue : MercConst.EquipFirst.IndexOf(x.ID)) ascending
				select x
				).ToList<global::LettuceMercenary>();
			foreach (var merc in mercenaries) // 遍历所有的佣兵
			{
				foreach (var equip in merc.m_equipmentList) // 遍历当前佣兵的装备列表
				{
					if (equip.Owned)
						continue;
					if (equipMapId.ContainsKey(equip.ID)) //如果未获得的装备已经在字典中
					{
						//将佣兵id + 地图id 加入队列
						Cache.unlockMercID = merc.ID;
						Cache.unlockMapID = equipMapId[equip.ID];
						Out.Log($"[自动解锁] 准备 [MNAME:{merc.m_mercName}] [MAPID:{equipMapId[equip.ID]}]");
						return true;
					}
				}
			}
			return false;
		}
		private void AckMercFullLevel()
		{
			Out.Log("[成就领取]");
			foreach (AchievementDbfRecord achievementDbfRecord in GameDbf.Achievement.GetRecords((AchievementDbfRecord x) => x.AchievementSection == 327, -1))
			{
				AchievementDataModel achievementDataModel = AchievementManager.Get().GetAchievementDataModel(achievementDbfRecord.ID);
				if (AchievementManager.AchievementStatus.COMPLETED != achievementDataModel.Status)
					continue;

				Network.Get().ClaimAchievementReward(achievementDataModel.ID, 0);
				Out.Log($"[成就领取] {achievementDataModel.Name}:{achievementDataModel.Description}");
			}
		}
		private void HandleDialogs()
		{
			DialogManager dialogManager = DialogManager.Get();
			if (dialogManager == null)
				return;

			if (!dialogManager.ShowingDialog())
				return;

			DialogBase currentDialog = (DialogBase)Traverse.Create(dialogManager).Field("m_currentDialog").GetValue();
			if (currentDialog == null)
				return;

			string realClassName = currentDialog.GetType().Name;
			Out.Log($"[HandleDialogs] A dialog of type {realClassName} is showing.");
		}


		private readonly Harmony _harmony = new Harmony("hs.patch");
		private static bool isRunning = true;
		private static ConfigEntry<string> mapConf;
		private static ConfigEntry<string> teamNameConf;
		private static ConfigEntry<bool> autoUpdateSkillConf;
		private static ConfigEntry<bool> autoCraftConf;
		private static ConfigEntry<int> coreTeamNumConf;
		private static ConfigEntry<int> teamNumConf;
		public static ConfigEntry<string> modeConf;
		private static ConfigEntry<bool> runningConf;
		private static ConfigEntry<string> strategyConf;
		public static ConfigEntry<string> cleanTaskConf;
		private static ConfigEntry<string> awakeTimeConf;
		private static ConfigEntry<int> awakeTimeIntervalConf;
		private static ConfigEntry<bool> autoTimeScaleConf;
		private static ConfigEntry<int> pvpConcedeLine;
		private static ConfigEntry<bool> autoRerollQuest;
		public static ConfigEntry<int> mercHasTaskChainConf;

		private static float sleepTime;
		private static float idleTime;
		private static bool initialize;
		private static int phaseID;
		private static bool readyToHang = false;// 挂机收菜模式 下次战斗准备挂机
		private static (int outplay, int inplay) TimeScaleValue = (2,8);// 齿轮倍数
		public static string hsUnitID = "";
	}
}
