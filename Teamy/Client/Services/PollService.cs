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
    }

    public class PollService : IManagePolls
    {
        HttpClient Http { get; set; }
        AppState AppState { get; set; }

        public PollService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState)
        {
            AppState = appState;
            Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("Teamy.AnonymousAPI");
        }

        public PollVM NewPoll()
            => new PollVM()
            {
                Choices = new List<PollChoiceVM>()
                {
                    new PollChoiceVM()
                    {
                        Choice = "Type poll choice text",
                        Answers = new List<PollAnswerVM>()
                    }
                },
                Question = "Type Question",
                MultiChoice = true,
                FreeTextOption = true
            };

        public async Task<bool> Vote(PollVM poll, PollChoiceVM choice)
        {
            choice.PollId = poll.Id;
            // @! choice.Answers = null;
            var result = await Http.PostAsJsonAsync<PollChoiceVM>("Polls/Vote", choice);
            return result.IsSuccessStatusCode;
        }
    }
}
