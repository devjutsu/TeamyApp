using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManageEvents
    {
        Task LoadUpcoming();
        Task<EventVM> Get(Guid id);
        Task<string> Create(EventVM eventVM);
        Task<string> Update(EventVM eventVM);
        Task<EventVM> Invited(string inviteCode);
    }

    public class EventService : IManageEvents
    {
        HttpClient Http { get; set; }
        AppState AppState { get; set; }
        NavigationManager Nav { get; set; }
        public EventService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState, NavigationManager nav)
        {
            AppState = appState;
            Http = appState.IsLoggedIn ? http : httpClientFactory.CreateClient("public");
            Nav = nav;
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
            eventVM.CleanEmptyPollChoices();

            var result = await Http.PostAsJsonAsync<EventVM>("Events/Create", eventVM);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadAsStringAsync();
            else return string.Empty;
        }

        public async Task<string> Update(EventVM eventVM)
        {
            eventVM.CleanEmptyPollChoices();
            eventVM.Participants = null;

            var result = await Http.PostAsJsonAsync<EventVM>("Events/Update", eventVM);
            
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadAsStringAsync();
            else return string.Empty;
        }

        public async Task<EventVM> Invited(string inviteCode)
        {
            var uri = Nav.BaseUri + $"Events/AnonInvited";

            var result = await Http.PostAsJsonAsync(uri, inviteCode);
            var content = await result.Content.ReadAsStringAsync();

            var evt = JsonSerializer.Deserialize<EventVM>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return evt;
        }
    }
}
