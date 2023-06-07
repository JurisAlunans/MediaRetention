using MediaRetention.NotificationHandlers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace MediaRetention.Composers
{
    public class StartupComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<MediaRetentionFilter>();

            builder.ContentApps().Append<MediaRetentionContentApp>();

            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunMediaRetentionMigration>();
        }
    }
}
