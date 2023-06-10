using Microsoft.Extensions.Logging;
using NPoco;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace MediaRetention
{
    public class AddMediaRetentionTable : MigrationBase
    {

        public AddMediaRetentionTable(IMigrationContext context) : base(context)
        {
        }
        protected override void Migrate()
        {
            Logger.LogDebug("Running migration {MigrationStep}", nameof(AddMediaRetentionTable));

            if (TableExists(Constants.TableName) == false)
            {
                Create.Table<MediaRetentionSchema>().Do();
            }
            else
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", Constants.TableName);
            }
        }

        [TableName(Constants.TableName)]
        [PrimaryKey("Id", AutoIncrement = true)]
        [ExplicitColumns]
        public class MediaRetentionSchema
        {
            public MediaRetentionSchema(int mediaId, int? userId, string fileName, string directoryPath)
            {
                MediaId = mediaId;
                UserId = userId;
                FileName = fileName;
                DirectoryPath = directoryPath;
                Created = DateTime.UtcNow;
            }

            [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
            [Column("Id")]
            public int Id { get; set; }

            [Column("MediaId")]
            public int MediaId { get; set; }

            [Column("UserId")]
            public int? UserId { get; set; }

            [Column("FileName")]
            public string FileName { get; set; }

            [Column("DirectoryPath")]
            [Length(500)]
            public string DirectoryPath { get; set; }

            [Column("Created")]
            public DateTime Created { get; set; }

        }
    }
}
