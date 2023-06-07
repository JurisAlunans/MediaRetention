using MediaRetention.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace MediaRetention.Controllers
{
    [PluginController(Constants.PluginName)]
    internal class MediaRetentionApiController : UmbracoAuthorizedApiController
    {
        private readonly MediaRetentionService _mediaRetentionService;

        public MediaRetentionApiController(MediaRetentionService mediaRetentionService)
        {
            _mediaRetentionService = mediaRetentionService;
        }

        public IActionResult GetAll(int mediaId)
        {
            return Ok(_mediaRetentionService.GetAll(mediaId));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id) 
        {
            return Ok(await _mediaRetentionService.DeleteAsync(id));
        }
    }
}
