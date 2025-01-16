using kestrelswiki.logging;

namespace kestrelswiki.service.file;

public interface IFileWriter
{
    Try<bool> Write(string contents, string fileName);
    Try<bool> WriteLine(string contents, string fileName);
}