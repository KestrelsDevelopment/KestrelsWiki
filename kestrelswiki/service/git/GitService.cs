using CliWrap;
using kestrelswiki.environment;

namespace kestrelswiki.service.git;

public class GitService(ILogger logger) : IGitService
{
    public Try<bool> TryCloneWebPageRepository()
    {
        Cli.Wrap("git").WithArguments(["clone"]).WithWorkingDirectory(Variables.ContentPath);

        throw new NotImplementedException();
    }

    public Try<bool> TryPullContentRepository()
    {
        throw new NotImplementedException();
    }

    private Try<bool> TryCloneContentRepository()
    {
        throw new NotImplementedException();
    }
}