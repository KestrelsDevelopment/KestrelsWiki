using System.Threading.Tasks;

namespace kestrelswiki.service.git;

public interface IGitService
{
    [Obsolete("We don't serve webpage through api in production, but in dev a mock service is used")]
    Task<Try<bool>> TryCloneWebPageRepositoryAsync();

    Task<Try<bool>> TryPullContentRepositoryAsync();
}