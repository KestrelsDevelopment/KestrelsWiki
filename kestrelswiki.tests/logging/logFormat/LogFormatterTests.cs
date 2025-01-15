using kestrelswiki.logging.logFormat;

namespace kestrelswiki.tests.logging.logFormat;

public class LogFormatterTests
{
    [Test]
    [TestCase("yyyy-MM-dd HH:mm:ss")]
    [TestCase("dd/MM/yyyy HH:mm:ss")]
    public void FormatLog_FormatsLogWithDateTimeAndLogDomain(string dateFormat)
    {
        LogFormatter formatter = new(dateFormat);
        LogDomain logDomain = LogDomain.Testing;
        string logMessage = "test message";

        string expectedResult = $"{DateTime.Now.ToString(dateFormat)} [{logDomain.Name}] {logMessage}";

        string result = formatter.Format(logDomain, logMessage);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
}