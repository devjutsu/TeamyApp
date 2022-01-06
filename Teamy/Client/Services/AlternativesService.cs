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
    public interface IManageAlternatives
    {
        Task<ProposedDateVM> RecommendDate(ProposedDateVM date);
        Task<ProposedDateVM> UpdateRecommendedDate(ProposedDateVM date);
        Task<bool> DeleteRecommendedDate(ProposedDateVM date);
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

        public async Task<ProposedDateVM> RecommendDate(ProposedDateVM date)
        {
            var uri = Nav.BaseUri.ToString() + $"Alternatives/RecommendDate";
            var response = await Http.PostAsJsonAsync<ProposedDateVM>(uri, date);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProposedDateVM>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<ProposedDateVM> UpdateRecommendedDate(ProposedDateVM date)
        {
            var uri = Nav.BaseUri.ToString() + $"Alternatives/UpdateRecommendedDate";
            var response = await Http.PostAsJsonAsync<ProposedDateVM>(uri, date);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProposedDateVM>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<bool> DeleteRecommendedDate(ProposedDateVM date)
        {
            var uri = Nav.BaseUri.ToString() + $"Alternatives/DeleteRecommendedDate";
            var response = await Http.PostAsJsonAsync<ProposedDateVM>(uri, date);
            return response.IsSuccessStatusCode;
        }
    }
}
