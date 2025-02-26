using System.Threading.Tasks;

namespace kestrelswiki.service.git;

public class DevGitService(ILogger logger) : IGitService
{
    public async Task<Try<bool>> TryCloneWebPageRepositoryAsync()
    {
        logger.Info("Running git clone for webpage repository");

        return true;
    }

    public async Task<Try<bool>> TryPullContentRepositoryAsync()
    {
        logger.Info("Running git pull for content repository");

        return true;
    }
}