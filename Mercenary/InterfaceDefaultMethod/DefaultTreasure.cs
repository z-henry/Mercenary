using Mercenary.MyInterface;
using System.Collections.Generic;

namespace Mercenary.InterfaceDefault
{
	public static class DefaultTreasure
	{
		public static List<string> DefaultTreasureMethod_GetTreasures<T>(this T obj) where T : ITreasure
		{
			return new List<string> {
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
		}
	}
}