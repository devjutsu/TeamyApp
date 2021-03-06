using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManagePolls
    {
        PollVM NewPoll();
        Task<bool> Vote(PollVM poll, PollChoiceVM choice);
        //Task ResetAnswers(PollVM poll);
        Task<bool> VoteDate(ProposedDateVM date);
        Task<bool> LockDate(ProposedDateVM date);
        Task<bool> UnlockDate(ProposedDateVM date);
    }

    public class PollService : IManagePolls
    {
        HttpClient Http { get; set; }
        AppState AppState { get; set; }

        public PollService(HttpClient http, AppState appState)
        {
            AppState = appState;
            Http = http;
        }
        public PollVM NewPoll()
        => new PollVM()
        {
            Choices = new List<PollChoiceVM>(),
            Question = String.Empty,
            MultiChoice = true,
            FreeTextOption = true
        };
        public async Task<bool> Vote(PollVM poll, PollChoiceVM choice)
        {
            var voteRequest = (PollChoiceVM)choice.Clone();
            voteRequest.Answers = null;

            var result = await Http.PostAsJsonAsync<PollChoiceVM>("Polls/Vote", voteRequest);
            return result.IsSuccessStatusCode;
        }

        //public async Task ResetAnswers(PollVM poll)
        //{
        //    var resetRequest = (PollVM)poll.Clone();
        //    resetRequest.Choices = new List<PollChoiceVM>();
        //    await Http.PostAsJsonAsync<PollVM>("Polls/Reset", resetRequest);
        //}

        public async Task<bool> VoteDate(ProposedDateVM date)
        {
            var result = await Http.PostAsJsonAsync<ProposedDateVM>("Polls/VoteDate", date);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> LockDate(ProposedDateVM date)
        {
            var result = await Http.PostAsJsonAsync<ProposedDateVM>("Polls/LockDate", date);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UnlockDate(ProposedDateVM date)
        {
            var result = await Http.PostAsJsonAsync<ProposedDateVM>("Polls/UnlockDate", date);
            return result.IsSuccessStatusCode;
        }
    }
}
