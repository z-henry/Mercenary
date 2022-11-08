using Mercenary.MyInterface;
using Mercenary.Strategy;
using System.Collections.Generic;

namespace Mercenary.InterfaceDefault
{
	public static class DefaultStrategy
	{
		public static (int hand_index, int play_index) DefaultStrategyMethod_GetEnterOrder<T>(this T obj,
			List<Target> hand_mercenaries, List<Target> play_mercenaries,
			Dictionary<MY_TAG_ROLE, int> dictOppositeRoleCount,
			List<Target> targets_opposite, List<Target> targets_opposite_graveyrad
			) where T : IStrategy
		{
			int 鞭笞者特里高雷 = StrategyUtils.FindMercenaryIndexForName("鞭笞者特里高雷", play_mercenaries);
			if (鞭笞者特里高雷 == 0)
				return (0, 鞭笞者特里高雷);
			else if (鞭笞者特里高雷 > 0)
				return (0, 鞭笞者特里高雷 + 1);
			return (0, play_mercenaries.Count);
		}

		public static List<BattleTarget> DefaultStrategyMethod_GetBattleTargets<T>(this T obj,
			int turn, List<Target> targets_opposite_all, List<Target> targets_friendly_all, List<Target> targets_opposite_graveyrad
			) where T : IStrategy
		{
			List<BattleTarget> list = new List<BattleTarget>();
			var targets_opposite_valid = targets_opposite_all.FindAll((Target t) => t.Enable == true);
			var targets_friendly = targets_friendly_all.FindAll((Target t) => t.Enable == true);

			foreach (var merc_iter in targets_friendly_all)
			{
				if (merc_iter.Name == "安东尼达斯")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
						//特殊关卡
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.CASTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));
					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("火球术", merc_iter)?.Id ?? -1,
						SkillName = "火球术",
					});
				}
				else if (merc_iter.Name == "赤精")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
						//特殊关卡
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));
					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("烈焰之歌", merc_iter)?.Id ?? -1,
						SkillName = "烈焰之歌",
					});
				}
				else if (merc_iter.Name == "巴琳达·斯通赫尔斯")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							"魔刃豹秘使","玛克扎尔王子",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打蓝
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.CASTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));
					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("烈焰之刺", merc_iter)?.Id ?? -1,
						SkillName = "烈焰之刺",
					});
				}
				else if (merc_iter.Name == "迦顿男爵")
				{
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("地狱火", merc_iter)?.Id ?? -1,
						SkillName = "地狱火",
					});
				}
				else if (merc_iter.Name == "拉格纳罗斯")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							"魔刃豹秘使",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打绿色
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.FIGHTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMaxHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));
					Target common_target = StrategyUtils.GetTarget(focusTargets);

					if (null == StrategyUtils.FindMercenaryForName("赤精", targets_friendly_all)
					&& null == StrategyUtils.FindMercenaryForName("巴琳达·斯通赫尔斯", targets_friendly_all)
					&& null == StrategyUtils.FindMercenaryForName("安东尼达斯", targets_friendly_all)
					&& null != StrategyUtils.FindMercenaryForName("迦顿男爵", targets_friendly_all)
					|| merc_iter.Health < 40)
					{
						list.Add(new BattleTarget()
						{
							TargetId = common_target?.Id ?? -1,
							TargetName = common_target?.Name ?? "",
							SkillId = StrategyUtils.FindSkillForName("熔岩冲击", merc_iter)?.Id ?? -1,
							SkillName = "熔岩冲击",
						});
					}
					else
					{
						list.Add(new BattleTarget()
						{
							SkillId = StrategyUtils.FindSkillForName("死吧，虫子", merc_iter)?.Id ?? -1,
							SkillName = "死吧，虫子",
						});
					}
				}
				else if (merc_iter.Name.IndexOf("次级水元素") == 0)
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							"触手恐吓者","奥秘吞噬者","龙人打击者","军情七处特工","夜魇骑士",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					focusTargets.Add(StrategyUtils.FindSlowestTarget(targets_opposite_all));
					Target common_target = StrategyUtils.GetTarget(focusTargets);
					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("攻击", merc_iter)?.Id ?? -1,
						SkillName = "攻击",
					});
				}
				else if (merc_iter.Name.IndexOf("次级火元素") == 0)
				{
					Target common_target = StrategyUtils.FindSlowestTarget(targets_opposite_all);
					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("喷吐火焰", merc_iter)?.Id ?? -1,
						SkillName = "喷吐火焰",
					});
				}
				else if (merc_iter.Name.IndexOf("绝命炸药") == 0)
				{
					Target common_target = StrategyUtils.FindSlowestTarget(targets_opposite_valid);
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("维修引线", merc_iter)?.Id ?? -1,
						SkillName = "维修引线",
					});
				}
				else if (merc_iter.Name == "冰雪之王洛克霍拉")
				{
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("冰雹", merc_iter)?.Id ?? -1,
						SkillName = "冰雹",
					});
				}
				else if (merc_iter.Name == "吉安娜·普罗德摩尔")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
						//特殊关卡
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));
					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("浮冰术", merc_iter)?.Id ?? -1,
						SkillName = "浮冰术",
					});
				}
				else if (merc_iter.Name == "瓦尔登·晨拥")
				{
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("冰风暴", merc_iter)?.Id ?? -1,
						SkillName = "冰风暴",
					});
				}
				else if (merc_iter.Name == "玛法里奥·怒风")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							//Boss关
							"猜忌的化身","颓丧的化身","恐惧的化身","索瑞森大帝","甜品货车","达基萨斯将军",
							"雷德·黑手","拉佐格尔",
							//小怪
							"卡德加","触手恐吓者","奥秘吞噬者","龙人打击者",
							"雷矛特种兵","雷矛狂战士","雷矛羊骑兵","始祖龟预言者","穴居人元素师",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打绿色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.FIGHTER)));
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					Target common_target = StrategyUtils.GetTarget(focusTargets);
					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("塞纳里奥波动", merc_iter)?.Id ?? -1,
						SkillName = "塞纳里奥波动",
					});
				}
				else if (merc_iter.Name == "古夫·符文图腾")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							//Boss关
							"猜忌的化身","颓丧的化身","恐惧的化身","索瑞森大帝","甜品货车","达基萨斯将军",
							//小怪
							"卡德加","触手恐吓者","奥秘吞噬者","龙人打击者",
							"雷矛特种兵","雷矛狂战士","雷矛羊骑兵","始祖龟预言者","穴居人元素师",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.FIGHTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));

					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("活体荆棘", merc_iter)?.Id ?? -1,
						SkillName = "活体荆棘",
					});
				}
				else if (merc_iter.Name == "奈姆希·灵沼")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							//Boss关
							"猜忌的化身","颓丧的化身","恐惧的化身","索瑞森大帝","甜品货车","达基萨斯将军",
							"雷德·黑手","拉佐格尔",
							//小怪
							"卡德加","触手恐吓者","奥秘吞噬者","龙人打击者",
							"雷矛特种兵","雷矛狂战士","雷矛羊骑兵","始祖龟预言者","穴居人元素师",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.FIGHTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));

					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("沼沼重击", merc_iter)?.Id ?? -1,
						SkillName = "沼沼重击",
					});
				}
				else if (merc_iter.Name == "布鲁坎")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							//Boss关
							"猜忌的化身","颓丧的化身","恐惧的化身","索瑞森大帝","甜品货车","达基萨斯将军",
							"雷德·黑手","拉佐格尔",
							//小怪
							"卡德加","触手恐吓者","奥秘吞噬者","龙人打击者",
							"雷矛特种兵","雷矛狂战士","雷矛羊骑兵","始祖龟预言者","穴居人元素师",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.FIGHTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));

					Target common_target = StrategyUtils.GetTarget(focusTargets);
					if (null == StrategyUtils.FindMercenaryForName("玛法里奥·怒风", targets_friendly_all)
					&& null != StrategyUtils.FindMercenaryForName("古夫·符文图腾", targets_friendly_all)
					|| merc_iter.Health < 40
					|| null != StrategyUtils.FindMercenaryForName("卡德加", targets_opposite_all))
					{
						list.Add(new BattleTarget()
						{
							TargetId = common_target?.Id ?? -1,
							TargetName = common_target?.Name ?? "",
							SkillId = StrategyUtils.FindSkillForName("陷足泥泞", merc_iter)?.Id ?? -1,
							SkillName = "陷足泥泞",
						});
					}
					else
						list.Add(new BattleTarget()
						{
							TargetId = common_target?.Id ?? -1,
							TargetName = common_target?.Name ?? "",
							SkillId = StrategyUtils.FindSkillForName("闪电箭", merc_iter)?.Id ?? -1,
							SkillName = "闪电箭",
						});
				}
				else if (merc_iter.Name == "安娜科德拉")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							//Boss关
							"猜忌的化身","颓丧的化身","恐惧的化身","索瑞森大帝","甜品货车","达基萨斯将军",
							"雷德·黑手","拉佐格尔",
							//小怪
							"卡德加","触手恐吓者","奥秘吞噬者","龙人打击者",
							"雷矛特种兵","雷矛狂战士","雷矛羊骑兵","始祖龟预言者","穴居人元素师",
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打蓝色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.CASTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));

					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("毒蛇噬咬", merc_iter)?.Id ?? -1,
						SkillName = "毒蛇噬咬",
					});
				}
				else if (merc_iter.Name == "贝恩·血蹄")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
						//特殊关卡
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == StrategyUtils.restrain_TAG_ROLE(merc_iter.Role))));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));

					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("大地母亲之怒", merc_iter)?.Id ?? -1,
						SkillName = "大地母亲之怒",
					});
				}
				else if (merc_iter.Name == "泰兰德·语风")
				{
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("奥术齐射", merc_iter)?.Id ?? -1,
						SkillName = "奥术齐射",
					});
				}
				else if (merc_iter.Name == "斯卡布斯·刀油")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
						//特殊关卡
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}
					//打蓝色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.CASTER)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));
					Target common_target = StrategyUtils.GetTarget(focusTargets);

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("暗影之刃", merc_iter)?.Id ?? -1,
						SkillName = "暗影之刃",
					});
					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("战术打击", merc_iter)?.Id ?? -1,
						SkillName = "战术打击",
					});
				}
				else if (merc_iter.Name == "魔像师卡扎库斯")
				{
					if (merc_iter.Health <= 30)
						list.Add(new BattleTarget()
						{
							SkillId = StrategyUtils.FindSkillForName("暗影之爪", merc_iter)?.Id ?? -1,
							SkillName = "暗影之爪",
						});
					else
						list.Add(new BattleTarget()
						{
							SkillId = StrategyUtils.FindSkillForName("构筑魔像", merc_iter)?.Id ?? -1,
							SubSkillIndex = 2,
							SkillName = "构筑魔像-2",
						});
				}
				else if (merc_iter.Name == "海盗帕奇斯")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							"夜魇骑士"
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_valid));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));

					Target common_target = StrategyUtils.GetTarget(focusTargets);
					if (null != StrategyUtils.FindMercenaryForName("重拳先生", targets_friendly_all))
						list.Add(new BattleTarget()
						{
							SkillId = StrategyUtils.FindSkillForName("眼魔船长", merc_iter)?.Id ?? -1,
							SkillName = "眼魔船长",
						});
					else
						list.Add(new BattleTarget()
						{
							TargetId = common_target?.Id ?? -1,
							TargetName = common_target?.Name ?? "",
							SkillId = StrategyUtils.FindSkillForName("眼魔船工", merc_iter)?.Id ?? -1,
							SkillName = "眼魔船工",
						});
				}
				else if (merc_iter.Name.IndexOf("小帕奇") == 0)
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{  "夜魇骑士"
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_valid));
					}
					//打红色
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role == MY_TAG_ROLE.TANK)));
					//打佣兵
					focusTargets.Add(StrategyUtils.FindMinHealthTarget(
						targets_opposite_valid.FindAll((Target t) => t.Role != MY_TAG_ROLE.NEUTRAL && t.Role != MY_TAG_ROLE.INVALID)));

					Target common_target = StrategyUtils.GetTarget(focusTargets);
					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("攻击", merc_iter)?.Id ?? -1,
						SkillName = "攻击",
					});
				}
				else if (merc_iter.Name == "变装大师")
				{
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("流变打击", merc_iter)?.Id ?? -1,
						SkillName = "流变打击",
					});
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("邪恶计谋", merc_iter)?.Id ?? -1,
						SkillName = "邪恶计谋",
					});
				}
				else if (merc_iter.Name == "重拳先生")
				{
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("落水追击", merc_iter)?.Id ?? -1,
						SkillName = "落水追击",
					});
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("重拳猛击", merc_iter)?.Id ?? -1,
						SkillName = "重拳猛击",
					});
				}
				else if (merc_iter.Name == "鞭笞者特里高雷")
				{
					list.Add(new BattleTarget()
					{
						SkillId = StrategyUtils.FindSkillForName("反冲", merc_iter)?.Id ?? -1,
						SkillName = "反冲",
					});
				}
				else if (merc_iter.Name == "尤朵拉")
				{
					List<Target> focusTargets = new List<Target>();
					string[] preKills = new string[]
					{
							//特殊关卡
							"夜魇骑士"
					};
					for (int i = 0; i < preKills.Length; i++)
					{
						focusTargets.Add(StrategyUtils.FindSpecNameTarget(preKills[i], targets_opposite_all));
					}

					Target common_target = targets_opposite_all[0];

					list.Add(new BattleTarget()
					{
						TargetId = common_target?.Id ?? -1,
						TargetName = common_target?.Name ?? "",
						SkillId = StrategyUtils.FindSkillForName("火炮突击", merc_iter)?.Id ?? -1,
						SkillName = "火炮突击",
					});
				}
				//*********************************************默认不动策略****************************************//
				else if (merc_iter.Name.IndexOf("盔中寄居蟹") == 0 ||
					merc_iter.Name.IndexOf("剑鱼") == 0 ||
					merc_iter.Name.IndexOf("闪光翻车鱼") == 0 ||
					merc_iter.Name.IndexOf("黄金猿") == 0 ||
					merc_iter.Name.IndexOf("便携冰墙") == 0 ||
					merc_iter.Name.IndexOf("石槌战旗") == 0 ||
					merc_iter.Name.IndexOf("尤朵拉的火炮") == 0)
				{
					list.Add(new BattleTarget()
					{
						MercName = merc_iter.Name,
						NeedActive = false,
					});
				}
			}

			return list;
		}
	}
}