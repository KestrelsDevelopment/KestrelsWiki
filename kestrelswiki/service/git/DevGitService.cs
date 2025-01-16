using System.Threading.Tasks;

namespace kestrelswiki.service.git;

public class DevGitService(ILogger logger) : IGitService
{
    public async Task<Try<bool>> TryCloneWebPageRepository()
    {
        logger.Write("Cloning Web Page Repository.");
        return new(true);
    }

    public Try<bool> TryPullContentRepository()
    {
        logger.Write("Pulling Content Repository.");
        return new(true);
    }
}