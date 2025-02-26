using System.Collections.Generic;
using System.Linq;
using kestrelswiki.environment;
using kestrelswiki.extensions;
using kestrelswiki.models;

namespace kestrelswiki.service.file;

public class FileReader(ILogger logger) : IFileReader
{
    public Try<string> TryReadAllText(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return new Exception("Unable to read file: Path is empty.");

        return TryReadAllText(new FileInfo(Path.GetFullPath(path)));
    }

    public Try<string> TryReadAllText(FileInfo file)
    {
        if (!file.Exists) return new Exception("File does not exist.");

        try
        {
            using FileStream stream = file.OpenRead();
            using StreamReader reader = new(stream);

            return reader.ReadToEnd();
        }
        catch (Exception e)
        {
            string errorMessage = $"Unable to read file at {file.FullName}: {e.Message}";
            logger.Error(errorMessage);

            return e;
        }
    }

    public Try<IEnumerable<Article>> GetMarkdownFiles()
    {
        Stack<DirectoryInfo> directories = new();
        directories.Push(new(Variables.ContentPath));

        int contentPathLength = Path.GetFullPath(Variables.ContentPath).Length;

        List<Exception> exceptions = [];
        List<Article> articles = [];

        while (directories.Count > 0)
        {
            DirectoryInfo directory = directories.Pop();

            if (!directory.Exists) continue;

            articles.AddRange(directory.GetFiles("*.md")
                .Where(f => !f.Name.StartsWith('.'))
                .Select(f =>
                {
                    Try<string> fileContent = TryReadAllText(f.FullName).Catch(ex => exceptions.Add(ex));

                    return new Article
                    {
                        Path = f.FullName[contentPathLength..].Replace(Path.DirectorySeparatorChar, '/'),
                        Content = fileContent.Result ?? string.Empty
                    };
                })
                .Where(a => !a.Content.IsNullOrWhiteSpace()));

            directory.GetDirectories()
                .Where(subDir => !subDir.Name.StartsWith('.'))
                .ForEach(directories.Push);
        }

        return (articles, exceptions.Count > 0 ? new AggregateException(exceptions) : null);
    }

    public Try<bool> Exists(string path)
    {
        try
        {
            return File.Exists(path);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}