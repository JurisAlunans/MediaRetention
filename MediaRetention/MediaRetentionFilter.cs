using Umbraco.Cms.Core.Manifest;

namespace MediaRetention
{
    internal class MediaRetentionFilter : IManifestFilter
    {
        public void Filter(List<PackageManifest> manifests)
        {
            manifests.Add(new PackageManifest
            {
                Version = "1.0",
                AllowPackageTelemetry = true,
                PackageName = Constants.PluginName,
                Scripts = new[]
                {
                    "/App_Plugins/MediaRetention/mediaRetention.controller.js",
                    "/App_Plugins/MediaRetention/mediaRetention.resources.js"
                }
            });
        }
    }
}
