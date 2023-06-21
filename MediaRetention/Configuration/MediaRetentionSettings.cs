namespace MediaRetention.Configuration
{
    public class MediaRetentionSettings
    {
        public string BackupRootDirectory { get; set; } = "/umbraco/mediaRetention";
        public string PathMode { get; set; } = PathModes.Relative;

        public int BackupFileLimit { get; set; } = 5;
    }
}
