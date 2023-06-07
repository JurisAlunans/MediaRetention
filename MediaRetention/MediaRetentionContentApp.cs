using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace MediaRetention
{
    public class MediaRetentionContentApp : IContentAppFactory
    {
        public ContentApp? GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
       
            if (source is IMedia media && media.HasProperty(Umbraco.Cms.Core.Constants.Conventions.Media.File))
            {
                return new ContentApp
                {
                    Alias = "mediaRetention",
                    Name = "Media Retention",
                    Icon = "icon-layers-alt",
                    View = "/App_Plugins/MediaRetention/mediaRetention.html",
                    Weight = 999
                };
            }

            return null;
        }
    }
}
