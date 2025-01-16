namespace kestrelswiki.service.git;

public interface IGitService
{
    Try<bool> TryCloneWebPageRepository();

    Try<bool> TryPullContentRepository();
}