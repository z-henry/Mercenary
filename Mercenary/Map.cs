using System;

namespace Mercenary
{
	public class Map
	{
		public Map(int id, string name, string boss, Type teamtype = null)
		{
			this.ID = id;
			this.Name = name;
			this.Boss = boss;
			this.TeamType = teamtype ?? typeof(DefaultTeam.IceFire);
		}

		public readonly int ID;
		public readonly string Name;
		public readonly string Boss;
		public readonly Type TeamType;
	}
}