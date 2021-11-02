using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManageTemplates
    {
        Task<List<EventVM>> Recommended();
        Task<EventVM> Get(Guid id);
    }

    public class TemplateService : IManageTemplates
    {
        HttpClient AnonymousHttp;
        HttpClient Http;
        AppState AppState;

        public TemplateService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState)
        {
            AnonymousHttp = httpClientFactory.CreateClient("Teamy1.AnonymousAPI");
            Http = http;
            AppState = appState;
        }

        public async Task<List<EventVM>> Recommended()
            => await Http.GetFromJsonAsync<List<EventVM>>("Templates");

        public async Task<EventVM> Get(Guid id)
            => await Http.GetFromJsonAsync<EventVM>($"Templates/{id}");
    }
}
