using System.Threading.Tasks;
using CliWrap;
using kestrelswiki.environment;

namespace kestrelswiki.service.git;

public class GitService(ILogger logger) : IGitService
{
    public async Task<Try<bool>> TryCloneWebPageRepositoryAsync()
    {
        logger.Write("Running git clone for webpage repository");
        string output = string.Empty;
        CommandTask<CommandResult> cloneTask = Cli.Wrap("git")
            .WithArguments(["clone", Variables.WebPageRepo, "."])
            .WithWorkingDirectory(Variables.WebRootPath)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(o => output = o))
            .ExecuteAsync();

        CommandResult result = await cloneTask.Task;
        if (!result.IsSuccess) logger.Write($"git clone failed: {output}");
        return result.IsSuccess ? new(true) : Try<bool>.Fail(output);
    }

    public async Task<Try<bool>> TryPullContentRepositoryAsync()
    {
        logger.Write("Running git pull for content repository");
        string output = string.Empty;
        CommandTask<CommandResult> cloneTask = Cli.Wrap("git")
            .WithArguments(["pull"])
            .WithWorkingDirectory(Variables.ContentPath)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(o => output = o))
            .ExecuteAsync();

        CommandResult result = await cloneTask.Task;
        if (!result.IsSuccess) logger.Write($"git pull failed: {output}");
        return result.IsSuccess ? new(true) : await TryCloneContentRepositoryAsync();
    }

    private async Task<Try<bool>> TryCloneContentRepositoryAsync()
    {
        logger.Write("Running git clone for content repository");
        string output = string.Empty;
        CommandTask<CommandResult> cloneTask = Cli.Wrap("git")
            .WithArguments(["clone", Variables.WebPageRepo, "."])
            .WithWorkingDirectory(Variables.WebRootPath)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(o => output = o))
            .ExecuteAsync();

        CommandResult result = await cloneTask.Task;
        if (!result.IsSuccess) logger.Write($"git clone failed: {output}");
        return result.IsSuccess ? new(true) : Try<bool>.Fail(output);
    }
}