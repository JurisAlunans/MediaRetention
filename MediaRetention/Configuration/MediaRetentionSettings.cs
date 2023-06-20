namespace MediaRetention.Configuration
{
    public class MediaRetentionSettings
    {
        public string BackupRootDirectory { get; set; } = "/umbraco/mediaRetention";
        public string FileMode { get; set; } = FileModes.Relative;

        public int BackupFileLimit { get; set; } = 5;
    }
}
