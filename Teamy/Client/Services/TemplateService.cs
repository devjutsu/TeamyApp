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
        Task<EventVM> Get(Guid id);
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
            Http = AppState.IsLoggedIn ? http : httpClientFactory.CreateClient("public");
            Nav = nav;
        }

        public async Task<List<EventVM>> Recommended()
            => await Http.GetFromJsonAsync<List<EventVM>>(Nav.BaseUri.ToString() + "Templates");

        public async Task<EventVM> Get(Guid id)
           => await Http.GetFromJsonAsync<EventVM>(Nav.BaseUri.ToString() + $"Templates/Get/{id}");
    }
}
