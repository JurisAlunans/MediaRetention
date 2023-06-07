using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace MediaRetention.NotificationHandlers
{
    public class MediaSaved : INotificationHandler<MediaSavedNotification>
    {
        private readonly ILogger<MediaSaved> _logger;

        public MediaSaved(ILogger<MediaSaved> logger)
        {
            _logger = logger;
        }

        public void Handle(MediaSavedNotification notification)
        {
            foreach (var mediaItem in notification.SavedEntities)
            {
                if (mediaItem.HasProperty(Umbraco.Cms.Core.Constants.Conventions.Media.File))
                {
                    _logger.LogDebug("MediaRetention - Saving copy of {name}, (id - {id})", mediaItem.Name, mediaItem.Id);
                }
            }
        }
    }
}
