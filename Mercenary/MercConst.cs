using System;
using System.Collections.Generic;

namespace Mercenary
{
	// Token: 0x02000005 RID: 5
	internal class MercConst
	{
		// Token: 0x04000018 RID: 24
		public const int VaLiLa = 17;

		// Token: 0x04000019 RID: 25
		public const int KaiRuiEr = 18;

		// Token: 0x0400001A RID: 26
		public const int ZeRuiLA = 19;

		// Token: 0x0400001B RID: 27
		public static int LUO_SI = 20;

		// Token: 0x0400001C RID: 28
		public static int JIA_DUN = 22;

		// Token: 0x0400001D RID: 29
		public const int ChenYong = 38;

		// Token: 0x0400001E RID: 30
		public static int TA_MU_XIN = 45;

		// Token: 0x0400001F RID: 31
		public static int LA_SUO_LI_AN = 55;

		// Token: 0x04000020 RID: 32
		public const int BinChiZhe = 71;

		// Token: 0x04000021 RID: 33
		public static int KAO_NEI_LIU_SI = 95;

		// Token: 0x04000022 RID: 34
		public static int AN_DONG_NI = 98;

		// Token: 0x04000023 RID: 35
		public static int SHAN_HU = 100;

		// Token: 0x04000024 RID: 36
		public static int QU_QI = 149;

		// Token: 0x04000025 RID: 37
		public const int BaLinDa = 249;

		// Token: 0x04000026 RID: 38
		public static int LUO_KE_HUO_LA = 251;

		// Token: 0x04000027 RID: 39
		public static int YU_LONG = 285;

		// Token: 0x04000028 RID: 40
		public static int CHI_JING = 286;

		// Token: 0x04000029 RID: 41
		public static int PA_QI_SI = 374;

		// Token: 0x0400002A RID: 42
		public static readonly List<int> First = new List<int>
		{
			249,
			17,
			MercConst.LUO_SI,
			MercConst.QU_QI,
			71
		};

		// Token: 0x0400002B RID: 43
		public static readonly List<int> Ignore = new List<int>
		{
			MercConst.LUO_KE_HUO_LA,
			MercConst.CHI_JING
		};
	}
}
