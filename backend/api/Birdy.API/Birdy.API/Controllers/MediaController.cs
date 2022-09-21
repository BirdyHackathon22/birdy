using Birdy.API.Models;
using Birdy.API.Services;
using Birdy.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Birdy.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IAzureBlobStorageService azureBlobStorageService;
        private readonly ILogger<MediaController> _logger;

        public MediaController(ILogger<MediaController> logger, IAzureBlobStorageService azureBlobStorageService)
        {
            _logger = logger;
            this.azureBlobStorageService = azureBlobStorageService;
        }

        [HttpGet("{filename}", Name = "GetMedia")]
        public async Task<FileContentResult> Get(string filename)
        {
            var (fileData, mimeType) = await azureBlobStorageService.Retrieve(filename);

            if (fileData != null)
                return File(fileData, "image/jpeg");

            byte[] fileContents = null;

            using (var client = new HttpClient())
            {
                fileContents = await client.GetByteArrayAsync(BirdSpecies.Images[filename]);
            }

            string contentType = "image/jpeg";

            return File(fileContents, contentType);
        }

        [HttpPost(Name = "UploadMedia")]
        public async Task<ActionResult<string>> Upload(IFormFile fileData)
        {
            var fileName = GetFileName(fileData.FileName);
            await azureBlobStorageService.Upload(fileName, fileData.ContentType, ReadFully(fileData.OpenReadStream()));
            return fileName;
        }

        private string GetFileName(string fileName)
        {
            return fileName; //In the future we will have some logic    
        }

        private byte[] ReadFully(Stream input)
        {
            using MemoryStream ms = new ();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
