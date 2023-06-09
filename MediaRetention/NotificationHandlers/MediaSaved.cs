using MediaRetention.Services;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Security;

namespace MediaRetention.NotificationHandlers
{
    public class MediaSaved : INotificationHandler<MediaSavedNotification>
    {
        private readonly ILogger<MediaSaved> _logger;
        private readonly MediaRetentionService _mediaRetentionService;
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

        public MediaSaved(ILogger<MediaSaved> logger,
            MediaRetentionService mediaRetentionService,
            IBackOfficeSecurityAccessor backOfficeSecurityAccessor)
        {
            _logger = logger;
            _mediaRetentionService = mediaRetentionService;
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        }

        public void Handle(MediaSavedNotification notification)
        {
            foreach (var mediaItem in notification.SavedEntities)
            {
                if (mediaItem.HasProperty(Umbraco.Cms.Core.Constants.Conventions.Media.File))
                {
                    _logger.LogDebug("MediaRetention - Saving copy of {name}, (id - {id})", mediaItem.Name, mediaItem.Id);

                    _mediaRetentionService.Save(mediaItem, _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser?.Id);
                }
            }
        }
    }
}
