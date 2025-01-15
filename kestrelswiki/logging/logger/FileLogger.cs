using kestrelswiki.service.file;

namespace kestrelswiki.logging.logger;

public class FileLogger(LogDomain logDomain, string dateFormat, string logFilePath, IFileWriter fileWriter) : ILogger
{
    public void Write(object message)
    {
        fileWriter.WriteLine(message.ToString() ?? "", logFilePath);
    }
}