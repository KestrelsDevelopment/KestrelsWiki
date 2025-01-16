using kestrelswiki.logging;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.service.file;

public class FileReader(ILogger logger) : IFileReader
{
    public Try<string> TryReadAllText(string path)
    {
        try
        {
            using FileStream stream = File.OpenRead(path);
            using StreamReader reader = new(stream);
            return new(reader.ReadToEnd());
        }
        catch (Exception e)
        {
            string errorMessage = $"Unable to read file at {path}: {e.Message}";
            logger.Write(errorMessage);
            return new(new Exception(errorMessage, e));
        }
    }
}