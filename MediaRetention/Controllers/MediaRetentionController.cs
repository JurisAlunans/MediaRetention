using MediaRetention.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Filters;

namespace MediaRetention.Controllers
{
    [PluginController(Constants.PluginName)]
    public class MediaRetentionApiController : UmbracoAuthorizedApiController
    {
        private readonly MediaRetentionService _mediaRetentionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MediaRetentionApiController(MediaRetentionService mediaRetentionService,
            IWebHostEnvironment webHostEnvironment)
        {
            _mediaRetentionService = mediaRetentionService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAll(int mediaId)
        {
            return Ok(_mediaRetentionService.GetAll(mediaId));
        }

        [HttpGet]
        public IActionResult Download(int id)
        {
            var file = _mediaRetentionService.GetMediaRetentionById(id);

            if (file == null) return NotFound();

            var filePath = _webHostEnvironment.MapPathContentRoot($"{file.DirectoryPath}/{file.FileName}");

            if (!System.IO.File.Exists(filePath)) return NotFound();

            return new FileStreamResult(System.IO.File.OpenRead(filePath), MimeTypes.GetMimeType(file.FileName))
            {
                FileDownloadName = file.FileName
            };
        }

        [HttpPost]
        public IActionResult Restore(int id)
        {
            return Ok(_mediaRetentionService.GetMediaRetentionById(id));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id) 
        {
            return Ok(await _mediaRetentionService.DeleteAsync(id));
        }
    }
}
