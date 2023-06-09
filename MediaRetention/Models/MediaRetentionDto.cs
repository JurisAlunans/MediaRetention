namespace MediaRetention.Models
{
    public class MediaRetentionDto
    {
        public int Id { get; set; }

        public int MediaId { get; set; }

        public string? Username { get; set; }

        public required string FileName { get; set; }

        public required string DirectoryPath { get; set; }

        public DateTime Created { get; set; }
    }
}
