using kestrelswiki.logging;
using kestrelswiki.logging.logger;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service.file;
using Moq;

namespace kestrelswiki.tests.logging.loggerFactory;

public class DefaultLoggerFactoryTests
{
    private DefaultLoggerFactory _factory;
    private Mock<IFileWriter> _fileWriterMock;

    [SetUp]
    public void Setup()
    {
        _fileWriterMock = new Mock<IFileWriter>();
        _factory = new DefaultLoggerFactory("", "", _fileWriterMock.Object);
    }

    [Test]
    public void DefaultLoggerFactory_Create_CreatesMultiLogger()
    {
        ILogger logger = _factory.CreateLogger(LogDomain.Startup);

        Assert.That(logger, Is.InstanceOf<MultiLogger>());
    }

    [Test]
    public void DefaultLoggerFactory_Create_CreatesLoggerThatLogsToFile()
    {
        ILogger logger = _factory.CreateLogger(LogDomain.Startup);
        logger.Write("testMessage");

        _fileWriterMock.Verify(writer => writer.WriteLine(It.IsAny<string>(), It.IsAny<string>()));
    }
}