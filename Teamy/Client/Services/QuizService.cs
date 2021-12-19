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
    public interface IManageQuiz
    {
        Task<QuizVM> Get(QuizCodeVM request);
        Task<bool> Post(QuizAnswerVM answer);
        Task<bool> Submit(string qCode, string userId);
        Task<List<QuizVM>> List();
    }

    public class QuizService : IManageQuiz
    {
        HttpClient Http;
        AppState AppState;
        NavigationManager Nav { get; set; }

        public QuizService(IHttpClientFactory httpClientFactory,
                            HttpClient http,
                            AppState appState,
                            NavigationManager nav)
        {
            AppState = appState;
            Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("public");
            Nav = nav;
        }

        public async Task<QuizVM> Get(QuizCodeVM request)
        {
            var uri = Nav.BaseUri + $"Quiz/Get";
            var result = await Http.PostAsJsonAsync(uri, request);
            var content = await result.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<QuizVM>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<bool> Post(QuizAnswerVM answer)
        {
            var result = await Http.PostAsJsonAsync<QuizAnswerVM>("Quiz/PostAnswer", answer);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> Submit(string qCode, string userId)
        {
            Console.WriteLine($"Submitting: {qCode}, {userId}");
            var request = new QuizCodeVM()
            {
                QCode = qCode,
                UserId = userId
            };
            var result = await Http.PostAsJsonAsync<QuizCodeVM>($"Quiz/Submit", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<List<QuizVM>> List()
        {
            return await Http.GetFromJsonAsync<List<QuizVM>>(Nav.BaseUri.ToString() + "Quiz/List");
        }
    }
}
