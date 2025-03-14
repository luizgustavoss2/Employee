using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Application.UseCases;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GS.Employee.Application.Tests.UseCases
{
    public class UserInsertCommandHandlerTests
    {
        private readonly Mock<IRepositoryUser> _mockUserRepository;
        private readonly Mock<IRepositoryUserPhone> _mockUserPhoneRepository;
        private readonly Mock<IRepositoryUserPermission> _mockUserPermissionRepository;
        private readonly Mock<ILogger<UserInsertCommandHandler>> _mockLogger = new();
        private readonly UserInsertCommandHandler _handler;

        public UserInsertCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IRepositoryUser>();
            _mockUserPhoneRepository = new Mock<IRepositoryUserPhone>();
            _mockUserPermissionRepository = new Mock<IRepositoryUserPermission>();
            _handler = new UserInsertCommandHandler(
                _mockUserRepository.Object,
                _mockUserPhoneRepository.Object,
                _mockUserPermissionRepository.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new UserInsertCommandRequest(

                "John",
                "Doe",
                "john.doe@example.com",
                "123456789",
                "1990-01-01",
                Guid.NewGuid(),
                "password",
                new List<string> { "1234567890" },
                new List<int> { 1, 2 },
                Guid.NewGuid()
           );

            request.Validate(request);

            _mockUserRepository.Setup(repo => repo.GetByFilters(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(new List<User>());

            _mockUserRepository.Setup(repo => repo.GetByUserId(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            _mockUserPermissionRepository.Setup(repo => repo.GetByUserId(It.IsAny<Guid>()))
                .ReturnsAsync(new List<UserPermission> { new UserPermission { PermissionId = 1 }, new UserPermission { PermissionId = 2 } });

            _mockUserRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Empty(response.Notifications);
        }

        [Fact]
        public async Task Handle_InvalidBirthDate_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UserInsertCommandRequest
            (  
                "John",
                "Doe",
                "john.doe@example.com",
                "123456789",
                "2010-01-01", // Menor de 18 anos
                Guid.NewGuid(),
                "password",
                new List<string> { "1234567890" },
                new List<int> { 1, 2 },
                Guid.NewGuid()
                
            );

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response.Notifications);
            Assert.Equal("Funcionário deve ser maior de idade!", response.Notifications.First().Message);
        }

        [Fact]
        public async Task Handle_ExistingEmail_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UserInsertCommandRequest
            (
                "John",
                "Doe",
                "existing@example.com",
                "123456789",
                "1990-01-01",
                Guid.Empty,
                "password",
                new List<string> { "1234567890" },
                new List<int> { 1, 2 },
                Guid.NewGuid()
                
            );

            var userResult = new List<User>() { new User() { Email = "existing@example.com" } };

            _mockUserRepository.Setup(repo => repo.GetByFilters(null,null,request.Email,null,null))
                .ReturnsAsync(userResult);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response.Notifications);
            Assert.Equal("Email já cadastrado!", response.Notifications.First().Message);
        }

        [Fact]
        public async Task Handle_InvalidPermissions_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UserInsertCommandRequest
            (
                "John",
                "Doe",
                "existing@example.com",
                "123456789",
                "1990-01-01",
                Guid.Empty,
                "password",
                new List<string> { "1234567890" },
                new List<int> { 3},
                Guid.NewGuid()
            );

            _mockUserPermissionRepository.Setup(repo => repo.GetByUserId(request.UserInsertId))
                .ReturnsAsync(new List<UserPermission> { new UserPermission { PermissionId = 1 } });

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response.Notifications);
            Assert.Equal("Permissão não pode ser maior que do usuário logado!", response.Notifications.First().Message);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UserInsertCommandRequest
            (
                null,
                "Doe",
                "existing@example.com",
                "123456789",
                "1990-01-01",
                Guid.NewGuid(),
                "password",
                new List<string> { "1234567890" },
                new List<int> { 3 },
                Guid.NewGuid()
            );

            _mockUserRepository.Setup(repo => repo.GetByFilters(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response.Notifications);
            Assert.Contains("Nome é obrigatório!", response.Notifications.First().Message);
        }
    }
}