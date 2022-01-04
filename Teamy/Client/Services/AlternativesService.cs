using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManageAlternatives
    {
        Task<bool> RecommendDate(ProposedDateVM date);
    }

    public class AlternativesSerice : IManageAlternatives
    {
        HttpClient Http;
        AppState AppState;
        NavigationManager Nav { get; set; }

        public AlternativesSerice(HttpClient http,
                            AppState appState,
                            NavigationManager nav)
        {
            AppState = appState;
            Http = http;
            Nav = nav;
        }

        public async Task<bool> RecommendDate(ProposedDateVM date)
        {
            var result = await Http.PostAsJsonAsync<ProposedDateVM>(Nav.BaseUri.ToString() + $"Alternatives/RecommendDate", date);
            return result.IsSuccessStatusCode;
        }
    }
}
