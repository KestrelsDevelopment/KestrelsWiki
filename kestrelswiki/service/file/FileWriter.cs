namespace kestrelswiki.service.file;

public class FileWriter(ILogger logger) : IFileWriter
{
    public Try<bool> Write(string contents, string fileName)
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
            return Try<bool>.Fail($"Unable to write file at {fileName}: {e.Message}");
        }

        return new(true);
    }

    public Try<bool> WriteLine(string contents, string fileName)
    {
        return Write(contents + Environment.NewLine, fileName);
    }

    public Try<bool> CreateDirectory(string directoryName)
    {
        try
        {
            Directory.CreateDirectory(directoryName);
        }
        catch (Exception e)
        {
            logger.Write(e);
            return Try<bool>.Fail(e.Message, e);
        }

        return new(true);
    }
}