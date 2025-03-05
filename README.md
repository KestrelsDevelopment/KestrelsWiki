# Kestrel's Wiki

The backend API for for my wiki site.

## Documentation

### Prerequisite's
- install git
  

### Environment Variables
The following environment variables are read by the application:
- FILE_LOGGING (default: false) - Enables logging to a file
- LOG_PATH (default: ../logs) - The path where log files will get saved if FILE_LOGGING is set to true
- CONTENT_PATH (default: ../content) - The root path of the content repository
- WEBROOT_PATH (default: ../wwwroot) - The root path of the webpage repository
- ENABLE_WEBPAGE_API (default: false) - Whether the webpage should be served through the API
- CONTENT_REPOSITORY (default: https://github.com/AceOfKestrels/kestrelsnest.git) - The URL where the content repository should be cloned and pulled from
- LOG_DATE_FORMAT (default: dd/MM/yyyy HH:mm:ss) - The date and time format used in logs
- LOG_LEVEL (default: Information) - Controls which messages will be logged.
  - Possible values: Trace, Debug, Information, Warning, Error, Critical, None
- DISABLED_LOG_DOMAINS (default: none) - A comma-separated list of log domains that will not be logged.
  - See [LogDomain.cs](https://github.com/AceOfKestrels/KestrelsWiki/blob/main/kestrelswiki/logging/logFormat/LogDomain.cs) for a list of available log domains.
- LOG_STACKTRACES: (default: false) - Enables logging of the entire stacktrace if an error is returned.