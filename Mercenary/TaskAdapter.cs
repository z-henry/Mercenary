using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mercenary.MyInterface;
using Mercenary.Strategy;

namespace Mercenary
{
	public static class TaskAdapter
	{
		public static Task SetTask(int mercenaryId, string title, string desc)
		{
			//难完成的
			//闪狐 任务14:唰！ 2装备成就解锁，加速等级不够

			// 对于优先使用多个英雄配合的，优先级设为0，保证一起出场
			// 未满30级的佣兵 优先级设为10，放后备箱里跟着升级
			// 部分要求地图的基本优先级都是5（正常）挨个排着
			// 英雄悬赏优先度为1，为了能早日排进H1-1的地图

			switch (mercenaryId)
			{
				case MercConst.乌瑟尔_光明使者:
					{
						//乌瑟尔 任务12
						if (title.Contains("光明使者"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "复仇之怒", 1),
								TaskAdapter.GetMercenary(MercConst.先知维伦, "维伦的祝福", 0),
								TaskAdapter.GetMercenary(MercConst.考内留斯_罗姆, "牺牲祝福", 2)
							});
							
						}
					}
					break;

				case MercConst.亚煞极:
					{
						//亚煞极 任务3:愤怒即是力量
						//亚煞极 任务10:饱受折磨
						if (title.Contains("愤怒即是力量") || title.Contains("饱受折磨"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "无羁之怒", 0),
								TaskAdapter.GetMercenary(mercenaryId, "亚煞极之力", 0),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.赤精, "振奋之歌", 1)
							});
							
						}
					}
					break;

				case MercConst.伊利丹_怒风:
					{
					}
					break;

				case MercConst.伊瑞尔:
					{
						//伊瑞尔 任务3:惩戒不信之徒
						//伊瑞尔 任务10:净化邪恶
						if (title.Contains("惩戒不信之徒") || title.Contains("净化邪恶"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "光缚之怒", 0),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.赤精, "振奋之歌", 1)
							});
							
						}
					}
					break;

				case MercConst.伊莉斯_逐星:
					{
					}
					break;

				case MercConst.先知维伦:
					{
						//先知维伦 任务12
						if (title.Contains("圣光扫荡一切"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "裂解之光", 1),
								TaskAdapter.GetMercenary(MercConst.安度因_乌瑞恩, "神圣新星", 0)
							});
							
						}
						//先知维伦 任务14
						if (title.Contains("纯净圣光"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "神圣冲击", 0),
								TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", 0,targettype:TARGETTYPE.FRIENDLY)
							});
							
						}
						//先知维伦 任务17
						if (title.Contains("完美圣光"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "裂解之光", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉, null, 0)
							});
							
						}
					}
					break;

				case MercConst.光明之翼:
					{
						//光明之翼 任务17:治疗朋友
						//光明之翼 任务11:我们交朋友吧
						if (title.Contains("治疗朋友") || title.Contains("我们交朋友吧"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "妖精之尘", 1),
								TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", 0,targettype:TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.冰雪之王洛克霍拉:
					{
						//雪王 任务12
						if (title.Contains("雪球滚滚"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "雪球", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
							});
							
						}
					}
					break;

				case MercConst.凯恩_血蹄:
					{
						//任务14 - 踩在蹄下
						if (title.Contains("踩在蹄下"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "大地践踏", 1),
								TaskAdapter.GetMercenary(mercenaryId, "坚韧光环", 1),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
							});
							
						}
					}
					break;

				case MercConst.凯瑞尔_罗姆:
					{
					}
					break;

				case MercConst.剑圣萨穆罗:
					{
					}
					break;

				case MercConst.加尔范上尉:
					{
						//加尔范上尉 任务9:贯穿伤口
						if (title.Contains("贯穿伤口"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "致命打击", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉, "快速治疗", 0),
								TaskAdapter.GetMercenary(MercConst.安娜科德拉, "野兽回复", 1)
							});
							
						}
					}
					break;

				case MercConst.加尔鲁什_地狱咆哮:
					{
					}
					break;

				case MercConst.加拉克苏斯大王:
					{
						//加拉克苏斯大王 任务11:地狱火顺劈
						//加拉克苏斯大王 任务10:邪能烈焰
						if (title.Contains("地狱火顺劈") || title.Contains("邪能烈焰"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "加拉克苏斯之拳", 1),
								TaskAdapter.GetMercenary(MercConst.玛诺洛斯, "恐惧嚎叫", 0),
								TaskAdapter.GetMercenary(MercConst.拉索利安, "巨型大恶魔", 2)
							});
							
						}
					}
					break;

				case MercConst.厨师曲奇:
					{
						//厨师曲奇 任务17:船上的厨子
						if (title.Contains("船上的厨子"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, null, 0),
								TaskAdapter.GetMercenary(mercenaryId, null, 0)
							});
							
						}
					}
					break;

				case MercConst.变装大师:
					{
					}
					break;

				case MercConst.古夫_符文图腾:
					{
						//古夫 任务14
						if (title.Contains("带刺的自然"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "活体荆棘", 0),
								TaskAdapter.GetMercenary(MercConst.布鲁坎, "陷足泥泞", 1)
							});
							
						}
					}
					break;

				case MercConst.古尔丹:
					{
					}
					break;

				case MercConst.吉安娜_普罗德摩尔:
					{
						// 吉安娜·普罗德摩尔 任务12
						if (title.Contains("千钧一发"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(mercenaryId, "浮冰术", 2)
							});
							
						}
					}
					break;

				case MercConst.塔姆辛_罗姆:
					{
						//塔姆辛·罗姆 任务10
						if (title.Contains("证明我的价值"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "暗影之幕", 0),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
							});
							
						}
					}
					break;

				case MercConst.塔维什_雷矛:
					{
						//塔维什·雷矛 任务14:当心脚下
						if (title.Contains("当心脚下"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "爆炸陷阱", 2),
								TaskAdapter.GetMercenary(MercConst.瓦罗克_萨鲁法尔, "旋风斩", 1),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
							});
							
						}
					}
					break;

				case MercConst.奈法利安:
					{
						//奈法利安 任务2:多彩龙，完美的龙
						if (title.Contains("完美的龙"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "多彩能量", 0, -1, TARGETTYPE.FRIENDLY)
							});
							
						}
						//奈法利安 任务12
						if (title.Contains("暗影烈焰"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "暗影烈焰", 1),
								TaskAdapter.GetMercenary(MercConst.希奈丝特拉, "法力壁垒", 1, -1, TARGETTYPE.FRIENDLY),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
							});
							
						}
						//奈法利安 任务17
						if (title.Contains("还算有用"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-2", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"多彩能量", 1),
								TaskAdapter.GetMercenary(MercConst.卡德加, "炉火", 0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯,"暗影之爪",2)
							});
							
						}
					}
					break;

				case MercConst.奔波尔霸:
					{
					}
					break;

				case MercConst.奥妮克希亚:
					{
						//奥妮克希亚 任务2:吸气！
						if (title.Contains("吸气"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, "H2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "深呼吸", 1)
							});
							
						}
						//奥妮克希亚 任务16:团本首领下场
						//奥妮克希亚 任务10:喷吐火焰
						if (title.Contains("喷吐火焰") || title.Contains("团本首领下场"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "清理巢穴", 0),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
							});
							
						}
					}
					break;

				case MercConst.娜塔莉_塞林:
					{
						//任务17
						if (title.Contains("平衡倾斜"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "集束之光", 1, targettype:TARGETTYPE.FRIENDLY),
								TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", 0,targettype:TARGETTYPE.FRIENDLY)
							});
							
						}
						//娜塔莉·赛琳 任务4
						if (title.Contains("圣光治愈一切"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "咒逐", 0),
								TaskAdapter.GetMercenary(mercenaryId, "祈福", 0)
							});
							
						}
					}
					break;

				case MercConst.安东尼达斯:
					{
						// 安东尼达斯 任务12
						if (title.Contains("火焰滚滚"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.赤精, "振奋之歌", 1),
								TaskAdapter.GetMercenary(mercenaryId, "火球风暴", 2)
							});
							
						}

						// 安东尼达斯 任务9
						if (title.Contains("保护肯瑞托"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.拉格纳罗斯, "熔岩冲击", 2),
								TaskAdapter.GetMercenary(mercenaryId, "烈焰风暴", 1)
							});
							
						}
					}
					break;

				case MercConst.安娜科德拉:
					{
					}
					break;

				case MercConst.安度因_乌瑞恩:
					{
					}
					break;

				case MercConst.尤朵拉:
					{
						//尤朵拉 任务9:火炮齐射！
						//尤朵拉 任务16:炮灰
						if (title.Contains("炮灰") || title.Contains("火炮齐射"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, "H1-1", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "准备火炮", 1)
							});
							
						}

						//尤朵拉 任务3:远洋助手
						//尤朵拉 任务14:飓风营救
						if (title.Contains("远洋助手") || title.Contains("飓风营救"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "掩护射击", 1, -1, TARGETTYPE.FRIENDLY),
								TaskAdapter.GetMercenary(MercConst.玉珑,"玉火打击",0),
							});
							
						}
					}
					break;

				case MercConst.尤格_萨隆:
					{
					}
					break;

				case MercConst.巫妖王:
					{
						//巫妖王 任务9:你们都将臣服
						if (title.Contains("你们都将臣服"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "凋零缠绕", 1, -1, TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.巴琳达_斯通赫尔斯:
					{
					}
					break;

				case MercConst.布莱恩_铜须:
					{
					}
					break;

				case MercConst.布鲁坎:
					{
						//布鲁坎 任务17:一点点电
						if (title.Contains("一点点电"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "陷足泥泞", 1),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
							});
							
						}
					}
					break;

				case MercConst.希奈丝特拉:
					{
						//希奈丝特拉 任务2
						if (title.Contains("何必忧伤"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "法力壁垒", 1, -1, TARGETTYPE.FRIENDLY)
							});
							
						}
						//希奈丝特拉 任务12:低语显现
						if (title.Contains("低语显现"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "法力壁垒", 1, targettype:TARGETTYPE.FRIENDLY),
								TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", 0,targettype:TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.希尔瓦娜斯_风行者:
					{
						//风行者 任务 12
						if (title.Contains("无论死活"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "重拾灵魂", 2),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "构筑魔像", 2),
								TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, "眼魔船工", 1)
							});
							
						}
						//希尔瓦娜斯·风行者 任务3:瞄准叛徒
						//希尔瓦娜斯·风行者 任务9:如此狡诈
						if (title.Contains("瞄准叛徒") || title.Contains("如此狡诈"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "为了女王", 1)
							});
							
						}

						//希尔瓦娜斯·风行者 任务17:女妖之怒
						if (title.Contains("女妖之怒"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "重拾灵魂", 0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "构筑魔像", 2),
								TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, "眼魔船工", 1)
							});
							
						}
					}
					break;

				case MercConst.库尔特鲁斯_陨烬:
					{
						//库尔特鲁斯·陨烬 任务10:独自训练
						if (title.Contains("独自训练"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "屠魔者", 2),
								TaskAdapter.GetMercenary(MercConst.卡德加,"炉火",0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "暗影之爪", 2)
							});
							
						}
						//库尔特鲁斯·陨烬 任务14:包扎伤口
						if (title.Contains("包扎伤口"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "眼棱", 0),
								TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", 0,targettype:TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.德雷克塔尔:
					{
						//德雷克塔尔 任务14:邪能的力量
						if (title.Contains("邪能的力量"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "邪能腐蚀", 2),
								TaskAdapter.GetMercenary(mercenaryId, "萨满教义", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
							});
							
						}
						//德雷克塔尔 任务17:淬霜之刃
						if (title.Contains("淬霜之刃"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-2", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, null, 0),
								TaskAdapter.GetMercenary(MercConst.卡德加, "炉火", 0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "暗影之爪", 2)
							});
							
						}
					}
					break;

				case MercConst.恩佐斯:
					{
					}
					break;

				case MercConst.拉希奥:
					{
						//拉希奥 任务14:展开双翼
						//拉希奥 任务3:黑王子之战
						if (title.Contains("展开双翼") || title.Contains("黑王子之战"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "真正形态", 1),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.赤精, "振奋之歌", 1)
							});
							
						}
					}
					break;

				case MercConst.拉格纳罗斯:
					{
						//拉格纳罗斯 任务3
						if (title.Contains("强力陨石"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "陨石术", 0)
							});
							
						}
						//拉格纳罗斯 任务10
						if (title.Contains("全部烧光"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "熔岩冲击", 0),
								TaskAdapter.GetMercenary(MercConst.安东尼达斯, null, 0)
							});
							
						}
					}
					break;

				case MercConst.拉索利安:
					{
						//拉索利安 任务12:势均力敌？哈！
						//拉索利安 任务1:无坚不摧之力
						if (title.Contains("势均力敌") || title.Contains("无坚不摧"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "闪电军团", 0),
								TaskAdapter.GetMercenary(MercConst.玛诺洛斯, null, 0)
							});
							
						}

						//拉索利安 任务14:战场主宰
						if (title.Contains("战场主宰"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"巨型大恶魔", 0),
							});
							
						}
						//拉索利安 任务17
						if (title.Contains("巨型邪魔"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "巨型大恶魔", 0),
								TaskAdapter.GetMercenary(MercConst.洛卡拉, "进攻集结", 1)
							});
							
						}
					}
					break;

				case MercConst.指挥官沃恩:
					{
						//指挥官沃恩 任务9:燃棘必胜
						if (title.Contains("燃棘必胜"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(mercenaryId, "火焰吐息", 0)
							});
							
						}
						//指挥官沃恩 任务12:他们想挨斧头了
						//指挥官沃恩 任务17:猛斧与盟友
						if (title.Contains("他们想挨斧头了") || title.Contains("猛斧与盟友"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "飞斧投掷", 0),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
							});
							
						}
					}
					break;

				case MercConst.提里奥_弗丁:
					{
						//弗丁 任务2 任务14
						if (title.Contains("王者祝福") || title.Contains("银色北伐军"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "王者祝福", 0, -1, TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.斯卡布斯_刀油:
					{
						//斯卡布斯·刀油 任务1 任务9
						if (title.Contains("秘密探员") || title.Contains("二加二等于五"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "战术打击", 0),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
							});
							
						}
					}
					break;

				case MercConst.斯尼德:
					{
						//斯尼德 任务9
						if (title.Contains("手脚并用"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-1", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "缴械", 0),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.提里奥_弗丁, "谦逊制裁", 0)
							});
							
						}
						//斯尼德 任务12 任务14
						if (title.Contains("一刀两断") || title.Contains("乌合之众"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "滋啦", 2)
							});
							
						}
					}
					break;

				case MercConst.暴龙王克鲁什:
					{
						//暴龙王克鲁什 任务12:暴龙之王
						if (title.Contains("暴龙之王"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "顶级捕食者", 2),
								TaskAdapter.GetMercenary(MercConst.洛卡拉, "进攻集结", 1),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "暗影之爪", 2)
							});
							
						}
					}
					break;

				case MercConst.格罗玛什_地狱咆哮:
					{
						//格罗玛什·地狱咆哮 任务9:暂缓苦痛
						if (title.Contains("暂缓苦痛"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "战斗怒火", 2),
								TaskAdapter.GetMercenary(MercConst.卡德加,"炉火",0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "暗影之爪", 2)
							});
							
						}

						//格罗玛什·地狱咆哮 任务14:玛诺洛斯的许诺
						if (title.Contains("玛诺洛斯的许诺"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-1", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "战斗怒火", 2),
								TaskAdapter.GetMercenary(MercConst.厨师曲奇, "小鱼快冲", 0)
							});
							
						}
					}
					break;

				case MercConst.格鲁尔:
					{
					}
					break;

				case MercConst.梵妮莎_范克里夫:
					{
						//梵妮莎·范克里夫 任务14:一刃双雕
						if (title.Contains("一刃双雕"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "临时武器", 2),
								TaskAdapter.GetMercenary(MercConst.乌瑟尔_光明使者, "保护祝福", 0),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
							});
							
						}
						//梵妮莎·范克里夫 任务9:夺刀斩将
						if (title.Contains("夺刀斩将"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-1", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "剽窃轰击", 0),
								TaskAdapter.GetMercenary(MercConst.提里奥_弗丁, "谦逊制裁", 0),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
							});
							
						}
					}
					break;

				case MercConst.死亡之翼:
					{
					}
					break;

				case MercConst.沃金:
					{
						//沃金 任务9:暗影秘法
						//沃金 任务14:沃金的意志
						if (title.Contains("暗影秘法") || title.Contains("沃金的意志"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "构筑魔像", 0),
								TaskAdapter.GetMercenary(mercenaryId, "暗影涌动", 1)
							});
							
						}
					}
					break;

				case MercConst.泰兰德_语风:
					{
					}
					break;

				case MercConst.泰瑞尔:
					{
						//泰瑞尔 任务2
						if (title.Contains("巨大的谎言"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "1-10", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.巴琳达_斯通赫尔斯, "烈焰之刺", 1),
								TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火", 1),
								TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子", 2),
								TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 1),
								TaskAdapter.GetMercenary(MercConst.吉安娜_普罗德摩尔,null,2),
								TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击", 0)
							});
							
						}
						//泰瑞尔 任务10
						if (title.Contains("黑暗流亡者"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-9", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.巴琳达_斯通赫尔斯, "烈焰之刺", 1),
								TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火", 1),
								TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子", 2),
								TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 0),
								TaskAdapter.GetMercenary(MercConst.吉安娜_普罗德摩尔,null,2),
								TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击", 0)
							});
							
						}
						//泰瑞尔 任务12
						if (title.Contains("恶魔再临"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.巴琳达_斯通赫尔斯, "烈焰之刺", 1),
								TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火", 1),
								TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子", 2),
								TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 0),
								TaskAdapter.GetMercenary(MercConst.吉安娜_普罗德摩尔,null,2),
								TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击", 0)
							});
							
						}
						//泰瑞尔 任务14
						if (title.Contains("夺魂之镰"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H5-2", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.巴琳达_斯通赫尔斯, "烈焰之刺", 1),
								TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火", 1),
								TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子", 2),
								TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 0),
								TaskAdapter.GetMercenary(MercConst.吉安娜_普罗德摩尔,null,2),
								TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击", 0)
							});
							
						}
					}
					break;

				case MercConst.泽瑞拉:
					{
					}
					break;

				case MercConst.洛卡拉:
					{
						//洛卡拉 任务9
						if (title.Contains("新的任务"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "部族战争", 0),
								TaskAdapter.GetMercenary(MercConst.剑圣萨穆罗, null, 0)
							});
							
						}
					}
					break;

				case MercConst.海巫扎尔吉拉:
					{
					}
					break;

				case MercConst.海盗帕奇斯:
					{
					}
					break;

				case MercConst.深水领主卡拉瑟雷斯:
					{
					}
					break;

				case MercConst.游学者周卓:
					{
					}
					break;

				case MercConst.滑矛领主:
					{
						//滑矛领主 任务3:丢进垃圾箱
						//滑矛领主 任务10:那就是垃圾
						if (title.Contains("丢进垃圾箱") || title.Contains("那就是垃圾"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-1", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "处理垃圾", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.厨师曲奇, "小鱼快冲", 0)
							});
							
						}
					}
					break;

				case MercConst.潮汐主母阿茜萨:
					{
						//潮汐主母阿茜萨 任务3:以艾萨拉之名
						//潮汐主母阿茜萨 任务10:水流之力
						if (title.Contains("以艾萨拉之名") || title.Contains("水流之力"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "俘获之潮", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.赤精, "振奋之歌", 1)
							});
							
						}
					}
					break;

				case MercConst.火车王里诺艾:
					{
					}
					break;

				case MercConst.玉珑:
					{
						//玉珑 任务12:成长与新生
						if (title.Contains("成长与新生"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-1", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "青玉劲风", 0),
								TaskAdapter.GetMercenary(MercConst.剑圣萨穆罗, "二连击", 0)
							});
							
						}
					}
					break;

				case MercConst.玛法里奥_怒风:
					{
						//玛法里奥·怒风 任务3:自然力量
						if (title.Contains("自然力量"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "大德鲁伊的召唤", 1, 0),
							});
							
						}
					}
					break;

				case MercConst.玛维_影歌:
					{
					}
					break;

				case MercConst.玛诺洛斯:
					{
						//玛诺洛斯 任务12:愚蠢的弱者
						if (title.Contains("愚蠢的弱者"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "邪能抽笞", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉, "快速治疗", 0)
							});
							
						}
						//玛诺洛斯 任务14:让他们学会畏惧
						if (title.Contains("让他们学会畏惧"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "恐惧嚎叫", 2)
							});
							
						}
					}
					break;

				case MercConst.珑心:
					{
						// 珑心	任务3 任务10
						if (title.Contains("远古洞窟") || title.Contains("修习飞举"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "群星簇拥", 1, -1, TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.瓦丝琪女士:
					{
					}
					break;

				case MercConst.瓦尔登_晨拥:
					{
						//瓦尔登·晨拥 任务9
						if (title.Contains("学术道德"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "冰枪术", 0),
								TaskAdapter.GetMercenary(MercConst.布莱恩_铜须, "铜须风范", 0),
							});
							
						}
						//瓦尔登·晨拥 任务10
						if (title.Contains("调节温度"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "冰枪术", 1),
								TaskAdapter.GetMercenary(mercenaryId, "急速冰冻", 1),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(MercConst.赤精, "振奋之歌", 1),
							});
							
						}
						//瓦尔登·晨拥 任务12
						if (title.Contains("附加影响"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-2", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "冰风暴", 0),
								TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 1)
							});
							
						}
					}
					break;

				case MercConst.瓦罗克_萨鲁法尔:
					{
						//瓦罗克·萨鲁法尔 任务12:全力一击
						if (title.Contains("全力一击"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "动员打击", 0),
								TaskAdapter.GetMercenary(MercConst.洛卡拉, "进攻集结", 1)
							});
							
						}
					}
					break;

				case MercConst.瓦莉拉_萨古纳尔:
					{
					}
					break;

				case MercConst.瓦里安_乌瑞恩:
					{
						//瓦里安·乌瑞恩 任务3 任务17
						if (title.Contains("为了暴风城") || title.Contains("屹立不倒"))
						{
							return TaskAdapter.GetTask(mercenaryId, 4, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "反击", 2)
							});
							
						}
					}
					break;

				case MercConst.砮皂:
					{
					}
					break;

				case MercConst.神谕者摩戈尔:
					{
						// 神谕者摩戈尔 任务12
						if (title.Contains("鲜血淋漓"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "鱼人弹幕", 1),
								TaskAdapter.GetMercenary(40, "邪鳍导航员", 1),
								TaskAdapter.GetMercenary(149, "小鱼快冲", 2)
							});
							
						}
						//神谕者摩戈尔 任务14:耐心取胜
						if (title.Contains("耐心取胜"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "治疗波", 2, targettype:TARGETTYPE.FRIENDLY),
								TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", 0,targettype:TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.穆克拉:
					{
						//穆克拉 任务17:家族纽带
						if (title.Contains("家族纽带"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, "H1-1", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "原始之力", 2)
							});
							
						}
					}
					break;

				case MercConst.穆坦努斯:
					{
						//穆坦努斯 任务3:鸡肉味
						//穆坦努斯 任务10:饥不可耐
						//穆坦努斯 任务17:每日一鱼人
						if (title.Contains("鸡肉味") || title.Contains("饥不可耐") || title.Contains("每日一鱼人"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "吞噬", 0),
								TaskAdapter.GetMercenary(mercenaryId, "鳞甲嘲讽", 2),
								TaskAdapter.GetMercenary(MercConst.老瞎眼, "老蓟皮", 2)
							});
							
						}
					}
					break;

				case MercConst.空军上将罗杰斯:
					{
						// 空军上将罗杰斯 任务2
						if (title.Contains("列队飞行"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "战术大师", 0, -1, TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.米尔豪斯_法力风暴:
					{
						//米尔豪斯_法力风暴 任务17
						if (title.Contains("大箭就是好箭"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "奥术箭", 0),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
							});
							
						}
						//任务12
						if (title.Contains("嗞，啪，嘭！"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"魔爆术",0),
								TaskAdapter.GetMercenary(MercConst.厨师曲奇,"小鱼快冲",2),
							});
							
						}
					}
					break;

				case MercConst.老瞎眼:
					{
						//老瞎眼 任务9
						if (title.Contains("鱼类学"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"老蓟皮",2)
							});
							
						}
						//老瞎眼 任务12
						if (title.Contains("鱼多势众"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"鱼人入侵",0),
								TaskAdapter.GetMercenary(MercConst.厨师曲奇,null,0),
								TaskAdapter.GetMercenary(MercConst.神谕者摩戈尔,null,0)
							});
							
						}
					}
					break;

				case MercConst.考内留斯_罗姆:
					{
						//考内留斯·罗姆 任务2:我来承受伤害
						//考内留斯·罗姆 任务10:进攻与收获
						if (title.Contains("我来承受伤害") || title.Contains("进攻与收获") || title.Contains("向我集结"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-2", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.奈姆希_灵沼,"沼泽工程学",0),
								TaskAdapter.GetMercenary(mercenaryId, "坚守前线", 1),
								TaskAdapter.GetMercenary(MercConst.穆克拉,"大餐时间",0)
							});
							
						}
					}
					break;

				case MercConst.艾德温_迪菲亚首脑:
					{
						//艾德温·迪菲亚首脑 任务11 任务17
						if (title.Contains("致命一击") || title.Contains("身先士卒"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "首脑的悬赏", 2),
								TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, null, 0),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
							});
							
						}
					}
					break;

				case MercConst.艾萨拉女王:
					{
					}
					break;

				case MercConst.范达尔_雷矛:
					{
						//范达尔·雷矛 任务10:我们必胜
						if (title.Contains("我们必胜"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-2", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"天神下凡", 1),
								TaskAdapter.GetMercenary(mercenaryId,"向前推进", 1),
								TaskAdapter.GetMercenary(MercConst.卡德加,"炉火",0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯,"暗影之爪", 2),
							});
							
						}
						//范达尔·雷矛 任务17
						if (title.Contains("拼尽全力"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"向前推进", 1),
								TaskAdapter.GetMercenary(MercConst.卡德加,"炉火",0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯,"暗影之爪",2)
							});
							
						}
					}
					break;

				case MercConst.萨尔:
					{
					}
					break;

				case MercConst.贝恩_血蹄:
					{
					}
					break;

				case MercConst.赤精:
					{
					}
					break;

				case MercConst.迦顿男爵:
					{
						//迦顿男爵 任务12:烧烤弱鸡
						if (title.Contains("烧烤弱鸡"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "活体炸弹", 1),
								TaskAdapter.GetMercenary(MercConst.厨师曲奇, "小鱼快冲", 0),
								TaskAdapter.GetMercenary(MercConst.安东尼达斯, "烈焰风暴", 1)
							});
							
						}
						//迦顿男爵 任务10:侵略如火
						//迦顿男爵 任务14:翻腾火流
						if (title.Contains("侵略如火") || title.Contains("翻腾火流"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "热力迸发", 0),
								TaskAdapter.GetMercenary(MercConst.安东尼达斯, "火球术", 0),
								TaskAdapter.GetMercenary(MercConst.拉格纳罗斯, "熔岩冲击", 0)
							});
							
						}
					}
					break;

				case MercConst.迪亚波罗:
					{
						//迪亚波罗 任务12:死亡降临
						if (title.Contains("死亡降临"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "火焰践踏", 0),
								TaskAdapter.GetMercenary(MercConst.厨师曲奇, "小鱼快冲", 2)
							});
							
						}
					}
					break;

				case MercConst.重拳先生:
					{
						//重拳先生 任务14:铁脚无情
						if (title.Contains("铁脚无情"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-6", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.洛卡拉, "进攻集结", 1, 0, 0),
								TaskAdapter.GetMercenary(mercenaryId, "落水追击", 0),
								TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, "眼魔船长", 1)
							});
							
						}
					}
					break;

				case MercConst.钩牙船长:
					{
					}
					break;

				case MercConst.闪狐:
					{
						//闪狐 任务3:现在归我了
						if (title.Contains("现在归我了"))
						{
							return TaskAdapter.GetTask(mercenaryId, 5, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "防护之戒", 2),
								TaskAdapter.GetMercenary(mercenaryId, "古树的坚韧", 2),
								TaskAdapter.GetMercenary(mercenaryId, "古树生长", 2),
								TaskAdapter.GetMercenary(mercenaryId, "古树知识", 2),
								TaskAdapter.GetMercenary(mercenaryId, "攻击", 2),
								TaskAdapter.GetMercenary(mercenaryId, "群体缠绕", 2),
								TaskAdapter.GetMercenary(mercenaryId, "树皮肌肤", 2),
								TaskAdapter.GetMercenary(mercenaryId, "大地的敌意", 2)
							});
							
						}

						//闪狐 任务14:唰！
						if (title.Contains("唰！"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
								TaskAdapter.GetMercenary(mercenaryId, "法力闪现", 1),
								TaskAdapter.GetMercenary(MercConst.贝恩_血蹄, "大地母亲之怒", 2),
								TaskAdapter.GetMercenary(MercConst.贝恩_血蹄, "治疗链", 2)
							});
							
						}
					}
					break;

				case MercConst.阿莱克丝塔萨:
					{
						//阿莱克丝塔萨 任务9
						if (title.Contains("生命守护者"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "巨龙吐息", 0, -1, TARGETTYPE.FRIENDLY)
							});
							
						}
						//阿莱克丝塔萨 任务17
						if (title.Contains("他们的力量属于我"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "红龙女王的计策", 1),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "指引道路", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "开启任务", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "躲避毒镖", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "丛林导航", 2),
								TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "摆脱滚石", 2),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
							});
							
						}
						//阿莱克丝塔萨 任务12 任务14
						if (title.Contains("一举两得") || title.Contains("蹈踏火焰"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "烈焰猛击", 0),
								TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
							});
							
						}
						//阿莱克丝塔萨 任务9:生命守护者
						if (title.Contains("生命守护者"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "巨龙吐息", 0, targettype:TARGETTYPE.FRIENDLY),
								TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", 1),
								TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", 0,targettype:TARGETTYPE.FRIENDLY)
							});
							
						}
					}
					break;

				case MercConst.雪怒:
					{
						//雪怒 任务12:往日旧梦
						if (title.Contains("往日旧梦"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "伏虎闪雷", 2),
								TaskAdapter.GetMercenary(mercenaryId, "白虎飞扑", 2),
								TaskAdapter.GetMercenary(MercConst.卡德加, "炉火", 0),
								TaskAdapter.GetMercenary(MercConst.穆克拉, "原始之力", 0)
							});
							
						}
						//雪怒 任务16
						if (title.Contains("战斗热血"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "H1-2", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,"白虎飞扑", 1),
								TaskAdapter.GetMercenary(MercConst.卡德加, "炉火", 0),
								TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯,"暗影之爪",2)
							});
							
						}
					}
					break;

				case MercConst.雷克萨:
					{
						//雷克萨 任务17:唯一的朋友
						if (title.Contains("唯一的朋友"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "动物伙伴", 2),
								TaskAdapter.GetMercenary(MercConst.穆克拉, null, 2)
							});
							
						}
					}
					break;

				case MercConst.雷诺_杰克逊:
					{
					}
					break;

				case MercConst.鞭笞者特里高雷:
					{
						//任务2:无情抽笞
						if (title.Contains("无情抽笞"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(MercConst.考内留斯_罗姆, "坚守前线", 0),
								TaskAdapter.GetMercenary(mercenaryId, "反冲", 1),
								TaskAdapter.GetMercenary(MercConst.凯瑞尔_罗姆, "嘲讽", 0)
							});
							
						}
						// 任务11
						if (title.Contains("侧面横扫"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, "2-5", new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId,null,0),
								TaskAdapter.GetMercenary(MercConst.瓦罗克_萨鲁法尔, "旋风斩", 1)
							});
							
						}
					}
					break;

				case MercConst.魔像师卡扎库斯:
					{
					}
					break;

				case MercConst.卡德加:
					{
						//任务3 - 燃烧之魂
						if (title.Contains("燃烧之魂"))
						{
							return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, null , 1),
								TaskAdapter.GetMercenary(MercConst.安东尼达斯, "烈焰风暴", 1)
							});
							
						}

						//任务9 - 搅热事态
						//任务14 - 臻于完美
						if (title.Contains("搅热事态") ||
							title.Contains("臻于完美"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "炉火", 2, targettype:TARGETTYPE.FRIENDLY),
							});
							
						}
						//任务10 - 然后降温
						if (title.Contains("然后降温"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "暴风雪", 1),
							});
							
						}
					}
					break;
				case MercConst.血骨傀儡:
					{
						//任务10
						if (title.Contains("血骨教育"))
						{
							return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
							{
								TaskAdapter.GetMercenary(mercenaryId, "骨焰", 0),
							});
							
						}
					}
					break;
			}

			//杀恶魔
			if (desc.Contains("击败") && desc.Contains("个恶魔"))
			{
				return TaskAdapter.GetTask(mercenaryId, 5, "2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			//杀龙
			if ((desc.Contains("击败") || desc.Contains("消灭")) && desc.Contains("条龙"))
			{
				return TaskAdapter.GetTask(mercenaryId, 5, "4-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			//杀野兽
			if (desc.Contains("击败") && desc.Contains("只野兽"))
			{
				return TaskAdapter.GetTask(mercenaryId, 5, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			//杀元素
			if (desc.Contains("击败") && desc.Contains("元素"))
			{
				return TaskAdapter.GetTask(mercenaryId, 5, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			//杀鱼人
			if (desc.Contains("消灭") && desc.Contains("个鱼人"))
			{
				return TaskAdapter.GetTask(mercenaryId, 5, "7-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}

			//加快技能速度
			if (desc.Contains("加快友方技能的速度值总") || desc.Contains("使一个友方技能的速度值加快"))
			{
				return TaskAdapter.GetTask(mercenaryId, 5, "2-5", TaskAdapter.GetQuickMercenary(mercenaryId));
				
			}

			//巨魔造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用巨魔造成"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.沃金, "暗影涌动", 1)
				});
				
			}
			//海盗造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用海盗造成"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, "眼魔船长", 0)
				});
				
			}
			//恶魔造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用恶魔造成"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.拉索利安, "巨型大恶魔", 0)
				});
				
			}
			//野兽造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用野兽造成"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.闪狐, null, 0)
				});
				
			}
			//元素造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用元素造成"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 0)
				});
				
			}
			//龙造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用龙造成"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.奈法利安,"龙人突袭",1)
				});
				
			}
			//人类造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用人类造成"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯, "烈焰风暴", 1)
				});
				
			}
			//造成神圣伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("神圣伤害"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉, null, 0)
				});
				
			}
			//造成冰霜伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("冰霜伤害"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉,"冰雹", 1)
				});
				
			}
			//造成火焰伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("火焰伤害"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯, "火球术", 0),
				});
				
			}
			//造成暗影伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("暗影伤害"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.沃金, "虚弱诅咒", 0),
				});
				
			}
			//造成邪能伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("邪能伤害"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.玛诺洛斯, null, 0),
				});
				
			}
			//自然伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("自然伤害"))
			{
				return TaskAdapter.GetTask(mercenaryId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.布鲁坎, "闪电箭", 0),
				});
				
			}
			//英雄难度相关
			if (desc.Contains("英雄难度"))
			{
				return TaskAdapter.GetTask(mercenaryId, 1, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			if (desc.Contains("完成") && desc.Contains("个悬赏"))
			{
				LettuceMercenary mercenary = HsGameUtils.GetMercenary(mercenaryId);
				if (desc.Contains("30级时") && !mercenary.IsMaxLevel())
				{
					return TaskAdapter.GetTask(mercenaryId, 10, new MercenaryEntity[]
					{
						TaskAdapter.GetMercenary(mercenaryId, null, 0)
					});
				}
				else
				{
					return TaskAdapter.GetTask(mercenaryId, 10, "H1-1", new MercenaryEntity[]
					{
						TaskAdapter.GetMercenary(mercenaryId, null, 0)
					});
				}
				
			}

			//抵达神秘选项，赐福或灵魂医者处
			if (desc.Contains("赐福或灵魂医者"))
			{
				TaskUtils.HaveTaskDocter = true;
				TaskUtils.HaveTaskCaster = true;
				TaskUtils.HaveTaskFighter = true;
				TaskUtils.HaveTaskTank = true;

				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}

			//灵魂医者
			if (desc.Contains("利用灵魂医者"))
			{
				TaskUtils.HaveTaskDocter = true;

				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			//赐福
			if (desc.Contains("利用赐福"))
			{
				if (desc.Contains("施法者"))
					TaskUtils.HaveTaskCaster = true;
				else if (desc.Contains("斗士"))
					TaskUtils.HaveTaskFighter = true;
				else if (desc.Contains("护卫"))
					TaskUtils.HaveTaskTank = true;

				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			int num = desc.IndexOf("$ability(", StringComparison.Ordinal);
			if (num == -1)
			{
				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			int num2 = num + "$ability(".Length;
			int num3 = desc.IndexOf(")", num2, StringComparison.Ordinal);
			if (num3 == -1)
			{
				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			string[] array = desc.Substring(num2, num3 - num2).Split(new char[]
			{
				','
			});
			int num4;
			int num5;
			if (!int.TryParse(array[0], out num4) || !int.TryParse(array[1], out num5))
			{
				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			LettuceMercenaryDbfRecord record = GameDbf.LettuceMercenary.GetRecord(mercenaryId);
			if (num4 >= record.LettuceMercenarySpecializations.Count)
			{
				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			LettuceMercenarySpecializationDbfRecord lettuceMercenarySpecializationDbfRecord = record.LettuceMercenarySpecializations[num4];
			if (num5 >= lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities.Count)
			{
				return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				});
				
			}
			int lettuceAbilityId = lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities[num5].LettuceAbilityId;
			LettuceAbilityDbfRecord record2 = GameDbf.LettuceAbility.GetRecord(lettuceAbilityId);
			return TaskAdapter.GetTask(mercenaryId, new MercenaryEntity[]
			{
				TaskAdapter.GetMercenary(mercenaryId, record2.AbilityName, GetEquipEnHanceSkill(record2.AbilityName))
			});
		}

		private static MercenaryEntity[] GetQuickMercenary(int mercenaryId)
		{
			List<MercenaryEntity> list = new List<MercenaryEntity>();
			foreach (LettuceAbility lettuceAbility in CollectionManager.Get().GetMercenary((long)mercenaryId, false, true).m_abilityList)
			{
				CardDbfRecord cardRecord = GameDbf.GetIndex().GetCardRecord(lettuceAbility.GetCardId());
				string @string = cardRecord.TextInHand.GetString(Locale.zhCN);
				if (@string != null && @string.Contains("速度值") && @string.Contains("加快"))
				{
					string string2 = cardRecord.Name.GetString(Locale.zhCN);
					list.Add(TaskAdapter.GetMercenary(mercenaryId, string2.Substring(0, string2.Length - 1), 0));
				}
			}
			return list.ToArray();
		}

		private static Task GetTask(int mercid, int priority, string map, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Priority = priority,
				Map = map,
				Mercenaries = mercenaries?.ToList(),
				mercID = mercid
			};
		}

		private static Task GetTask(int mercid, int priority, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Mercenaries = mercenaries?.ToList(),
				Priority = priority,
				mercID = mercid
			};
		}

		private static Task GetTask(int mercid, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Mercenaries = mercenaries?.ToList(),
				Priority = 5,
				mercID = mercid
			};
		}

		private static MercenaryEntity GetMercenary(int id, string skill = null, int eq = 0, int subskill = -1, TARGETTYPE targettype = TARGETTYPE.UNSPECIFIED)
		{
			return new MercenaryEntity(id, skill, eq, subskill, targettype);
		}

		private static int GetEquipEnHanceSkill(string skill)
		{
			return m_dictSkillEquip.ContainsKey(skill) ? m_dictSkillEquip[skill] : 0;
		}

		public static Task SetMainLineTask(string desc)
		{

			Regex regex = new Regex(@"完成(\D.+)悬赏。$");
			Match match = regex.Match(desc);
			if (match.Groups.Count == 2)
			{
				string mapName = MapUtils.GetMapByBoss(match.Groups[1].Value)?.Name ?? "";
				return TaskAdapter.GetTask(-1, 0, mapName, null);

			}

			switch (desc)
			{
				//贫瘠之地
				case "击败8只野兽。":
					return TaskAdapter.GetTask(-1, 0, "1-6", null);

				//费伍德森林
				case "在费伍德森林击败20个敌人。":
					return TaskAdapter.GetTask(-1, 0, "2-1", null);


				case "击败15个恶魔。":
					return TaskAdapter.GetTask(-1, 0, "2-6", null);

				//冬泉谷
				case "击败12个熊怪。":
				case "在冬泉谷完成3个悬赏。":
					return TaskAdapter.GetTask(-1, 0, "3-1", null);

				//黑石山
				case "在黑石山造成2000点伤害。":
				case "在黑石山击败50个敌人。":
				case "在黑石山击败60个敌人。":
				case "在黑石山造成2500点伤害。":
					return TaskAdapter.GetTask(-1, 0, "4-1", null);


				case "在黑石山击败25个元素。":
					return TaskAdapter.GetTask(-1, 0, "4-4", null);

				//奥特兰克山谷
				case "在奥特兰克山谷击败25个敌人。":
				case "在奥特兰克造成2000点伤害。":
					return TaskAdapter.GetTask(-1, 0, "5-1", null);

			}
			return null;
		}
		public static Task SetEventTask(string title, string mercname)
		{
			string taskStrategyName = $"活动_{mercname}_{title}";
			IStrategy strategy = StrategyHelper.GetStrategy(taskStrategyName);
			if (strategy == null)
				return null;

			return TaskAdapter.GetTask(-1, 0, ((IEventInfo)strategy).Map, null);

		}


		private static Dictionary<string, int> m_dictSkillEquip = new Dictionary<string, int>
		{
			{
				"神圣冲击",
				0
			},
			{
				"裂解之光",
				1
			},
			{
				"致盲之光",
				0
			},
			{
				"救赎",
				2
			},
			{
				"魔爆术",
				0
			},
			{
				"强能奥术飞弹",
				2
			},
			{
				"集束暗影",
				0
			},
			{
				"集束之光",
				1
			},
			{
				"咒逐",
				2
			},
			{
				"精灵吐息",
				0
			},
			{
				"妖精之尘",
				1
			},
			{
				"相位变换",
				2
			},
			{
				"热力迸发",
				0
			},
			{
				"地狱火",
				1
			},
			{
				"闪电箭",
				0
			},
			{
				"符文猛击",
				0
			},
			{
				"活体荆棘",
				1
			},
			{
				"铁木树皮",
				2
			},
			{
				"冰风暴",
				0
			},
			{
				"急速冰冻",
				1
			},
			{
				"暗影箭",
				0
			},
			{
				"虚空吞噬者",
				1
			},
			{
				"毁灭之雨",
				0
			},
			{
				"生命虹吸",
				1
			},
			{
				"暗影震击",
				0
			},
			{
				"暗影涌动",
				1
			},
			{
				"制裁之锤",
				0
			},
			{
				"复仇之怒",
				1
			},
			{
				"冰刺",
				0
			},
			{
				"火球术",
				0
			},
			{
				"烈焰风暴",
				1
			},
			{
				"奥术飞掷",
				0
			},
			{
				"法力闪现",
				1
			},
			{
				"曲奇下厨",
				1
			},
			{
				"暗影之爪",
				0
			},
			{
				"天空守卫",
				0
			},
			{
				"烈焰之歌",
				0
			},
			{
				"振奋之歌",
				1
			},
			{
				"傲慢的凡人",
				0
			},
			{
				"深呼吸",
				1
			},
			{
				"杀戮命令",
				0
			},
			{
				"动物伙伴",
				2
			},
			{
				"爆炸射击",
				1
			},
			{
				"战术打击",
				0
			},
			{
				"邪恶挥刺",
				1
			},
			{
				"暗影之刃",
				2
			},
			{
				"部族战争",
				0
			},
			{
				"进攻集结",
				1
			},
			{
				"侧翼突击",
				0
			},
			{
				"流放攻击",
				1
			},
			{
				"女妖之箭",
				1
			},
			{
				"重拾灵魂",
				0
			},
			{
				"鱼人入侵",
				0
			},
			{
				"邪鳍导航员",
				1
			},
			{
				"老蓟皮",
				2
			},
			{
				"顶级捕食者",
				2
			},
			{
				"惊骇",
				0
			},
			{
				"魔暴龙",
				1
			},
			{
				"瞄准射击",
				0
			},
			{
				"爆炸陷阱",
				2
			},
			{
				"捕熊陷阱",
				2
			},
			{
				"动员打击",
				0
			},
			{
				"旋风斩",
				1
			},
			{
				"二连击",
				2
			},
			{
				"镜像",
				0
			},
			{
				"旋风之刃",
				1
			},
			{
				"鳞甲嘲讽",
				0
			},
			{
				"吞噬攻击",
				2
			},
			{
				"火焰吐息",
				0
			},
			{
				"辟法巨龙",
				1
			},
			{
				"毒蛇噬咬",
				0
			},
			{
				"野兽恢复",
				1
			},
			{
				"奥术射击",
				0
			},
			{
				"奥术齐射",
				1
			},
			{
				"艾露恩的赐福",
				2
			},
			{
				"末日冲锋",
				1
			},
			{
				"末日",
				0
			},
			{
				"首脑的悬赏",
				2
			},
			{
				"辅助打击",
				1
			},
			{
				"火炮突击",
				1
			},
			{
				"准备火炮",
				1
			},
			{
				"掩护射击",
				1
			},
			{
				"致命打击",
				1
			},
			{
				"破胆怒吼",
				2
			},
			{
				"蠕行疯狂",
				0
			},
			{
				"扫尾",
				1
			},
			{
				"均衡一击",
				0
			},
			{
				"白虎飞扑",
				1
			},
			{
				"伏虎闪雷",
				2
			},
			{
				"军团爆能",
				0
			},
			{
				"加拉克苏斯之拳",
				1
			},
			{
				"邪能地狱火",
				2
			},
			{
				"眼棱",
				0
			},
			{
				"屠魔者",
				2
			},
			{
				"盲目突击",
				2
			},
			{
				"热血",
				0
			},
			{
				"惊愕猛击",
				1
			},
			{
				"战斗怒火",
				2
			},
			{
				"爆裂打击",
				0
			},
			{
				"英勇飞跃",
				1
			},
			{
				"反击",
				2
			},
			{
				"远征军打击",
				0
			},
			{
				"嘲讽",
				2
			},
			{
				"光明圣印",
				1
			},
			{
				"熔岩冲击",
				0
			},
			{
				"死吧，虫子",
				2
			},
			{
				"陨石术",
				2
			},
			{
				"先祖勾拳",
				2
			},
			{
				"坚韧光环",
				0
			},
			{
				"大地践踏",
				1
			},
			{
				"暴食香蕉",
				1
			},
			{
				"大餐时间",
				1
			},
			{
				"原始之力",
				2
			},
			{
				"为了部落",
				2
			},
			{
				"大酋长的命令",
				0
			},
			{
				"荣耀决斗",
				0
			},
			{
				"部落之力",
				1
			},
			{
				"战斗怒吼",
				2
			},
			{
				"奔跑破坏神",
				0
			},
			{
				"邪能抽笞",
				1
			},
			{
				"恐惧嚎叫",
				2
			},
			{
				"塞纳里奥波动",
				2
			},
			{
				"纠缠根须",
				0
			},
			{
				"大德鲁伊的召唤",
				1
			},
			{
				"狂暴攻击",
				0
			},
			{
				"反冲",
				1
			},
			{
				"再生头颅",
				2
			},
			{
				"武艺精通",
				0
			},
			{
				"坚守前线",
				1
			},
			{
				"寒冰之噬",
				0
			},
			{
				"凋零缠绕",
				1
			},
			{
				"嗞啦",
				2
			},
			{
				"启动电锯",
				1
			},
			{
				"缴械",
				2
			},
			{
				"重拳猛击",
				0
			},
			{
				"落水追击",
				1
			},
			{
				"剽窃轰击",
				0
			},
			{
				"临时武器",
				2
			},
			{
				"搬运背包",
				2
			},
			{
				"冰雹",
				1
			},
			{
				"冰霜震击",
				1
			},
			{
				"雪球",
				1
			},
			{
				"雷霆打击",
				0
			},
			{
				"向前推进",
				1
			},
			{
				"天神下凡",
				1
			},
			{
				"恐怖利爪",
				0
			},
			{
				"源质护甲",
				1
			}
		};

		private static int m_globalWater = 0;
	}
}