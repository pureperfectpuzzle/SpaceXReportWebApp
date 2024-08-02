using Data.Objects.SpaceX;

namespace Data.Interfaces
{
    public interface ISpaceXRepository : IDisposable
    {
        int QueryLimit { get; set; }
        Task<IEnumerable<LaunchPad>> GetLaunchPadsAsync();
        Task<IEnumerable<Capsule>> GetCapsulesAsync();
        Task<IEnumerable<Launch>> GetLaunchesAsync();
		Task<IEnumerable<Launch>> GetUpcomingLaunchesAsync();
	}
}
