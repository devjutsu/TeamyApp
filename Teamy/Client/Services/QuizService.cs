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
        Task<List<QuizVM>> ManageList();
        Task<QuizVM> GenerateQCode(Guid quizId);
        Task<bool> UpdateQCodeInfo(QuizCodeVM request);
        Task<List<QuizQuestionVM>> GetAnswers(string qcode);
        Task<int> TotalAnswers(string qcode);
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
            var request = new QuizCodeVM()
            {
                Id = qCode,
                UserId = userId
            };
            var result = await Http.PostAsJsonAsync<QuizCodeVM>($"Quiz/Submit", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<List<QuizVM>> ManageList()
        {
            return await Http.GetFromJsonAsync<List<QuizVM>>(Nav.BaseUri.ToString() + "Quiz/ManageList");
        }

        public async Task<QuizVM> GenerateQCode(Guid quizId)
        {
            var request = new QuizIdVM() { Id = quizId };

            var uri = Nav.BaseUri + $"Quiz/GenerateQCode";
            var result = await Http.PostAsJsonAsync(uri, request);
            var content = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<QuizVM>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<bool> UpdateQCodeInfo(QuizCodeVM request)
        {
            var uri = Nav.BaseUri + $"Quiz/UpdateQCodeInfo";
            var result = await Http.PostAsJsonAsync(uri, request);
            return result.IsSuccessStatusCode;
       }

        public async Task<List<QuizQuestionVM>> GetAnswers(string qcode)
        {
            var request = new QCodeIdRequest() { Id = qcode };

            var uri = Nav.BaseUri + $"Quiz/GetAnswers";
            var result = await Http.PostAsJsonAsync(uri, request);
            var content = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<QuizQuestionVM>>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<int> TotalAnswers(string qcode)
        {
            var request = new QCodeIdRequest() { Id = qcode };

            var uri = Nav.BaseUri + $"Quiz/TotalAnswers";
            var result = await Http.PostAsJsonAsync(uri, request);
            var content = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}
