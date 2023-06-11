# Media Retention

Umbraco package for media file backups for Umbraco 10, 11.
Currently supports physical file system and on-premise Umbraco.

## Usage

When installed, contentApp will be added to any media file that has default "umbracoFile" property. In contentApp it's possible to restore, download and delete backup file.

![ContentApp Screenshot](https://raw.github.com/JurisAlunans/MediaRetention/main/screenshots/contentApp.png)

## Configuration

```  
"MediaRetention": {
    "BackupFileLimit": 3,
    "BackupRootDirectory": "/umbraco/mediaRetention"
}
```