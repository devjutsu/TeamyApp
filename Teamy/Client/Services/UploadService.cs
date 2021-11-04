namespace Teamy.Client.Services
{
    public interface IManageUploads
    {
        Task<string> AddImage(MultipartFormDataContent image);
    }

    public class UploadService : IManageUploads
    {
        HttpClient AnonymousHttp;
        HttpClient Http;
        AppState AppState;

        public UploadService(IHttpClientFactory httpClientFactory, HttpClient http, AppState appState)
        {
            AnonymousHttp = httpClientFactory.CreateClient("Teamy1.AnonymousAPI");
            Http = http;
            AppState = appState;
        }
        public async Task<string> AddImage(MultipartFormDataContent image)
        {
            var postResult = await Http.PostAsync($"Upload/AddImage/", image);
            return await postResult.Content.ReadAsStringAsync();
        }
    }
}
