using System.Threading.Tasks;

namespace kestrelswiki.service.git;

public class DevGitService(ILogger logger) : IGitService
{
    public async Task<Try<bool>> TryCloneWebPageRepositoryAsync()
    {
        logger.Write("Running git clone for webpage repository");
        return new(true);
    }

    public async Task<Try<bool>> TryPullContentRepositoryAsync()
    {
        logger.Write("Running git pull for content repository");
        return new(true);
    }
}