# LogUploadService

This project is just a simple service to upload you GW2 arc logs automatically to [dps.report](https://dps.report/). It is a simple project and I'm thankful for every
feedback.

![.NET Core build](https://github.com/BergerAl/Gw2LogUploadService/workflows/.NET%20Core/badge.svg?branch=master)

## Configuration

The configuration happens via the [appsettings.json](.\LogUploadService\appsettings.json) ant the names below should be very intuitive. Just change `directoryName`, `userName` and `webhookURL` and the service should work.

```json
{
  "FileWatcher": {
    "directoryName": "yourPath\\arcdps.cbtlogs",
    "fileExtension": "zevtc"
  },
  "LogFilesUpdate": {
    "restClient": "https://dps.report"
  },
  "Discord": {
    "webhookURL": "yourDiscordWebhook",
    "userName": "",
    "avatarUrl": ""
  }
}
```

## Known Issues
    - sometimes there is a problem with the file handling, which leeds to permission problems which again leads to no upload. WIP

## Thanks to the following Persons

[nikolalv](https://github.com/nikolalv/DiscordWebhook)

## Todos
    - distribute a downloadable version
    - make it a real service
    - create an installer
    