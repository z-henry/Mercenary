using Mercenary.DefaultTeam;
using Mercenary.InterfaceDefault;
using Mercenary.MyInterface;
using System.Collections.Generic;

namespace Mercenary.Strategy
{
	public class DefaultStrategyEvent : ITreasure, IStrategy, IEventInfo
	{
		public virtual (int hand_index, int play_index) GetEnterOrder(
			List<Target> hand_mercenaries, List<Target> play_mercenaries,
			Dictionary<MY_TAG_ROLE, int> dictOppositeRoleCount,
			List<Target> targets_opposite, List<Target> targets_opposite_graveyrad)
		{
			return this.DefaultStrategyMethod_GetEnterOrder(hand_mercenaries, play_mercenaries, dictOppositeRoleCount, targets_opposite, targets_opposite_graveyrad);
		}

		public virtual List<BattleTarget> GetBattleTargets(int turn, List<Target> targets_opposite_all, List<Target> targets_friendly_all, List<Target> targets_opposite_graveyrad)
		{
			return this.DefaultStrategyMethod_GetBattleTargets(turn, targets_opposite_all, targets_friendly_all, targets_opposite_graveyrad);
		}

		public virtual string Name
		{
			get { return "DefaultStrategy_Event"; }
		}

		public virtual List<string> Treasures
		{
			get { return this.DefaultTreasureMethod_GetTreasures(); }
		}

		public virtual string Map
		{
			get { return this.DefaultEventInfoMethod_GetMap(); }
		}

		public virtual ITeam Team
		{
			get { return this.DefaultEventInfoMethod_GetTeam(); }
		}
	}
}