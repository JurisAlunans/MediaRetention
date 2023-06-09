using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaRetention.Configuration
{
    public class MediaRetentionSettings
    {
        public string BackupRootDirectory { get; set; } = "/umbraco/mediaRetention";

        public int BackupFileLimit { get; set; } = 5;
    }
}
