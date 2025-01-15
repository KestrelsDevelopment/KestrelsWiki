using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.service.file;

public class FileWriter(ILogger logger) : IFileWriter
{
    public bool Write(string contents, string fileName)
    {
        try
        {
            using FileStream stream = File.OpenWrite(fileName);
            using StreamWriter writer = new(stream);
            writer.Write(contents);
        }
        catch (Exception e)
        {
            logger.Write(e);
            return false;
        }

        return true;
    }

    public bool WriteLine(string contents, string fileName)
    {
        return Write(contents + Environment.NewLine, fileName);
    }
}