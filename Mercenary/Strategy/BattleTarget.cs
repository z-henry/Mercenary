using System;

namespace Mercenary.Strategy
{
	public enum TARGETTYPE
	{
		UNSPECIFIED = 0,
		FRIENDLY = 1
	}

	
	public class BattleTarget
	{

		public int TargetId = -1;
		public string TargetName = "";
		public int SkillId = -1;
		public string SkillName = "";
		public TARGETTYPE TargetType = TARGETTYPE.UNSPECIFIED;
		public int SubSkillIndex = -1;
		public string SubSkillName = "";
		public string MercName = "";
		public bool NeedActive = true;

		public BattleTarget() 
		{ 
		}

// 		public BattleTarget(int _skill, string _skillname, int _subskillindex = -1, TARGETTYPE _targettype = TARGETTYPE.UNSPECIFIED) 
// 		{
// 			SkillId = _skill;
// 			TargetType = _targettype;
// 			SkillName = _skillname;
// 			SubSkillIndex = _subskillindex;
// 		}
	}
}
