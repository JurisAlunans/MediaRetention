using MediaRetention.Services;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Security;

namespace MediaRetention.NotificationHandlers
{
    public class MediaDeleted : INotificationHandler<MediaDeletedNotification>
    {
        private readonly ILogger<MediaDeleted> _logger;
        private readonly MediaRetentionService _mediaRetentionService;
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

        public MediaDeleted(ILogger<MediaDeleted> logger,
            MediaRetentionService mediaRetentionService,
            IBackOfficeSecurityAccessor backOfficeSecurityAccessor)
        {
            _logger = logger;
            _mediaRetentionService = mediaRetentionService;
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        }

        public void Handle(MediaDeletedNotification notification)
        {
            foreach (var mediaItem in notification.DeletedEntities)
            {
                if (mediaItem.HasProperty(Umbraco.Cms.Core.Constants.Conventions.Media.File))
                {
                    _logger.LogDebug("MediaRetention - Deleting backup files of {name}, (id - {id})", mediaItem.Name, mediaItem.Id);

                    _mediaRetentionService.DeleteByMediaId(mediaItem.Id);
                }
            }
        }
    }
}
