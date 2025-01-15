using kestrelswiki.logging;
using kestrelswiki.logging.logger;
using kestrelswiki.logging.loggerFactory;

namespace kestrelswiki.tests.logging.loggerFactory;

public class DefaultLoggerFactoryTests
{
    [Test]
    public void DefaultLoggerFactory_Create_CreatesMultiLogger()
    {
        DefaultLoggerFactory factory = new("", "");

        ILogger logger = factory.CreateLogger(LogDomain.Startup);

        Assert.That(logger, Is.InstanceOf<MultiLogger>());
    }
}