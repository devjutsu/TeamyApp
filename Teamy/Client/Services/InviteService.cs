using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;
using Teamy.Shared.Common;
using Microsoft.AspNetCore.Components;

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

        public InviteService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState, NavigationManager nav)
        {
            AppState = appState;
            Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("public");
            Nav = nav;
        }

        public async Task<ParticipationVM> Respond(Guid eventId, bool accept, string participantName, string inviteCode)
        {
            var participation = new ParticipationVM
            {
                EventId = eventId,
                Status = accept ? ParticipationStatus.Accept : ParticipationStatus.Reject,
                Name = participantName,
                InviteCode = inviteCode
            };
            var url = $"{Nav.BaseUri}Invites/Respond";
            await Http.PostAsJsonAsync<ParticipationVM>(url, participation);
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
            return participation;
        }
    }
}
