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

		private static Dictionary<string, int> m_dictSkillEquip = new Dictionary<string, int>{
			{ "制裁之锤", EquipConst.光明使者 },
			{ "保护祝福", EquipConst.微光肩甲 },
			{ "复仇之怒", EquipConst.牺牲圣契 },
			{ "亵渎攻击", EquipConst.亚煞极印记 },
			{ "亚煞极之力", EquipConst.亚煞极之心 },
			{ "无羁之怒", EquipConst.亚煞极神像 },
			{ "侧翼突击", EquipConst.拳刃 },
			{ "流放攻击", EquipConst.埃辛诺斯战刃 },
			{ "刃舞", EquipConst.恶魔斗篷 },
			{ "守备官之怒", EquipConst.纳鲁之锤 },
			{ "耀眼圣光", EquipConst.智慧圣契 },
			{ "光缚之怒", EquipConst.卡拉波之光 },
			{ "逐星", EquipConst.利爪饰品 },
			{ "指引道路", EquipConst.绿洲水壶 },
			{ "黄金猿任务", EquipConst.猴爪 },
			{ "神圣冲击", EquipConst.激励秘典 },
			{ "裂解之光", EquipConst.受祝福的碎片 },
			{ "维伦的祝福", EquipConst.圣光药水 },
			{ "精灵吐息", EquipConst.光明之翼的项链 },
			{ "妖精之尘", EquipConst.妖精口袋 },
			{ "相位变换", EquipConst.光明之翼的圆环 },
			{ "冰雹", EquipConst.凝聚冰凌 },
			{ "冰霜震击", EquipConst.刺骨寒风 },
			{ "雪球", EquipConst.冰雪之王的遮蔽 },
			{ "先祖勾拳", EquipConst.迅捷图腾 },
			{ "坚韧光环", EquipConst.雷霆饰带 },
			{ "远征军打击", EquipConst.黎明之锤 },
			{ "嘲讽", EquipConst.裁决秘典 },
			{ "光明圣印", EquipConst.圣光秘典 },
			{ "二连击", EquipConst.幻觉饰带 },
			{ "镜像", EquipConst.磨砺之刃 },
			{ "旋风之刃", EquipConst.燃烧之刃 },
			{ "撕裂挥砍", EquipConst.霜狼之怒 },
			{ "致命打击", EquipConst.德雷克塔尔的恩惠 },
			{ "破胆怒吼", EquipConst.震耳咆哮 },
			{ "荣耀决斗", EquipConst.决斗护手 },
			{ "部落之力", EquipConst.玛诺洛斯的獠牙 },
			{ "战斗怒吼", EquipConst.兽人的旗帜 },
			{ "军团爆能", EquipConst.狂怒坠饰 },
			{ "加拉克苏斯之拳", EquipConst.实击护手 },
			{ "邪能地狱火", EquipConst.邪能核心 },
			{ "鱼类大餐", EquipConst.调味的锅 },
			{ "曲奇下厨", EquipConst.钢铁汤勺 },
			{ "小鱼快冲", EquipConst.开胃前菜 },
			{ "邪恶计谋", EquipConst.木乃伊面具 },
			{ "流变打击", EquipConst.拟态面具 },
			{ "叛变", EquipConst.愤怒面具 },
			{ "符文猛击", EquipConst.盛开菌菇 },
			{ "活体荆棘", EquipConst.木棘图腾 },
			{ "铁木树皮", EquipConst.土灵护腕 },
			{ "邪能箭", EquipConst.玛诺洛斯之血 },
			{ "毁灭之雨", EquipConst.灵魂护符 },
			{ "生命虹吸", EquipConst.混乱之杖 },
			{ "冰刺", EquipConst.寒冰碎片 },
			{ "浮冰术", EquipConst.冰霜之尘 },
			{ "水元素", EquipConst.寒冰屏障护身符 },
			{ "暗影箭", EquipConst.暗影符文 },
			{ "虚空吞噬者", EquipConst.虚空石 },
			{ "暗影之幕", EquipConst.最终仪祭 },
			{ "瞄准射击", EquipConst.高能枪弹 },
			{ "爆炸陷阱", EquipConst.狩猎篷布 },
			{ "捕熊陷阱", EquipConst.拉线操纵 },
			{ "龙人突袭", EquipConst.实验对象 },
			{ "多彩能量", EquipConst.多彩龙军团 },
			{ "暗影烈焰", EquipConst.备用肢体 },
			{ "黏液时间", EquipConst.虾戮时间 },
			{ "安全泡泡", EquipConst.又湿又滑 },
			{ "气泡鱼", EquipConst.鱼人冲锋 },
			{ "傲慢的凡人", EquipConst.锋刃之爪 },
			{ "深呼吸", EquipConst.更深的呼吸 },
			{ "清理巢穴", EquipConst.巢母之怒 },
			{ "集束暗影", EquipConst.暗影之眼 },
			{ "集束之光", EquipConst.神圣之眼 },
			{ "咒逐", EquipConst.诺达希尔碎片 },
			{ "火球术", EquipConst.烬核之杖 },
			{ "烈焰风暴", EquipConst.烈焰饰环 },
			{ "火球风暴", EquipConst.灼热吊坠 },
			{ "毒蛇噬咬", EquipConst.剧毒毒液 },
			{ "野兽恢复", EquipConst.狂野的徽记 },
			{ "梦魇毒蛇", EquipConst.毒蛇点心 },
			{ "苦修", EquipConst.祥和钟杵 },
			{ "神圣新星", EquipConst.疗愈长袍 },
			{ "圣言术：赎", EquipConst.纯洁指环 },
			{ "火炮突击", EquipConst.弹片射击 },
			{ "准备火炮", EquipConst.装填武器 },
			{ "掩护射击", EquipConst.咸水护腕 },
			{ "疯乱凝视", EquipConst.尖利凝视 },
			{ "心灵癫狂", EquipConst.狂乱抽笞 },
			{ "古神在上", EquipConst.觉醒咆哮 },
			{ "寒冰之噬", EquipConst.霜之哀伤 },
			{ "凋零缠绕", EquipConst.虚空之踏 },
			{ "寒冰之盾", EquipConst.统御头盔 },
			{ "烈焰之刺", EquipConst.次级火元素 },
			{ "冰霜之刺", EquipConst.次级水元素 },
			{ "冰霜灼烧", EquipConst.雷矛的拯救 },
			{ "可靠的皮鞭", EquipConst.奥丹姆遗物 },
			{ "山巅营火", EquipConst.冒险者的包裹 },
			{ "铜须风范", EquipConst.意气风发 },
			{ "闪电箭", EquipConst.雷鸣系带 },
			{ "陷足泥泞", EquipConst.霜木图腾 },
			{ "闪电链", EquipConst.引雷针 },
			{ "破坏扫击", EquipConst.弯钩利爪 },
			{ "法力壁垒", EquipConst.法力胸针 },
			{ "暮光灭绝", EquipConst.哀伤碎片 },
			{ "女妖之箭", EquipConst.灵魂缶 },
			{ "重拾灵魂", EquipConst.哀伤之剑 },
			{ "为了女王", EquipConst.死亡箭雨 },
			{ "屠魔者", EquipConst.混乱护符 },
			{ "盲目突击", EquipConst.邪能之刃 },
			{ "眼棱", EquipConst.恶魔卫士 },
			{ "血之狂暴", EquipConst.冰川之刃 },
			{ "灵魂向导", EquipConst.德雷克塔尔的法术书 },
			{ "邪能腐蚀", EquipConst.微光草药水 },
			{ "恩佐斯的子嗣", EquipConst.异变之力 },
			{ "恩佐斯之赐", EquipConst.腐化的神经元 },
			{ "腐化脏器", EquipConst.永恒折磨 },
			{ "蠕行疯狂", EquipConst.恐惧奇美拉 },
			{ "扫尾", EquipConst.黑龙鳞片 },
			{ "真正形态", EquipConst.拉希奥的角 },
			{ "熔岩冲击", EquipConst.熔火之心 },
			{ "死吧，虫子", EquipConst.萨弗拉斯 },
			{ "陨石术", EquipConst.炽烧符文 },
			{ "闪电军团", EquipConst.恶魔灰烬 },
			{ "巨型大恶魔", EquipConst.灵魂宝石 },
			{ "邪能命令", EquipConst.恶魔印记 },
			{ "飞斧投掷", EquipConst.巨龙印记 },
			{ "火焰吐息", EquipConst.巨龙之爪 },
			{ "辟法巨龙", EquipConst.巨龙符文斧 },
			{ "神圣突击", EquipConst.王者之盔 },
			{ "王者祝福", EquipConst.灰烬使者 },
			{ "谦逊制裁", EquipConst.提里奥的护盾 },
			{ "战术打击", EquipConst.军情七处合约 },
			{ "邪恶挥刺", EquipConst.剥皮刀 },
			{ "暗影之刃", EquipConst.精磨之杖 },
			{ "嗞啦", EquipConst.颅骨之尘 },
			{ "启动电锯", EquipConst.泰坦神铁锯刃 },
			{ "缴械", EquipConst.加装锯刃 },
			{ "顶级捕食者", EquipConst.新鲜的肉 },
			{ "惊骇", EquipConst.岩质甲壳 },
			{ "魔暴龙", EquipConst.烈焰利爪 },
			{ "热血", EquipConst.血吼 },
			{ "惊愕猛击", EquipConst.休止饰带 },
			{ "战斗怒火", EquipConst.饮血坠饰 },
			{ "癫狂乱舞", EquipConst.燃烧射击 },
			{ "屠龙射击", EquipConst.巨龙之颅 },
			{ "龙喉偷猎者", EquipConst.龙爪之拳 },
			{ "剽窃轰击", EquipConst.恐惧之刃 },
			{ "临时武器", EquipConst.加装口袋 },
			{ "搬运背包", EquipConst.夺刀手 },
			{ "恐怖利爪", EquipConst.不死者之心 },
			{ "源质护甲", EquipConst.不朽者之毅 },
			{ "毁灭万物", EquipConst.恶魔之魂 },
			{ "暗影震击", EquipConst.静滞海马 },
			{ "暗影涌动", EquipConst.沃金战刃 },
			{ "虚弱诅咒", EquipConst.急速指环 },
			{ "奥术射击", EquipConst.艾露恩的护符 },
			{ "奥术齐射", EquipConst.苍翠反曲弓 },
			{ "艾露恩的赐福", EquipConst.狂野之戒 },
			{ "圣剑挺击", EquipConst.圣羽之辉 },
			{ "神圣审判", EquipConst.夺目护手 },
			{ "天使庇护", EquipConst.纯洁之冠 },
			{ "致盲之光", EquipConst.强光魔杖 },
			{ "快速治疗", EquipConst.纳鲁碎片 },
			{ "救赎", EquipConst.纯洁长袍 },
			{ "部族战争", EquipConst.霜狼护身符 },
			{ "进攻集结", EquipConst.激励头盔 },
			{ "兽人猛攻", EquipConst.先祖护甲 },
			{ "女巫诅咒", EquipConst.冰冷出击 },
			{ "冰冻之触", EquipConst.侍者新贵 },
			{ "碎浪多头怪", EquipConst.迟缓之环 },
			{ "眼魔船工", EquipConst.勾住船员 },
			{ "眼魔船长", EquipConst.武器柜 },
			{ "全员开火", EquipConst.轮到我了 },
			{ "海潮打击", EquipConst.深水诱饵 },
			{ "灾难之箭", EquipConst.甲板破拆 },
			{ "海潮祝福", EquipConst.踏潮饰针 },
			{ "学识", EquipConst.卷轴珍藏 },
			{ "耐心", EquipConst.中立之心 },
			{ "智慧", EquipConst.周氏传家宝 },
			{ "滑矛之怒", EquipConst.出笼鳗鱼 },
			{ "健身兄弟", EquipConst.鱼人大锅 },
			{ "处理垃圾", EquipConst.锐利思维 },
			{ "波浪冲击", EquipConst.督军帕杰什 },
			{ "激流", EquipConst.紧握仇恨 },
			{ "俘获之潮", EquipConst.蔑视 },
			{ "狂野挥舞", EquipConst.大宝剑 },
			{ "吃炸鸡", EquipConst.需要治疗 },
			{ "火车王", EquipConst.需求怪 },
			{ "青玉劲风", EquipConst.玉火之矛 },
			{ "玉火打击", EquipConst.翔龙吊坠 },
			{ "氤氲之雾", EquipConst.玉珑骊珠 },
			{ "塞纳里奥波动", EquipConst.森林徽章 },
			{ "纠缠根须", EquipConst.石南草 },
			{ "大德鲁伊的召唤", EquipConst.活根草之杖 },
			{ "专心追捕", EquipConst.充能战刃 },
			{ "致命一击", EquipConst.看守之眼 },
			{ "禁锢", EquipConst.守望者斗篷 },
			{ "奔跑破坏神", EquipConst.深渊领主之杖 },
			{ "邪能抽笞", EquipConst.邪能尖刺 },
			{ "恐惧嚎叫", EquipConst.邪能魔肺 },
			{ "天神之息", EquipConst.群星之瓶 },
			{ "星火祝福", EquipConst.昆莱之晶 },
			{ "群星簇拥", EquipConst.爆裂新星指环 },
			{ "放电", EquipConst.寒冰之握 },
			{ "侍女的药膏", EquipConst.女巫披风 },
			{ "双曲射击", EquipConst.邪能坩埚 },
			{ "冰风暴", EquipConst.寒冰药水 },
			{ "急速冰冻", EquipConst.霜冻之戒 },
			{ "冰枪术", EquipConst.冰风护符 },
			{ "动员打击", EquipConst.萨隆踏靴 },
			{ "旋风斩", EquipConst.狂战士之刃 },
			{ "刃手狂战士", EquipConst.锯齿盾牌 },
			{ "影袭", EquipConst.暗影匕首 },
			{ "伏击", EquipConst.隐秘踪迹 },
			{ "刀扇", EquipConst.异常烟尘 },
			{ "爆裂打击", EquipConst.萨拉迈恩 },
			{ "英勇飞跃", EquipConst.界限之靴 },
			{ "反击", EquipConst.战争旗帜 },
			{ "迎头冲撞", EquipConst.玄牛之带 },
			{ "玄牛之韧", EquipConst.砮皂之盔 },
			{ "神牛赐福", EquipConst.玄牛护符 },
			{ "鱼人飞弹", EquipConst.摩戈尔的手套 },
			{ "鱼人弹幕", EquipConst.尖刺泡泡鱼 },
			{ "治疗波", EquipConst.润泽之杖 },
			{ "暴食香蕉", EquipConst.新鲜香蕉 },
			{ "大餐时间", EquipConst.辐射香蕉 },
			{ "原始之力", EquipConst.穆克拉的大表哥 },
			{ "吞噬攻击", EquipConst.炫彩项链 },
			{ "鳞甲嘲讽", EquipConst.珠光之鳞 },
			{ "吞噬", EquipConst.土灵护甲 },
			{ "天空守卫", EquipConst.浮夸腰带 },
			{ "战术大师", EquipConst.船铃 },
			{ "开火姿态", EquipConst.侦查望远镜 },
			{ "魔爆术", EquipConst.奥术粉尘 },
			{ "奥术箭", EquipConst.法力魔棒 },
			{ "强能奥术飞弹", EquipConst.魔网之杖 },
			{ "鱼人入侵", EquipConst.泡泡魔杖 },
			{ "邪鳍导航员", EquipConst.导航员的护符 },
			{ "老蓟皮", EquipConst.始生鱼人 },
			{ "武艺精通" , EquipConst.猛击护手 },
			{ "坚守前线" , EquipConst.启迪环带 },
			{ "牺牲祝福" , EquipConst.黎明之盾 },
			{ "刺客之刃", EquipConst.黑色船旗 },
			{ "辅助打击", EquipConst.轮番豪饮},
			{ "首脑的悬赏", EquipConst.公平分配 },
			{ "变换之潮", EquipConst.萨拉塔斯 },
			{ "法师巅峰", EquipConst.高戈奈斯潮汐之石 },
			{ "傲人训诫", EquipConst.莎拉达尔_女王权杖 },
			{ "雷霆打击", EquipConst.无坚不摧之力 },
			{ "向前推进", EquipConst.无法撼动之物 },
			{ "天神下凡", EquipConst.雷矛化身 },
			{ "为了部落", EquipConst.华丽的军号 },
			{ "闪电风暴", EquipConst.力量之戒 },
			{ "大酋长的命令", EquipConst.毁灭之锤 },
			{ "莫高雷之力", EquipConst.图腾掌握 },
			{ "治疗链", EquipConst.家传草药 },
			{ "大地母亲之怒", EquipConst.酋长的羽毛 },
			{ "烈焰之歌", EquipConst.烈焰系带 },
			{ "振奋之歌", EquipConst.赤精之杖 },
			{ "火雨风暴", EquipConst.焰心水晶 },
			{ "热力迸发", EquipConst.熔岩之刃 },
			{ "地狱火", EquipConst.焚火印记 },
			{ "活体炸弹", EquipConst.燃烧护腕 },
			{ "末日冲锋", EquipConst.熔岩魔角 },
			{ "火焰践踏", EquipConst.恐怖利爪 },
			{ "末日", EquipConst.黑暗灵魂石 },
			{ "重拳猛击", EquipConst.水手帽 },
			{ "停火", EquipConst.沉重铁锚 },
			{ "落水追击", EquipConst.锋锐剑鞘 },
			{ "反转一击", EquipConst.骷髅黑帆 },
			{ "海上威胁", EquipConst.船长的骄傲 },
			{ "激烈谈判", EquipConst.随身刀具 },
			{ "奥术飞掷", EquipConst.奥术尖牙 },
			{ "法力闪现", EquipConst.法力符文 },
			{ "心灵窃贼", EquipConst.第十条尾巴 },
			{ "巨龙吐息", EquipConst.阿莱克丝塔萨的胸针 },
			{ "烈焰猛击", EquipConst.巨龙军团护身符 },
			{ "红龙女王的计策", EquipConst.迅捷坠饰 },
			{ "均衡一击", EquipConst.雪怒之矛 },
			{ "白虎飞扑", EquipConst.天神胸甲 },
			{ "伏虎闪雷", EquipConst.滚雷护爪 },
			{ "杀戮命令", EquipConst.迅羽之弓 },
			{ "动物伙伴", EquipConst.猎手的步枪 },
			{ "爆炸射击", EquipConst.熊妈妈之爪 },
			{ "加特林法杖", EquipConst.探险帽 },
			{ "傲人肌肉", EquipConst.要发财了 },
			{ "宝藏是我的", EquipConst.宝物探员 },
			{ "狂暴攻击", EquipConst.注能琥珀 },
			{ "反冲", EquipConst.锋锐利爪 },
			{ "再生头颅", EquipConst.再生之鳞 },
			{ "暗影之爪", EquipConst.魔皇草 },
			{ "构筑魔像", EquipConst.皇血草 },
			{ "真实形态", EquipConst.野葡萄藤 },
			{ "大地践踏",EquipConst.雷霆饰带},

		};

		private static int m_globalWater = 0;
	}
}
