using Teamy.Server.Data;

namespace Teamy.Server.Services
{
    public interface IUploadImages
    {
        Task<Guid> UploadImageAsync(IFormFile image);
    }

    public class DbImgUpload : IUploadImages
    {
        private ILogger<DbImgUpload> _logger;
        private TeamyDbContext _db;
        private IStorageService _storage;

        public DbImgUpload(ILogger<DbImgUpload> logger,
                                    TeamyDbContext db,
                                    IStorageService storage)
        {
            _logger = logger;
            _db = db;
            _storage = storage;
        }

        public async Task<Guid> UploadImageAsync(IFormFile image)
        {
            var filename = Guid.NewGuid().ToString().Substring(0, 13);
            var ext = Path.GetExtension(image.FileName);
            var imageUrl = await _storage.Upload(image.OpenReadStream(), filename + ext);
            var dbAddedModel = await _db.Images.AddAsync(new Models.ImageModel { Url = imageUrl.AbsoluteUri });

            await _db.SaveChangesAsync();
            return dbAddedModel.Entity.Id;
        }
    }
}
