using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Teamy.Shared.ViewModels;
using Teamy.Shared.Common;

namespace Teamy.Client.Services
{
    public class AppState
    {
        public ClaimsPrincipal User { get; private set; }
        public string UserId { get; private set; }
        public string UserDisplayName { get; private set; }
        public bool IsLoggedIn => User?.Identity.IsAuthenticated ?? false;
        public List<EventVM> StoredEvents { get; private set; }
        public List<EventVM>? UpcomingEvents => StoredEvents?
                                            .Where(e => 
                                            (e.CreatedById == UserId || e.Participants.Any(o => o.UserId == UserId && (o.Status == ParticipationStatus.Creator || o.Status == ParticipationStatus.Accept)))
                                            && (e.DateStatus != EventDateStatus.Locked || e.EventDate > DateTime.Now))
                                            .ToList();
        public List<EventVM>? PastEvents => StoredEvents?
                                            .Where(e => e.DateStatus == EventDateStatus.Locked && e.EventDate < DateTime.Now)
                                            .OrderByDescending(o => o.EventDate)
                                            .ToList();

        public List<EventVM>? NoGoEvents => StoredEvents?
                                            .Where(e => e.Participants.Any(o => o.UserId == UserId && o.Status == ParticipationStatus.Reject))
                                            .ToList();
        public ParticipationVM? LatestParticipation { get; private set; }
        public EventVM Invite { get; set; }

        public AppState(AuthenticationStateProvider authStateProvider)
        {
            User = authStateProvider.GetAuthenticationStateAsync().Result.User;
            UserId = User?.Claims?.SingleOrDefault(o => o.Type == "sub")?.Value;
            UserDisplayName = User?.Claims?.SingleOrDefault(o => o.Type == "DisplayName")?.Value ?? User?.Identity?.Name;
        }

        public void UpdateEvents(ComponentBase source, List<EventVM> events)
        {
            this.StoredEvents = events;
            NotifyStateChanged(source, "Events");
        }

        public void SetParticipation(ComponentBase source, ParticipationVM participation)
        {
            this.LatestParticipation = participation;
            NotifyStateChanged(source, "LatestParticipation");
        }

        public void SetInvite(ComponentBase source, EventVM invitedEvent)
        {
            Invite = invitedEvent;
        }


        public event Action<ComponentBase, string> Statechanged;
        private void NotifyStateChanged(ComponentBase source, string Property) =>
            Statechanged?.Invoke(source, Property);
    }
}
