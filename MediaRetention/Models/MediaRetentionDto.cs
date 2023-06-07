namespace MediaRetention.Models
{
    internal class MediaRetentionDto
    {
        public int Id { get; set; }

        public int MediaId { get; set; }

        public string? Username { get; set; }

        public required string FileName { get; set; }

        public Guid Guid { get; set; }

        public DateTime Created { get; set; }
    }
}
