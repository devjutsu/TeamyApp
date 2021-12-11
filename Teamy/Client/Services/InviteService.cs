using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;
using Teamy.Shared.Common;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Microsoft.JSInterop;

namespace Teamy.Client.Services
{
    public interface IManageInvites
    {
        Task<ParticipationVM> Respond(Guid eventId, bool accept, string participantName, string inviteCode);
        Task<ParticipationVM> RespondAnon(Guid eventId, bool accept, string participantName, string inviteCode);
    }

    public class InviteService : IManageInvites
    {
        HttpClient Http { get; set; }
        AppState AppState { get; set; }
        NavigationManager Nav { get; set; }
        IJSRuntime JS { get; set; }

        public InviteService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState, NavigationManager nav, IJSRuntime js)
        {
            AppState = appState;
            Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("public");
            Nav = nav;
            JS = js;
        }

        public async Task<ParticipationVM> Respond(Guid eventId, bool accept, string participantName, string inviteCode)
        {
            var participation = new ParticipationVM
            {
                EventId = eventId,
                Status = accept ? ParticipationStatus.Accept : ParticipationStatus.Reject,
                Name = participantName,
                InviteCode = inviteCode,
                UserId = AppState.UserId,
            };
            var url = $"{Nav.BaseUri}Invites/Respond";
            var result = await Http.PostAsJsonAsync<ParticipationVM>(url, participation);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var id = JsonSerializer.Deserialize<Guid>(content);
                participation.Id = id;
            }
            AppState.SetInvite(null, null);
            await JS.InvokeAsync<object>("WriteCookie.WriteCookie", "participationEventId", participation.EventId, DateTime.Now.AddMinutes(600));
            return participation;
        }

        public async Task<ParticipationVM> RespondAnon(Guid eventId, bool accept, string participantName, string inviteCode)
        {
            var participation = new ParticipationVM
            {
                EventId = eventId,
                Status = accept ? ParticipationStatus.Accept : ParticipationStatus.Reject,
                Name = participantName,
                InviteCode = inviteCode
            };
            var url = $"{Nav.BaseUri}Invites/RespondAnon";
            var result = await Http.PostAsJsonAsync<ParticipationVM>(url, participation);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var id = JsonSerializer.Deserialize<Guid>(content);
                participation.Id = id;
            }
            await JS.InvokeAsync<object>("WriteCookie.WriteCookie", "participationEventId", participation.EventId, DateTime.Now.AddMinutes(600));
            return participation;
        }
    }
}
