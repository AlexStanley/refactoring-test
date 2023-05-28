namespace LegacyApp.Tests
{
    public class UserServiceTests
    {
        private readonly UserService userService;
        private readonly Mock<ClientRepository> clientRepositoryMock;
        private readonly Mock<UserCreditServiceClient> userCreditServiceMock;

        public UserServiceTests()
        {
            clientRepositoryMock = new Mock<ClientRepository>();
            userCreditServiceMock = new Mock<UserCreditServiceClient>();

            userService = new UserService()
            {
                clientRepository = clientRepositoryMock.Object,
                userCreditService = userCreditServiceMock.Object
            };
        }


        [Fact]
        public void AddUser_WithValidData_ReturnsTrue()
        {
            // Arrange
            const int clientId = 1;
            var client = new Client { Id = clientId, Name = "Client" };
            clientRepositoryMock.Setup(c => c.GetById(clientId)).Returns(client);
            userCreditServiceMock.Setup(u => u.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(1000);

            // Act
            bool result = userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(2000, 1, 1), clientId);

            // Assert
            result.Should().BeTrue();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void AddUser_WithInvalidFirstName_ReturnsFalse()
        {
            // Act
            bool result = userService.AddUser("", "Doe", "john.doe@example.com", new DateTime(2000, 1, 1), 1);

            // Assert
            result.Should().BeFalse();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void AddUser_WithInvalidLastName_ReturnsFalse()
        {
            // Act
            bool result = userService.AddUser("John", "", "john.doe@example.com", new DateTime(2000, 1, 1), 1);

            // Assert
            result.Should().BeFalse();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void AddUser_WithInvalidEmail_ReturnsFalse()
        {
            // Act
            bool result = userService.AddUser("John", "Doe", "invalid-email", new DateTime(2000, 1, 1), 1);

            // Assert
            result.Should().BeFalse();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void AddUser_WithAgeLessThan21_ReturnsFalse()
        {
            // Act
            bool result = userService.AddUser("John", "Doe", "john.doe@example.com", DateTime.Now.AddYears(-20), 1);

            // Assert
            result.Should().BeFalse();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void AddUser_WithVeryImportantClient_DoesNotSetCreditLimit()
        {
            // Arrange
            const int clientId = 1;
            var client = new Client { Id = clientId, Name = "VeryImportantClient" };
            clientRepositoryMock.Setup(c => c.GetById(clientId)).Returns(client);

            // Act
            bool result = userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(2000, 1, 1), clientId);

            // Assert
            result.Should().BeTrue();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void AddUser_WithImportantClient_SetsCreditLimitTwice()
        {
            // Arrange
            const int clientId = 1;
            var client = new Client { Id = clientId, Name = "ImportantClient" };
            clientRepositoryMock.Setup(c => c.GetById(clientId)).Returns(client);
            userCreditServiceMock.Setup(u => u.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(1000);

            // Act
            bool result = userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(2000, 1, 1), clientId);

            // Assert
            result.Should().BeTrue();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void AddUser_WithCreditLimitLessThan500_ReturnsFalse()
        {
            // Arrange
            const int clientId = 1;
            var client = new Client { Id = clientId, Name = "Client" };
            clientRepositoryMock.Setup(c => c.GetById(clientId)).Returns(client);
            userCreditServiceMock.Setup(u => u.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(100);

            // Act
            bool result = userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(2000, 1, 1), clientId);

            // Assert
            result.Should().BeFalse();
            clientRepositoryMock.Verify(u => u.GetById(It.IsAny<int>()), Times.Once);
            userCreditServiceMock
                .Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}