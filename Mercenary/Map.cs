using System;

namespace Mercenary
{
	
	public class Map
	{
		
		public Map(int id, string name)
		{
			this.ID = id;
			this.Name = name;
		}

		
		public readonly int ID;

		
		public readonly string Name;
	}
}
