using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManageEvents
    {
        Task LoadUpcoming();
        Task<EventVM> Get(Guid id);
    }

    public class EventService : IManageEvents
    {
        HttpClient AnonymousHttp;
        HttpClient Http;
        AppState AppState;

        public EventService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState)
        {
            AnonymousHttp = httpClientFactory.CreateClient("Teamy1.AnonymousAPI");
            Http = http;
            AppState = appState;
        }

        public async Task<EventVM> Get(Guid id)
        {
            return await Http.GetFromJsonAsync<EventVM>($"Events/Get/{id}");
        }

        public async Task LoadUpcoming()
        {
            if (AppState.IsLoggedIn)
            {
                var events = await Http.GetFromJsonAsync<List<EventVM>>("Events/Upcoming/9");
                AppState.UpdateEvents(null, events);
            }
        }
    }
}
