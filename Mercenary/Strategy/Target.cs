using System.Collections.Generic;

namespace Mercenary.Strategy
{
	public enum MY_TAG_ROLE
	{
		INVALID,
		CASTER,
		FIGHTER,
		TANK,
		NEUTRAL
	}
	
	public class Target
	{
		public int Id = -1;
		public string Name;
		public int DefHealth;//基础生命
		public int Health;// 当前生命
		public int Speed;//pve有效
		public int Attack;//攻击力
		public MY_TAG_ROLE Role;//颜色
		public bool Enable;//是否可作为目标
		public List<Skill> Skills;
		
		public int ForstEnhance;
		public int ForstWeak;
	}

	public class Skill
	{
		public int Id = -1;
		public string Name;
		public int Speed;
	}
}
