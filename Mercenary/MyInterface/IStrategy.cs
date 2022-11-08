using Mercenary.Strategy;
using System.Collections.Generic;

namespace Mercenary.MyInterface
{
	public interface IStrategy
	{
		List<BattleTarget> GetBattleTargets(int turn,
			List<Target> targets_opposite, List<Target> targets_friendly, List<Target> targets_opposite_graveyrad);

		(int hand_index, int play_index) GetEnterOrder(
			List<Target> hand_mercenaries, List<Target> play_mercenaries,
			Dictionary<MY_TAG_ROLE, int> dictOppositeRoleCount,
			List<Target> targets_opposite, List<Target> targets_opposite_graveyrad);

		string Name { get; }
	}
}