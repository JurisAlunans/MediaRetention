using MediaRetention.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace MediaRetention.Controllers
{
    [PluginController(Constants.PluginName)]
    public class MediaRetentionApiController : UmbracoAuthorizedApiController
    {
        private readonly MediaRetentionService _mediaRetentionService;

        public MediaRetentionApiController(MediaRetentionService mediaRetentionService)
        {
            _mediaRetentionService = mediaRetentionService;
        }

        [HttpGet]
        public object GetAll([FromQuery]int mediaId)
        {
            return Ok(_mediaRetentionService.GetAll(mediaId));
        }

        [HttpGet]
        public IActionResult Download(int id)
        {
            var file = _mediaRetentionService.GetMediaRetentionById(id);

            if (file == null) return NotFound();

            var filePath = _mediaRetentionService.MapFilePath($"{file.DirectoryPath}/{file.FileName}");

            if (!System.IO.File.Exists(filePath))
            {
              
               return ValidationProblem("No file found for path " + filePath);
                
            }

            // Set custom header so umbRequestHelper.downloadFile can save the correct filename
            Response.Headers.Add("x-filename", WebUtility.UrlEncode(file.FileName));

            return File(System.IO.File.OpenRead(filePath), MediaTypeNames.Application.Octet, file.FileName);
        }

        [HttpPost]
        public IActionResult Restore(int id)
        {
            return Ok(_mediaRetentionService.Restore(id));
        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            return Ok(_mediaRetentionService.DeleteById(id));
        }
    }
}
