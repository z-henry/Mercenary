using System;

namespace Mercenary
{
	// Token: 0x02000006 RID: 6
	public static class Out
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000417B File Offset: 0x0000237B
		public static void Log(string log)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000417D File Offset: 0x0000237D
		public static void UI(string log)
		{
			UIStatus.Get().AddInfo(log);
		}
	}
}
