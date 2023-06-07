using MediaRetention.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Scoping;

namespace MediaRetention.Services
{
    internal class MediaRetentionService
    {
        private readonly IScopeProvider _scopeProvider;


        public MediaRetentionService(IScopeProvider scopeProvider) 
        {
            _scopeProvider = scopeProvider;
        }

        //Download
        //Restore
        //Save MediaRetention

        //Get all
        public List<MediaRetentionDto>? GetAll(int mediaId)
        {
            using var scope = _scopeProvider.CreateScope();
            var queryResults = scope.Database.Fetch<MediaRetentionDto>
                ("SELECT mr.Id, mr.MediaId, mr.FileId, mr.Guid, mr.Created, mr.FileName, uu.username" +
                " mr. FROM [MediaRetention] LEFT JOIN [umbracoUser] uu mr.UserId = uu.UserId WHERE MediaId = @0", mediaId);
            scope.Complete();
            return queryResults;
        }


        //Delete
        public async Task<bool> DeleteAsync(int id)
        {
            using var scope = _scopeProvider.CreateScope();
            var result = await scope.Database.DeleteAsync(id);
            scope.Complete();

            return result == 1;
        }



    }
}
