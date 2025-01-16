using kestrelswiki.logging;
using kestrelswiki.logging.logger;
using kestrelswiki.service.file;
using kestrelswiki.service.webpage;
using Moq;

namespace kestrelswiki.tests.service.webpage;

public class WebpageService_Tests
{
    private Mock<IFileReader> _mockFileReader;
    private Mock<ILogger> _mockLogger;
    private WebpageService _webpageService;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new();
        _mockFileReader = new();
        _webpageService = new(_mockLogger.Object, _mockFileReader.Object);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void TryGetWebpage_InvalidPath_ReturnsFailure(string? path)
    {
        Try<string> result = _webpageService.TryGetWebpage(path);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void TryGetWebpage_ReadingFails_ReturnsFailure()
    {
        _mockFileReader.Setup(
            m => m.TryReadAllText(It.IsAny<string>())).Returns(new Try<string>(new Exception()));

        Try<string> result = _webpageService.TryGetWebpage("test");

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void TryGetWebpage_ReadingFails_WritesLog()
    {
        _mockFileReader.Setup(
            m => m.TryReadAllText(It.IsAny<string>())).Returns(new Try<string>(new Exception()));

        Try<string> result = _webpageService.TryGetWebpage("test");

        _mockLogger.Verify(m => m.Write(It.IsAny<string>()));
    }

    [Test]
    public void TryGetWebpage_ReadingSuccess_ReturnsSuccessWithContent()
    {
        string content = "test";
        _mockFileReader.Setup(
            m => m.TryReadAllText(It.IsAny<string>())).Returns(new Try<string>(content));

        Try<string> result = _webpageService.TryGetWebpage("testPath");

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Result, Is.EqualTo(content));
        });
    }
}