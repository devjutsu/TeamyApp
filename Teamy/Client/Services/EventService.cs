using System.Net.Http.Json;
using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManageEvents
    {
        Task LoadUpcoming();
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
