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
        Task<QuizVM> Get(string qCode);
        Task<bool> Post(QuizAnswerVM answer);
        Task<bool> Submit(string qCode, string? userId = null);
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

        public async Task<QuizVM> Get(string qCode)
        {
            //var result = await Http.GetFromJsonAsync<QuizVM>(Nav.BaseUri.ToString() + $"Quiz/{qCode}");

            var uri = Nav.BaseUri + $"Quiz/Get";

            var result = await Http.PostAsJsonAsync(uri, qCode);
            var content = await result.Content.ReadAsStringAsync();

            var quiz = JsonSerializer.Deserialize<QuizVM>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return quiz;
        }

        public async Task<bool> Post(QuizAnswerVM answer)
        {
            var result = await Http.PostAsJsonAsync<QuizAnswerVM>("Quiz/PostAnswer", answer);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> Submit(string qCode, string? userId = null)
        {
            var result = await Http.PostAsJsonAsync<string>($"Quiz/Submit/{qCode}", userId ?? "");
            return result.IsSuccessStatusCode;
        }
    }
}
