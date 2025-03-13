using System.Threading.Tasks;
using kestrelswiki.environment;
using kestrelswiki.service.file;

namespace kestrelswiki.service.git;

public class DevGitService(ILogger logger, IFileReader fileReader) : IGitService
{
    public async Task<Try<bool>> TryPullContentRepositoryAsync()
    {
        await Task.Run(() => { }); // just to satisfy the async
        logger.Info("Running in dev mode. In prod, would pull content repo.");
        Try<bool> result = fileReader.DirectoryExists(Variables.ContentPath);

        if (!result.Success) return new Exception("Could not check content repo", result.Exception);

        return result.Result;
    }
}