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
        Task<string> Create(EventVM eventVM);
    }

    public class EventService : IManageEvents
    {
        HttpClient Http { get; set; }
        AppState AppState { get; set; }

        public EventService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState)
        {
            AppState = appState;
            //Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("Teamy.AnonymousAPI");
            Http = http;
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

        public async Task<string> Create(EventVM eventVM)
        {
            //    Title = "sampl test",
            //    Description = "blah blah",
            //    ImageUrl = "",
            //    When = DateTime.Now,
            //    Where = "",

            var result = await Http.PostAsJsonAsync<EventVM>("Events/Create", eventVM);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadAsStringAsync();
            else return string.Empty;
        }
    }
}
