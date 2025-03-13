using System.Collections.Generic;
using kestrelswiki.models;

namespace kestrelswiki.service.file;

public interface IFileReader
{
    /// <summary>
    ///     Opens a file at path and reads its contents, returning a string containing it.
    /// </summary>
    /// <param name="path">The path to read at.</param>
    /// <param name="content"></param>
    /// <returns>The contents of the file, or null if an error occurs.</returns>
    Try<string> TryReadAllText(string path);

    Try<string> TryReadAllText(FileInfo file);

    /// <returns>An IEnumerable of FileInfos referring to .md files in the given directory and all subdirectories.</returns>
    Try<IEnumerable<Article>> GetMarkdownFiles();

    Try<bool> FileExists(string path);

    Try<bool> DirectoryExists(string path);
}