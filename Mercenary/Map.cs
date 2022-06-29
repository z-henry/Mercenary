using System;

namespace Mercenary
{
	// Token: 0x0200000C RID: 12
	public class Map
	{
		// Token: 0x06000052 RID: 82 RVA: 0x0000521E File Offset: 0x0000341E
		public Map(int id, string name)
		{
			this.ID = id;
			this.Name = name;
		}

		// Token: 0x04000038 RID: 56
		public readonly int ID;

		// Token: 0x04000039 RID: 57
		public readonly string Name;
	}
}
