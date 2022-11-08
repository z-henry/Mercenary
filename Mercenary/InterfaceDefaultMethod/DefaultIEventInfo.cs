using Mercenary.DefaultTeam;
using Mercenary.MyInterface;

namespace Mercenary.InterfaceDefault
{
	public static class DefaultEventInfo
	{
		public static string DefaultEventInfoMethod_GetMap<T>(this T obj) where T : IEventInfo
		{
			return "1-1";
		}

		public static ITeam DefaultEventInfoMethod_GetTeam<T>(this T obj) where T : IEventInfo
		{
			return new Nature();
		}
	}
}