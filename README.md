
# Media Retention

Umbraco package for media file backups for Umbraco 10, 11, 12.
Currently supports physical file system and on-premise Umbraco.

## Installation

Add MediaRetention NuGet package to a project:

```
dotnet add package MediaRetention
```
    
## Usage

When installed, contentApp will be added to any media file that has default "umbracoFile" property. In contentApp it's possible to restore, download and delete backup file.

![ContentApp Screenshot](https://raw.github.com/JurisAlunans/MediaRetention/main/screenshots/contentApp.png)

## Configuration

```  
"MediaRetention": {
    "BackupFileLimit": 3,
    "BackupRootDirectory": "/umbraco/mediaRetention",
    "PathMode": "Relative"
}
```

By default it saves only 3 latest versions of media file.
PathMode can be also set to "Absolute" then you can use full path like "Z:/mediaRetention"


## Credits

Icon created by - https://css.gg
## License

[MIT](https://opensource.org/license/mit/)

