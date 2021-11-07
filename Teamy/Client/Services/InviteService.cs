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
        Task Respond(Guid eventId, bool accept);
    }

    public class InviteService : IManageInvites
    {
        HttpClient Http { get; set; }
        AppState AppState { get; set; }
        NavigationManager Nav { get; set; }

        public InviteService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState, NavigationManager nav)
        {
            AppState = appState;
            Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("Teamy.AnonymousAPI");
            Nav = nav;
        }

        public async Task Respond(Guid eventId, bool accept)
        {
            var participation = new ParticipationVM
            {
                EventId = eventId,
                Status = accept ? ParticipationStatus.Accept : ParticipationStatus.Reject,
            };
            var url = $"{Nav.BaseUri}Invites/Respond";
            await Http.PostAsJsonAsync<ParticipationVM>(url, participation);
        }
    }
}
