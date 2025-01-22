using System.Threading.Tasks;

namespace kestrelswiki.service.git;

public interface IGitService
{
    Task<Try<bool>> TryCloneWebPageRepositoryAsync();

    Task<Try<bool>> TryPullContentRepositoryAsync();
}