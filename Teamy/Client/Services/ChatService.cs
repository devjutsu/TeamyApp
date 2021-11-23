using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManageChats
    {
        Task<List<ChatMessageVM>> Get(Guid eventId);
        Task<bool> Post(ChatMessageVM message);
    }

    public class ChatService : IManageChats
    {
        HttpClient Http;
        AppState AppState;
        NavigationManager Nav { get; set; }

        public ChatService(HttpClient http,
                            AppState appState,
                            NavigationManager nav)
        {
            AppState = appState;
            Http = http;
            Nav = nav;
        }

        public async Task<List<ChatMessageVM>> Get(Guid eventId)
            => await Http.GetFromJsonAsync<List<ChatMessageVM>>(Nav.BaseUri.ToString() + $"Chat/Get/{eventId}");

        public async Task<bool> Post(ChatMessageVM message)
        {
            await Http.PostAsJsonAsync<ChatMessageVM>(Nav.BaseUri.ToString() + $"Chat/Post", message);
            return true;
        }
    }
}
