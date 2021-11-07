using Microsoft.AspNetCore.SignalR;

namespace Teamy.Server.Services
{
    public interface IVoteHub
    {
        Task EventUpdated(Guid eventId);
        Task EventDeleted(Guid eventId);
    }

    public class VoteHub : Hub, IVoteHub
    {
        IHubContext<VoteHub> _hub;
        public VoteHub(IHubContext<VoteHub> ctx)
        {
            _hub = ctx;
        }

        public async Task EventUpdated(Guid eventId)
        {
            await _hub.Clients.All.SendAsync("EventUpdated", eventId);
        }

        public async Task EventDeleted(Guid eventId)
        {
            await _hub.Clients.All.SendAsync("EventDeleted", eventId);
        }
    }
}
