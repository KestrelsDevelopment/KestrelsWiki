using System.Collections.Generic;
using System.Linq;
using kestrelswiki.extensions;
using kestrelswiki.models;

namespace kestrelswiki.service.file;

public class FileReader(ILogger logger) : IFileReader
{
    public Try<string> TryReadAllText(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return new(new ArgumentException("Unable to read file: Path is empty."));

        return TryReadAllText(new FileInfo(Path.GetFullPath(path)));
    }

    public Try<string> TryReadAllText(FileInfo file)
    {
        if (!file.Exists) return Try<string>.Fail("File does not exist.");

        try
        {
            using FileStream stream = file.OpenRead();
            using StreamReader reader = new(stream);
            return new(reader.ReadToEnd());
        }
        catch (Exception e)
        {
            string errorMessage = $"Unable to read file at {file.FullName}: {e.Message}";
            logger.Error(errorMessage);
            return Try<string>.Fail(errorMessage, e);
        }
    }

    // TODO: Don't do this recursively
    public Try<IEnumerable<Article>> GetMarkdownFiles(string path)
    {
        DirectoryInfo directory = new(path);
        if (!directory.Exists) return new([]);

        List<Exception> exceptions = [];
        List<Article> files = directory
            .GetFiles("*.md")
            .Select(f =>
            {
                Try<string> fileContent = TryReadAllText(f.FullName).Catch(ex => exceptions.Add(ex));
                return new Article
                {
                    Path = f.FullName, // should be relative path from content root
                    Content = fileContent.Result ?? string.Empty
                };
            })
            .Where(a => !a.Content.IsNullOrWhiteSpace())
            .ToList();

        foreach (DirectoryInfo subDir in directory.GetDirectories().ToList().FindAll(d => d.Name != ".git"))
        {
            Try<IEnumerable<Article>> tri = GetMarkdownFiles(subDir.FullName);
            if (tri.Result is not null) files.AddRange(tri.Result ?? []);

            switch (tri.Exception)
            {
                case null:
                    break;
                case AggregateException aggEx:
                    exceptions.AddRange(aggEx.InnerExceptions);
                    break;
                default:
                    exceptions.Add(tri.Exception);
                    break;
            }
        }

        return new(files, new AggregateException(exceptions));
    }

    public Try<bool> Exists(string path)
    {
        try
        {
            return new(File.Exists(path));
        }
        catch (Exception e)
        {
            return Try<bool>.Fail(e.Message);
        }
    }
}