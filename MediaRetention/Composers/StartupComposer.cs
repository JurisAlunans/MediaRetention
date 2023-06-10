using MediaRetention.Configuration;
using MediaRetention.NotificationHandlers;
using MediaRetention.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace MediaRetention.Composers
{
    public class StartupComposer : IComposer
    {
        public StartupComposer()
        {
        }

        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<MediaRetentionFilter>();

            builder.Services.AddTransient<MediaRetentionService>();

            builder.ContentApps().Append<MediaRetentionContentApp>();

            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunMediaRetentionMigration>();
            builder.AddNotificationHandler<ServerVariablesParsingNotification, ServerVariables>();
            builder.AddNotificationHandler<MediaSavedNotification, MediaSaved>();

            builder.Services.Configure<MediaRetentionSettings>(builder.Config.GetSection(Constants.PluginName));
        }
    }
}
