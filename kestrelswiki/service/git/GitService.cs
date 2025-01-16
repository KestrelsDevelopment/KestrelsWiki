using System.Threading.Tasks;
using CliWrap;
using kestrelswiki.environment;

namespace kestrelswiki.service.git;

public class GitService(ILogger logger) : IGitService
{
    public async Task<Try<bool>> TryCloneWebPageRepository()
    {
        string output = string.Empty;
        CommandTask<CommandResult> commandTask = Cli.Wrap("git")
            .WithArguments(["clone", Variables.WebPageRepo])
            .WithWorkingDirectory(Variables.ContentPath)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(o => output = o))
            .ExecuteAsync();

        CommandResult result = await commandTask.Task;
        return result.IsSuccess ? new(true) : Try<bool>.Fail(output);
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