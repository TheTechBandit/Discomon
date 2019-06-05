using System;
using Xunit;

namespace DiscomonProject.xUnit.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void BasicLoggerTest()
        {
            var logger = Unity.Resolve<ILogger>();

            Assert.NotNull(logger);

            logger.Log("Hello, World!");
            Assert.Throws<ArgumentException>(() => logger.Log(null));
        }
    }
}