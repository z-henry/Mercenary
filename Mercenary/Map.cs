using Mercenary.DefaultTeam;
using System;

namespace Mercenary
{
	
	public class Map
	{
		
		public Map(int id, string name, string boss, IDefaultTeam team = null)
		{
			this.ID = id;
			this.Name = name;
			this.Boss = boss;
			this.Team = team ?? new TeamIceFire();
		}

		
		public readonly int ID;
		public readonly string Name;
		public readonly string Boss;
		public readonly IDefaultTeam Team;
	}
}
