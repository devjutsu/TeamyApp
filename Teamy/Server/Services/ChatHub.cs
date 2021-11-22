using Microsoft.AspNetCore.SignalR;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Services
{
    public interface IChatHub
    {
        Task SendMessage(ChatMessageVM message);
    }

    public class ChatHub : Hub, IChatHub
    {
        IHubContext<ChatHub> _hub;
        public ChatHub(IHubContext<ChatHub> ctx)
        {
            _hub = ctx;
        }

        public async Task SendMessage(ChatMessageVM message)
        {
            await _hub.Clients.All.SendAsync("ChatUpdated", message);
        }
    }
}

