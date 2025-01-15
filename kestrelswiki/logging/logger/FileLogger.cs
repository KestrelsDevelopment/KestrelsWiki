using kestrelswiki.logging.logFormat;
using kestrelswiki.service.file;

namespace kestrelswiki.logging.logger;

public class FileLogger(
    LogDomain logDomain,
    ILogFormatter logFormatter,
    string logFilePath,
    IFileWriter fileWriter) : ILogger
{
    public void Write(object message)
    {
        fileWriter.WriteLine(logFormatter.Format(logDomain, message), logFilePath);
    }
}