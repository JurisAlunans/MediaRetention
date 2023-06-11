
# Media Retention

Umbraco package for media file backups for Umbraco 10, 11.
Currently supports physical file system and on-premise Umbraco.


## Installation

Add MediaRetention NuGet package to a project:

```
dotnet add package MediaRetention
```
    
## Usage

When installed, contentApp will be added to any media file that has default "umbracoFile" property. In contentApp it's possible to restore, download and delete backup file.

![ContentApp Screenshot](/screenshots/contentApp.png?raw=true)

## Configuration

```  
"MediaRetention": {
    "BackupFileLimit": 3,
    "BackupRootDirectory": "/umbraco/mediaRetention"
}
```

By default it saves only 3 latest versions of media file.


## Credits

Icon created by - https://css.gg
## License

[MIT](https://opensource.org/license/mit/)

