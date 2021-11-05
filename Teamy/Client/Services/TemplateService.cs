using Microsoft.AspNetCore.Components;
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
    }

    public class TemplateService : IManageTemplates
    {
        HttpClient Http;
        AppState AppState;
        NavigationManager Nav { get; set; }

        public TemplateService(IHttpClientFactory httpClientFactory, 
                            HttpClient http, 
                            AppState appState, 
                            NavigationManager nav)
        {
            AppState = appState;
            Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("Teamy1.AnonymousAPI");
            Nav = nav;
        }

        public async Task<List<EventVM>> Recommended()
            => await Http.GetFromJsonAsync<List<EventVM>>(Nav.BaseUri.ToString() + "Templates");
    }
}
