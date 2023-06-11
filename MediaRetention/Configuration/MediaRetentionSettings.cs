namespace MediaRetention.Configuration
{
    public class MediaRetentionSettings
    {
        public string BackupRootDirectory { get; set; } = "/umbraco/mediaRetention";

        public int BackupFileLimit { get; set; } = 5;
    }
}
