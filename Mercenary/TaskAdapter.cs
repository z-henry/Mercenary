using System;
using System.Collections.Generic;

namespace Mercenary
{
	// Token: 0x0200000D RID: 13
	public static class TaskAdapter
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00005234 File Offset: 0x00003434
		public static void SetTask(int taskId, int mercenaryId, string title, string desc, List<Task> tasks)
		{
			// 对于优先使用多个英雄配合的，优先级设为0，保证一起出场
			// 未满30级的佣兵 优先级设为10，放后备箱里跟着升级
			// 部分要求地图的基本优先级都是5（正常）挨个排着
			// 英雄悬赏优先度为1，为了能早日排进H1-1的地图
			Out.Log(string.Format("[TID:{0}][MID:{1}] {2} {3}",
				taskId, mercenaryId, title, desc));

			//古夫 任务14
			if (title.Contains("带刺的自然"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "圣剑挺击", 0),
					TaskAdapter.GetMercenary(21, "陷足泥泞", 1)
				}));
				return;
			}

			//塔姆辛·罗姆 任务10
			if (title.Contains("证明我的价值"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
					TaskAdapter.GetMercenary(mercenaryId, "暗影之幕", 0)
				}));
				return;
			}

			//德雷克塔尔 任务9 沃金 任务14
			if (title.Contains("暗影秘法") || title.Contains("沃金的意志"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "构筑魔像", 0),
					TaskAdapter.GetMercenary(mercenaryId, "暗影涌动", 1)
				}));
				return;
			}

			// 神谕者摩戈尔 任务12
			if (title.Contains("鲜血淋漓"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "鱼人弹幕", 1),
					TaskAdapter.GetMercenary(40, "邪鳍导航员", 1),
					TaskAdapter.GetMercenary(149, "小鱼快冲", 2)
				}));
				return;
			}

			// 空军上将罗杰斯 任务2
			if (title.Contains("列队飞行"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "战术大师", 0, 0, HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}

			// 珑心	任务3 任务10
			if (title.Contains("远古洞窟") || title.Contains("修习飞举"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "群星簇拥", 1, 0, HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}

			//泰瑞尔 任务2
			if (title.Contains("巨大的谎言"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "1-10", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子",EquipConst.炽烧符文),
					TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火",EquipConst.焚火印记),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯,null,EquipConst.次级水元素),
					TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击",EquipConst.圣羽之辉),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉,"致盲之光",EquipConst.强光魔杖),
					TaskAdapter.GetMercenary(MercConst.剑圣萨穆罗,"二连击",EquipConst.燃烧之刃)

				}));
				return;
			}
			//泰瑞尔 任务10
			if (title.Contains("黑暗流亡者"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-9", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子",EquipConst.炽烧符文),
					TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火",EquipConst.焚火印记),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯,null,EquipConst.次级水元素),
					TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击",EquipConst.圣羽之辉),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉,"致盲之光",EquipConst.强光魔杖),
					TaskAdapter.GetMercenary(MercConst.剑圣萨穆罗,"二连击",EquipConst.燃烧之刃)

				}));
				return;
			}
			//泰瑞尔 任务12
			if (title.Contains("恶魔再临"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子",EquipConst.炽烧符文),
					TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火",EquipConst.焚火印记),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯,null,EquipConst.次级水元素),
					TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击",EquipConst.圣羽之辉),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉,"致盲之光",EquipConst.强光魔杖),
					TaskAdapter.GetMercenary(MercConst.剑圣萨穆罗,"二连击",EquipConst.燃烧之刃)

				}));
				return;
			}
			//泰瑞尔 任务14
			if (title.Contains("夺魂之镰"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H5-2", new MercenaryEntity[]
				{

					TaskAdapter.GetMercenary(MercConst.巴琳达_斯通赫尔斯,null,EquipConst.次级水元素),
					TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,"死吧，虫子",EquipConst.炽烧符文),
					TaskAdapter.GetMercenary(MercConst.迦顿男爵,"地狱火",EquipConst.焚火印记),
					TaskAdapter.GetMercenary(mercenaryId,"圣剑挺击",EquipConst.圣羽之辉),
					TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉,null,EquipConst.凝聚冰凌),
					TaskAdapter.GetMercenary(MercConst.吉安娜_普罗德摩尔,"浮冰术",EquipConst.寒冰屏障护身符)

				}));
				return;
			}
			if (title.Contains("势均力敌") || title.Contains("无坚不摧"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "闪电军团", 0),
					TaskAdapter.GetMercenary(MercConst.玛诺洛斯, null, 0)
				}));
				return;
			}
			//洛卡拉 任务9 任务12
			if (title.Contains("新的任务") || title.Contains("毒性猛烈"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "部族战争", 0),
					TaskAdapter.GetMercenary(MercConst.剑圣萨穆罗, null, 0)
				}));
				return;
			}

			//拉索利安 任务12
			if (title.Contains("战场主宰"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId,"巨型大恶魔",EquipConst.恶魔灰烬),
				}));
				return;
			}
			//拉索利安 任务17
			if (title.Contains("巨型邪魔"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "巨型大恶魔", EquipConst.恶魔灰烬),
					TaskAdapter.GetMercenary(MercConst.洛卡拉, "进攻集结", EquipConst.激励头盔)
				}));
				return;
			}
			//瓦尔登·晨拥 任务9 
			if (title.Contains("学术道德"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.吉安娜_普罗德摩尔, "水元素", EquipConst.冰霜之尘),
					TaskAdapter.GetMercenary(mercenaryId, "冰枪术", EquipConst.寒冰药水)
				}));
				return;
			}
			//瓦尔登·晨拥 任务10
			if (title.Contains("调节温度"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "冰枪术", EquipConst.霜冻之戒),
					TaskAdapter.GetMercenary(mercenaryId, "急速冰冻", EquipConst.霜冻之戒)
				}));
				return;
			}
			//瓦尔登·晨拥 任务12
			if (title.Contains("附加影响"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "冰风暴", EquipConst.寒冰药水),
					TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, EquipConst.刺骨寒风)
				}));
				return;
			}
			//先知维伦 任务12
			if (title.Contains("圣光扫荡一切"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "维伦的祝福", EquipConst.受祝福的碎片),
					TaskAdapter.GetMercenary(mercenaryId, "裂解之光", EquipConst.受祝福的碎片),
					TaskAdapter.GetMercenary(MercConst.安度因_乌瑞恩, "神圣新星", EquipConst.祥和钟杵)
				}));
				return;
			}
			//先知维伦 任务17
			if (title.Contains("完美圣光"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "裂解之光", EquipConst.受祝福的碎片),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉, null, 0)
				}));
				return;
			}
			//奈法利安 任务12 
			if (title.Contains("暗影烈焰"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "暗影烈焰", EquipConst.多彩龙军团),
					TaskAdapter.GetMercenary(MercConst.希奈丝特拉,"暮光灭绝", 0),
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", EquipConst.玉火之矛)
				}));
				return;
			}
			//奈法利安 任务17
			if (title.Contains("还算有用"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId,"多彩能量", EquipConst.多彩龙军团),
					TaskAdapter.GetMercenary(MercConst.凯瑞尔_罗姆,"光明圣印",EquipConst.裁决秘典),
					TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯,"暗影之爪",EquipConst.野葡萄藤)
				}));
				return;
			}
			//拉格纳罗斯 任务10 
			if (title.Contains("全部烧光"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "熔岩冲击", EquipConst.熔火之心),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯, null, 0)
				}));
				return;
			}
			//娜塔莉·赛琳 任务4
			if (title.Contains("圣光治愈一切"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "咒逐", 0),
					TaskAdapter.GetMercenary(mercenaryId, "祈福", 0)
				}));
				return;
			}
			//弗丁 任务2 任务14
			if (title.Contains("王者祝福") || title.Contains("银色北伐军"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "王者祝福", 0, 0, HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}
			//阿莱克斯塔萨 任务9 
			if (title.Contains("生命守护者"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "巨龙吐息", 0, 0, HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}
			//阿莱克斯塔萨 任务17
			if (title.Contains("他们的力量属于我"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "红龙女王的计策", 0)
				}));
				return;
			}
			//阿莱克斯塔萨 任务12 任务14
			if (title.Contains("一举两得") || title.Contains("蹈踏火焰"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "烈焰猛击", 0),
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
				}));
				return;
			}
			//乌瑟尔 任务12
			if (title.Contains("光明使者"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "复仇之怒", 0),
					TaskAdapter.GetMercenary(MercConst.凯瑞尔_罗姆, "光明圣印", 0),
					TaskAdapter.GetMercenary(MercConst.考内留斯_罗姆, "牺牲祝福", 0)
				}));
				return;
			}

			// 吉安娜·普罗德摩尔 任务12
			if (title.Contains("千钧一发"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.伊莉斯_逐星, "黄金猿", 2),
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
					TaskAdapter.GetMercenary(mercenaryId, "浮冰术", 2)
				}));
				return;
			}

			// 安东尼达斯 任务12
			if (title.Contains("火焰滚滚"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
					TaskAdapter.GetMercenary(MercConst.赤精, "振奋之歌", 1),
					TaskAdapter.GetMercenary(mercenaryId, "火球风暴", 2)
				}));
				return;
			}

			// 安东尼达斯 任务9
			if (title.Contains("保护肯瑞托"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.拉格纳罗斯, "熔岩冲击", 2),
					TaskAdapter.GetMercenary(mercenaryId, "烈焰风暴", 1)
				}));
				return;
			}

			//迦顿 任务12
			if (title.Contains("烧烤弱鸡"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "活体炸弹", 0),
					TaskAdapter.GetMercenary(MercConst.厨师曲奇, "小鱼快冲", 0)
				}));
				return;
			}
			//迦顿 任务10 任务14
			if (title.Contains("侵略如火") || title.Contains("翻腾火流"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "热力迸发", 0),
					TaskAdapter.GetMercenary(MercConst.安东尼达斯, "火球术", 0),
					TaskAdapter.GetMercenary(MercConst.拉格纳罗斯, "熔岩冲击", 0)
				}));
				return;
			}
			//斯尼德 任务9
			if (title.Contains("手脚并用"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "缴械", 0),
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
					TaskAdapter.GetMercenary(MercConst.梵妮莎_范克里夫, "剽窃轰击", EquipConst.恐惧之刃)
				}));
				return;
			}
			//斯尼德 任务12 任务14
			if (title.Contains("一刀两断")|| title.Contains("乌合之众"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "滋啦", EquipConst.加装锯刃)
				}));
				return;
			}
			//希奈斯特拉 任务2
			if (title.Contains("何必忧伤"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "法力壁垒", EquipConst.法力胸针, 0, HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}
			//风行者 任务 12
			if(title.Contains("无论死活"))
            {
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "重拾灵魂", EquipConst.死亡箭雨),
					TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯, "构筑魔像", EquipConst.野葡萄藤),
					TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, "眼魔船工", EquipConst.武器柜)
				}));
				return;
			}
			//范达尔·雷矛 任务17 
			if(title.Contains("拼尽全力"))
            {
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId,"向前推进", EquipConst.无法撼动之物),
					TaskAdapter.GetMercenary(MercConst.凯瑞尔_罗姆,"光明圣印",EquipConst.裁决秘典),
					TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯,"暗影之爪",EquipConst.野葡萄藤)
				}));
				return;
            }
			//雪怒 任务16
			if (title.Contains("战斗热血"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId,"白虎飞扑", EquipConst.天神胸甲),
					TaskAdapter.GetMercenary(MercConst.凯瑞尔_罗姆,"光明圣印",EquipConst.裁决秘典),
					TaskAdapter.GetMercenary(MercConst.魔像师卡扎库斯,"暗影之爪",EquipConst.野葡萄藤)
				}));
				return;
			}
			//老瞎眼 任务9
			if (title.Contains("鱼类学"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId,"邪鳍导航员",EquipConst.导航员的护符)
				}));
				return;
            }
			//老瞎眼 任务12
			if(title.Contains("鱼多势众"))
            {
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId,"鱼人入侵",0),
					TaskAdapter.GetMercenary(MercConst.厨师曲奇,null,0),
					TaskAdapter.GetMercenary(MercConst.神谕者摩戈尔,null,0)
				}));
				return;
            }
			//瓦里安·乌瑞恩 任务3 任务17
			if (title.Contains("为了暴风城") || title.Contains("屹立不倒"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 4, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "反击", EquipConst.战争旗帜)
				}));
				return;
			}
			if (title.Contains("船上的厨子"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, null, 0),
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//奥妮克希亚 任务10 任务16
			if (title.Contains("喷吐火焰") || title.Contains("团本首领下场"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "清理巢穴", 0),
					TaskAdapter.GetMercenary(MercConst.拉格纳罗斯,null,EquipConst.炽烧符文)
				}));
				return;
			}
			//雪王 任务12
			if (title.Contains("雪球滚滚"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "3-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "雪球", EquipConst.刺骨寒风),
					TaskAdapter.GetMercenary(MercConst.瓦尔登_晨拥, "急速冰冻",EquipConst.霜冻之戒),
					TaskAdapter.GetMercenary(MercConst.吉安娜_普罗德摩尔, "浮冰术",EquipConst.寒冰屏障护身符)
				}));
				return;
			}
			//艾德温·迪菲亚首脑 任务11 任务17
			if (title.Contains("致命一击")|| title.Contains("身先士卒"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "首脑的悬赏", EquipConst.公平分配),
					TaskAdapter.GetMercenary(MercConst.海盗帕奇斯, null, 0),
					TaskAdapter.GetMercenary(MercConst.尤朵拉, null, 0)
				}));
				return;
			}
			//斯卡布斯·刀油 任务1 任务9
			if (title.Contains("秘密探员")|| title.Contains("二加二等于五"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0,  new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "战术打击", EquipConst.军情七处合约),
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0)
					
				}));
				return;
			}
			//迪亚波罗
			if (title.Contains("死亡降临"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0,"2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, "火焰践踏", 0),
					TaskAdapter.GetMercenary(MercConst.玉珑, "玉火打击", 0),
					TaskAdapter.GetMercenary(MercConst.瓦莉拉_萨古纳尔, "影袭", EquipConst.异常烟尘),
				}));
				return;
			}
			
			//回复生命类任务
			//赛琳 任务17  维伦 任务14 光明之翼 任务17 任务11 希奈斯特拉 任务12 
			if (title.Contains("平衡倾斜") || title.Contains("纯净圣光") ||
				title.Contains("治疗朋友") || title.Contains("我们交朋友吧") ||
				title.Contains("低语显现"))
			{
				
				tasks.Add(TaskAdapter.GetTask(taskId, 0, "2-5", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.光明之翼,"妖精之尘", EquipConst.妖精口袋),
					TaskAdapter.GetMercenary(MercConst.赤精,"振奋之歌", EquipConst.赤精之杖),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉,"快速治疗", EquipConst.强光魔杖,0,HsMercenaryStrategy.TARGETTYPE.FRIENDLY)
				}));
				return;
			}
			//杀恶魔
			if (desc.Contains("击败") && desc.Contains("个恶魔"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "2-6", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//杀龙
			if (desc.Contains("击败") && desc.Contains("条龙"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "4-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//杀野兽
			if (desc.Contains("击败") && desc.Contains("只野兽"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//杀元素
			if (desc.Contains("击败") && desc.Contains("元素"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//加快技能速度
			if (desc.Contains("加快友方技能的速度值总") || desc.Contains("使一个友方技能的速度值加快"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 5, "2-5", TaskAdapter.GetQuickMercenary(mercenaryId)));
				return;
			}
			//恶魔造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用恶魔造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.拉索利安, "巨型大恶魔", EquipConst.恶魔灰烬)
				}));
				return;
			}
			//野兽造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用野兽造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.闪狐, null, 0)
				}));
				return;
			}
			//元素造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用元素造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉, null, 0)
				}));
				return;
			}
			//龙造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用龙造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.奈法利安,"龙人突袭",EquipConst.多彩龙军团)

				}));
				return;
            }
			//人类造成伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("使用人类造成"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.凯瑞尔_罗姆,"远征军打击",EquipConst.黎明之锤)

				}));
				return;
			}
			//造成神圣伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("神圣伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.泽瑞拉, null, 0)
				}));
				return;
			}
			//造成冰霜伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("冰霜伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0),
					TaskAdapter.GetMercenary(MercConst.冰雪之王洛克霍拉,"冰雹", EquipConst.刺骨寒风)
				}));
				return;
			}
			//造成火焰伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("火焰伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.安东尼达斯, "火球术", EquipConst.烬核之杖),
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//造成暗影伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("暗影伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.塔姆辛_罗姆, null, 0),
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//造成邪能伤害
			if (desc.Contains("使用包含此佣兵的队伍") && desc.Contains("邪能伤害"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 0, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(MercConst.玛诺洛斯, null, 0),
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			//英雄难度相关
			if (desc.Contains("英雄难度"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 1, "H1-1", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			if (desc.Contains("完成") && desc.Contains("个悬赏"))
			{
				LettuceMercenary mercenary = HsGameUtils.GetMercenary(mercenaryId);
				if (desc.Contains("30级时") && !mercenary.IsMaxLevel())
				{
					tasks.Add(TaskAdapter.GetTask(taskId, 10, new MercenaryEntity[]
					{
						TaskAdapter.GetMercenary(mercenaryId, null, 0)
					}));
				}
				else
				{
					tasks.Add(TaskAdapter.GetTask(taskId, 5, "H1-1", new MercenaryEntity[]
					{
						TaskAdapter.GetMercenary(mercenaryId, null, 0)
					}));
				}
				return;
			}
			//赐福
			if (desc.Contains("利用赐福"))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, 10, "H1-2", new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			int num = desc.IndexOf("$ability(", StringComparison.Ordinal);
			if (num == -1)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			int num2 = num + "$ability(".Length;
			int num3 = desc.IndexOf(")", num2, StringComparison.Ordinal);
			if (num3 == -1)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			string[] array = desc.Substring(num2, num3 - num2).Split(new char[]
			{
				','
			});
			int num4;
			int num5;
			if (!int.TryParse(array[0], out num4) || !int.TryParse(array[1], out num5))
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			LettuceMercenaryDbfRecord record = GameDbf.LettuceMercenary.GetRecord(mercenaryId);
			if (num4 >= record.LettuceMercenarySpecializations.Count)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			LettuceMercenarySpecializationDbfRecord lettuceMercenarySpecializationDbfRecord = record.LettuceMercenarySpecializations[num4];
			if (num5 >= lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities.Count)
			{
				tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
				{
					TaskAdapter.GetMercenary(mercenaryId, null, 0)
				}));
				return;
			}
			int lettuceAbilityId = lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities[num5].LettuceAbilityId;
			LettuceAbilityDbfRecord record2 = GameDbf.LettuceAbility.GetRecord(lettuceAbilityId);
			tasks.Add(TaskAdapter.GetTask(taskId, new MercenaryEntity[]
			{
				TaskAdapter.GetMercenary(mercenaryId, record2.AbilityName, GetEquipEnHanceSkill(record2.AbilityName))
			}));
		}
		
		// Token: 0x06000054 RID: 84 RVA: 0x00005BB0 File Offset: 0x00003DB0
		private static MercenaryEntity[] GetQuickMercenary(int mercenaryId)
		{
			List<MercenaryEntity> list = new List<MercenaryEntity>();
			foreach (LettuceAbility lettuceAbility in CollectionManager.Get().GetMercenary((long)mercenaryId, false, true).m_abilityList)
			{
				CardDbfRecord cardRecord = GameDbf.GetIndex().GetCardRecord(lettuceAbility.GetCardId());
				string @string = cardRecord.TextInHand.GetString(true);
				if (@string != null && @string.Contains("速度值") && @string.Contains("加快"))
				{
					string string2 = cardRecord.Name.GetString(true);
					list.Add(TaskAdapter.GetMercenary(mercenaryId, string2.Substring(0, string2.Length - 1), 0));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005C80 File Offset: 0x00003E80
		private static Task GetTask(int id, int priority, string map, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Priority = priority,
				Map = map,
				Mercenaries = new List<MercenaryEntity>(mercenaries),
				Id = id
			};
		}

		private static Task GetTask(int id, int priority, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Id = id,
				Mercenaries = new List<MercenaryEntity>(mercenaries),
				Priority = priority,
			};
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005CA8 File Offset: 0x00003EA8
		private static Task GetTask(int id, params MercenaryEntity[] mercenaries)
		{
			return new Task
			{
				water = ++m_globalWater,
				Id = id,
				Mercenaries = new List<MercenaryEntity>(mercenaries),
				Priority = 5
			};
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005CC9 File Offset: 0x00003EC9
		private static MercenaryEntity GetMercenary(int id, string skill = null, int eq = 0, int subskill = 0, HsMercenaryStrategy.TARGETTYPE targettype = HsMercenaryStrategy.TARGETTYPE.UNSPECIFIED)
		{
			return new MercenaryEntity(id, skill, eq, subskill, targettype);
		}

		private static int GetEquipEnHanceSkill(string skill)
		{
			return m_dictSkillEquip.ContainsKey(skill) ? m_dictSkillEquip[skill] : 0;
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
