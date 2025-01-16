using System.Threading.Tasks;

namespace kestrelswiki.service.git;

public interface IGitService
{
    Task<Try<bool>> TryCloneWebPageRepository();

    Try<bool> TryPullContentRepository();
}