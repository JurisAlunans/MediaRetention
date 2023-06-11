using MediaRetention.Configuration;
using MediaRetention.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NPoco.Expressions;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;
using static MediaRetention.Migrations.AddMediaRetentionTable;
using static Umbraco.Cms.Core.Constants;

namespace MediaRetention.Services
{
    public class MediaRetentionService
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly ILogger<MediaRetentionService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MediaUrlGeneratorCollection _mediaUrlGeneratorCollection;
        private readonly MediaFileManager _mediaFileManager;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
        private readonly IMediaService _mediaService;
        private readonly IOptions<MediaRetentionSettings> _mediaRetentionSettings;

        public MediaRetentionService(IScopeProvider scopeProvider,
            ILogger<MediaRetentionService> logger,
            IWebHostEnvironment webHostEnvironment,
            IMediaService mediaService,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
            IShortStringHelper shortStringHelper,
            MediaFileManager mediaFileManager,
            MediaUrlGeneratorCollection mediaUrlGeneratorCollection,
            IOptions<MediaRetentionSettings> mediaRetentionSettings)
        {
            _scopeProvider = scopeProvider;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _mediaService = mediaService;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
            _shortStringHelper = shortStringHelper;
            _mediaFileManager = mediaFileManager;
            _mediaUrlGeneratorCollection = mediaUrlGeneratorCollection;
            _mediaRetentionSettings = mediaRetentionSettings;
        }
       
        public MediaRetentionDto? GetMediaRetentionById(int id)
        {
            using var scope = _scopeProvider.CreateScope();
            var result = scope.Database.Fetch<MediaRetentionDto>
                ("SELECT mr.Id, mr.MediaId, mr.DirectoryPath, mr.Created, mr.FileName, uu.username" +
                " FROM [MediaRetention] mr LEFT JOIN [umbracoUser] uu on mr.UserId = uu.id WHERE mr.Id = @0", id);

            return result.MaxBy(x => x.Created);
        }

        public void Save(IMedia media, int? userId)
        {          
            media.TryGetMediaPath(Conventions.Media.File, _mediaUrlGeneratorCollection, out string? mediaFilePath);
            
            if (string.IsNullOrEmpty(mediaFilePath))
            {
                _logger.LogWarning("Media Retention - file property is empty, mediaId - @0", media.Id);

                return;
            } 

            var filePath = _webHostEnvironment.MapPathWebRoot(mediaFilePath);

            if (System.IO.File.Exists(filePath))
            {
                string backupRelativeDirectoryPath = $"{_mediaRetentionSettings.Value.BackupRootDirectory}/{media.Id}/{Guid.NewGuid()}";

                string backupAbsoluteDirectoryPath = _webHostEnvironment.MapPathContentRoot(backupRelativeDirectoryPath);

                Directory.CreateDirectory(backupAbsoluteDirectoryPath);

                string fileName = Path.GetFileName(filePath);

                System.IO.File.Copy(filePath, $"{backupAbsoluteDirectoryPath}/{fileName}", true);

                DeleteBackupsOverLimit(media.Id);

                using var scope = _scopeProvider.CreateScope();
                scope.Database.Insert(new MediaRetentionSchema(media.Id, userId, fileName, backupRelativeDirectoryPath));

                scope.Complete();
            }
        }

        public bool Restore(int id)
        {
            var file = GetMediaRetentionById(id);

            if (file?.FileName != null)
            {
                var media = _mediaService.GetById(file.MediaId);

                if (media != null)
                {
                    using Stream stream = System.IO.File
                        .OpenRead(_webHostEnvironment.MapPathContentRoot($"{file.DirectoryPath}/{file.FileName}"));

                    media.SetValue(_mediaFileManager, _mediaUrlGeneratorCollection, _shortStringHelper,
                        _contentTypeBaseServiceProvider, Conventions.Media.File, file.FileName, stream);

                    var result = _mediaService.Save(media);

                    return result.Success;
                }
            }

            return false;
        }

        public bool DeleteById(int id)
        {
            var file = GetMediaRetentionById(id);

            if (file?.DirectoryPath != null)
            {
                using var scope = _scopeProvider.CreateScope();
                var result = scope.Database.Delete<MediaRetentionSchema>("WHERE [Id] = @0", id);
                scope.Complete();
                DeleteDirectory(file.DirectoryPath);

                return result == 1;
            }

            return false;
        }

        public void DeleteByMediaId(int mediaId)
        {
            var directoryPath = _webHostEnvironment.MapPathContentRoot($"{_mediaRetentionSettings.Value.BackupRootDirectory}/{mediaId}");

            DeleteDirectory(directoryPath);

            using var scope = _scopeProvider.CreateScope();
            var result = scope.Database.Delete<MediaRetentionSchema>("WHERE [MediaId] = @0", mediaId);
            scope.Complete();
            if (result > 0)
            {
                _logger.LogInformation("Deleted media retention {count} file(s), mediaId - {id}", result, mediaId);
            }
        }

        public List<MediaRetentionDto>? GetAll(int mediaId)
        {
            using var scope = _scopeProvider.CreateScope();
            var queryResults = scope.Database.Fetch<MediaRetentionDto>
                ("SELECT mr.Id, mr.MediaId, mr.DirectoryPath, mr.Created, mr.FileName, uu.username" +
                " FROM [MediaRetention] mr LEFT JOIN [umbracoUser] uu on mr.UserId = uu.id WHERE MediaId = @0 ORDER BY Created DESC", mediaId);
            scope.Complete();
            return queryResults;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var scope = _scopeProvider.CreateScope();
            var result = await scope.Database.DeleteAsync(id);
            scope.Complete();

            return result == 1;
        }

        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        private void DeleteBackupsOverLimit(int mediaId)
        {
            using var scope = _scopeProvider.CreateScope();

            var result = scope.Database.Fetch<MediaRetentionSchema>("WHERE [MediaId] = @0 ORDER BY Created desc",mediaId);

            if (result != null && result.Any() && result.Count >= _mediaRetentionSettings.Value.BackupFileLimit)
            {
                result = result.Skip(_mediaRetentionSettings.Value.BackupFileLimit - 1).ToList();

                foreach(var file in result)
                {
                    DeleteDirectory(file.DirectoryPath);
                }

                scope.Database.DeleteMany<MediaRetentionSchema>()
                    .Where(x => x.Id.In(result.Select(y => y.Id))).Execute();

                _logger.LogInformation("Deleted media retention {count} files over limit, mediaId - {id}", result.Count, mediaId);
            }

            scope.Complete();
        }
    }
}
