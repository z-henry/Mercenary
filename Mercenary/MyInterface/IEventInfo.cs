using Mercenary.DefaultTeam;

namespace Mercenary.MyInterface
{
	public interface IEventInfo
	{
		string Map { get; }
		ITeam Team { get; }
	}
}