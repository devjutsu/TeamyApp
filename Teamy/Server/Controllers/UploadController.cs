using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Models;
using Teamy.Server.Services;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        IMapper _mapper;
        IStorageService _storage { get; set; }
        public UploadController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    IMapper mapper,
                                    IStorageService storage)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _storage = storage;
        }

        [AllowAnonymous]
        [HttpPost("AddImage")]
        public async Task<string> AddImage()
        {
            var file = Request.Form.Files[0];
            var filename = Guid.NewGuid().ToString().Substring(0, 13);
            var ext = System.IO.Path.GetExtension(file.FileName);

            var imgUri = await _storage.Upload(file.OpenReadStream(), filename + ext);

            return imgUri.AbsoluteUri;
        }
    }
}
