namespace kestrelswiki.service.file;

public interface IFileWriter
{
    bool Write(string contents, string fileName);
    bool WriteLine(string contents, string fileName);
}