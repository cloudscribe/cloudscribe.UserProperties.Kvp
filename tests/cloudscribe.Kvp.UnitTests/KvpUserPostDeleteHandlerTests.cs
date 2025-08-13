using cloudscribe.Kvp.Models;
using cloudscribe.UserProperties.Kvp;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cloudscribe.Kvp.UnitTests
{
    public class KvpUserPostDeleteHandlerTests
    {
        private readonly Mock<IKvpStorageService> _mockKvpStorage;
        private readonly Mock<ILogger<KvpUserPostDeleteHandler>> _mockLogger;
        private readonly KvpUserPostDeleteHandler _handler;
        private readonly Guid _siteId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        public KvpUserPostDeleteHandlerTests()
        {
            _mockKvpStorage = new Mock<IKvpStorageService>();
            _mockLogger = new Mock<ILogger<KvpUserPostDeleteHandler>>();
            _handler = new KvpUserPostDeleteHandler(_mockKvpStorage.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task HandleUserPostDelete_WithUserKvpItems_DeletesAllItems()
        {
            // Arrange
            var kvpItems = new List<IKvpItem>
            {
                CreateMockKvpItem("1", "FirstName", "John"),
                CreateMockKvpItem("2", "LastName", "Doe"),
                CreateMockKvpItem("3", "MembershipNo", "12345")
            };

            _mockKvpStorage
                .Setup(x => x.FetchById(
                    _siteId.ToString(),
                    "*",
                    _siteId.ToString(),
                    _userId.ToString(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(kvpItems);

            _mockKvpStorage
                .Setup(x => x.Delete(_siteId.ToString(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleUserPostDelete(_siteId, _userId, CancellationToken.None);

            // Assert
            _mockKvpStorage.Verify(
                x => x.FetchById(
                    _siteId.ToString(),
                    "*",
                    _siteId.ToString(),
                    _userId.ToString(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _mockKvpStorage.Verify(
                x => x.Delete(_siteId.ToString(), "1", It.IsAny<CancellationToken>()),
                Times.Once);
            
            _mockKvpStorage.Verify(
                x => x.Delete(_siteId.ToString(), "2", It.IsAny<CancellationToken>()),
                Times.Once);
            
            _mockKvpStorage.Verify(
                x => x.Delete(_siteId.ToString(), "3", It.IsAny<CancellationToken>()),
                Times.Once);

            VerifyLogMessage(LogLevel.Information, "Starting KVP cleanup for deleted user");
            VerifyLogMessage(LogLevel.Information, "Successfully cleaned up 3 of 3 KVP items for user");
        }

        [Fact]
        public async Task HandleUserPostDelete_WithNoKvpItems_LogsDebugMessage()
        {
            // Arrange
            _mockKvpStorage
                .Setup(x => x.FetchById(
                    _siteId.ToString(),
                    "*",
                    _siteId.ToString(),
                    _userId.ToString(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IKvpItem>());

            // Act
            await _handler.HandleUserPostDelete(_siteId, _userId, CancellationToken.None);

            // Assert
            _mockKvpStorage.Verify(
                x => x.Delete(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);

            VerifyLogMessage(LogLevel.Debug, "No KVP data found for user");
        }

        [Fact]
        public async Task HandleUserPostDelete_WhenFetchThrowsException_LogsErrorAndDoesNotThrow()
        {
            // Arrange
            _mockKvpStorage
                .Setup(x => x.FetchById(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () => 
                await _handler.HandleUserPostDelete(_siteId, _userId, CancellationToken.None));

            // Should not throw - errors should be caught and logged
            Assert.Null(exception);

            VerifyLogMessage(LogLevel.Error, "Failed to cleanup KVP data for user");
        }

        [Fact]
        public async Task HandleUserPostDelete_WhenDeleteThrowsException_ContinuesWithOtherItems()
        {
            // Arrange
            var kvpItems = new List<IKvpItem>
            {
                CreateMockKvpItem("1", "FirstName", "John"),
                CreateMockKvpItem("2", "LastName", "Doe"),
                CreateMockKvpItem("3", "MembershipNo", "12345")
            };

            _mockKvpStorage
                .Setup(x => x.FetchById(
                    _siteId.ToString(),
                    "*",
                    _siteId.ToString(),
                    _userId.ToString(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(kvpItems);

            // First delete succeeds
            _mockKvpStorage
                .Setup(x => x.Delete(_siteId.ToString(), "1", It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Second delete fails
            _mockKvpStorage
                .Setup(x => x.Delete(_siteId.ToString(), "2", It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Delete failed"));

            // Third delete succeeds
            _mockKvpStorage
                .Setup(x => x.Delete(_siteId.ToString(), "3", It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(async () => 
                await _handler.HandleUserPostDelete(_siteId, _userId, CancellationToken.None));

            // Assert
            Assert.Null(exception); // Should not throw

            // Verify all delete attempts were made
            _mockKvpStorage.Verify(
                x => x.Delete(_siteId.ToString(), "1", It.IsAny<CancellationToken>()),
                Times.Once);
            
            _mockKvpStorage.Verify(
                x => x.Delete(_siteId.ToString(), "2", It.IsAny<CancellationToken>()),
                Times.Once);
            
            _mockKvpStorage.Verify(
                x => x.Delete(_siteId.ToString(), "3", It.IsAny<CancellationToken>()),
                Times.Once);

            // Should log success message with partial cleanup count
            VerifyLogMessage(LogLevel.Information, "Successfully cleaned up 2 of 3 KVP items for user");
            VerifyLogMessage(LogLevel.Error, "Failed to delete KVP item");
        }

        [Fact]
        public async Task HandleUserPostDelete_UsesCancellationToken()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var kvpItems = new List<IKvpItem>
            {
                CreateMockKvpItem("1", "FirstName", "John")
            };

            _mockKvpStorage
                .Setup(x => x.FetchById(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    cancellationToken))
                .ReturnsAsync(kvpItems);

            _mockKvpStorage
                .Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleUserPostDelete(_siteId, _userId, cancellationToken);

            // Assert
            _mockKvpStorage.Verify(
                x => x.FetchById(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    cancellationToken),
                Times.Once);

            _mockKvpStorage.Verify(
                x => x.Delete(It.IsAny<string>(), It.IsAny<string>(), cancellationToken),
                Times.Once);
        }


        private IKvpItem CreateMockKvpItem(string id, string key, string value)
        {
            var mock = new Mock<IKvpItem>();
            mock.Setup(x => x.Id).Returns(id);
            mock.Setup(x => x.Key).Returns(key);
            mock.Setup(x => x.Value).Returns(value);
            mock.Setup(x => x.SetId).Returns(_siteId.ToString());
            mock.Setup(x => x.SubSetId).Returns(_userId.ToString());
            return mock.Object;
        }

        private void VerifyLogMessage(LogLevel level, string messageContains)
        {
            _mockLogger.Verify(
                logger => logger.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(messageContains)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}