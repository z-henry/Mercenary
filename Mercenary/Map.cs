using System;

namespace Mercenary
{
	
	public class Map
	{
		
		public Map(int id, string name, string boss)
		{
			this.ID = id;
			this.Name = name;
			this.Boss = boss;
		}

		
		public readonly int ID;
		public readonly string Name;
		public readonly string Boss;
	}
}
