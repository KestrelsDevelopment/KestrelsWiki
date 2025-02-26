using System.Threading.Tasks;
using CliWrap;
using kestrelswiki.environment;
using kestrelswiki.extensions;
using kestrelswiki.service.file;

namespace kestrelswiki.service.git;

public class GitService(ILogger logger, IFileWriter fileWriter) : IGitService
{
    public async Task<Try<bool>> TryPullContentRepositoryAsync()
    {
        Try<bool> tri = fileWriter.CreateDirectory(Variables.ContentPath);

        if (!tri.Success) return new Exception(tri.Exception?.Message ?? "error creating directory", tri.Exception);

        logger.Info("Running git pull for content repository");
        string output = string.Empty;
        Command command = Cli.Wrap("git")
            .WithArguments(["pull"])
            .WithWorkingDirectory(Variables.ContentPath)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(o => output = o));

        Try<CommandResult> pullResult = (await TryRunCommandAsync(command))
            .Then(_ => logger.Info(output))
            .Catch(e => logger.Error($"git pull failed: {(string.IsNullOrWhiteSpace(output) ? e.Message : output)}"));

        return pullResult.Success ? true : await TryCloneContentRepositoryAsync();
    }

    private async Task<Try<bool>> TryCloneContentRepositoryAsync()
    {
        logger.Info("Running git clone for content repository");
        string output = string.Empty;
        Command command = Cli.Wrap("git")
            .WithArguments(["clone", Variables.ContentRepository, "."])
            .WithWorkingDirectory(Variables.ContentPath)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(o => output = o));

        Try<CommandResult> cloneResult = (await TryRunCommandAsync(command))
            .Then(_ => logger.Info(output))
            .Catch(
                e => logger.Critical($"git clone failed: {(output.IsNullOrWhiteSpace() ? e.Message : output)}"));

        return cloneResult.Success ? true : new Exception(output);
    }

    protected async Task<Try<CommandResult>> TryRunCommandAsync(Command command)
    {
        try
        {
            CommandTask<CommandResult> task = command.WithValidation(CommandResultValidation.None).ExecuteAsync();
            CommandResult result = await task.Task;

            return result.IsSuccess
                ? result
                : new Exception($"Exit code does not indicate success. ({result.ExitCode})");
        }
        catch (Exception e)
        {
            return e;
        }
    }
}