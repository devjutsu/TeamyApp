namespace Teamy.Client.Services
{
    public interface IManageUploads
    {
        Task<string> AddImage(MultipartFormDataContent image);
    }

    public class UploadService : IManageUploads
    {
        HttpClient Http;
        AppState AppState;

        public UploadService(HttpClient http, AppState appState)
        {
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
