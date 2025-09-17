using InnoClinic.Authorization.Application.Features.Auth.Commands.RegisterUser;
using InnoClinic.Authorization.Application.Interfaces;
using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Domain.Entities;

using InnoClinicCommon.Enums;

using MassTransit;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using Moq;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IEmailSender> _emailSenderMock = new();
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();

    private RegisterUserCommandHandler CreateHandler() =>
        new(_userRepositoryMock.Object, _emailSenderMock.Object,
            _passwordHasherMock.Object, _configurationMock.Object,
            _publishEndpointMock.Object);

    [Fact]
    public async Task Handle_ShouldCreatePatient()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "patient@mail.com",
            Password = "Password123!",
            ReEnteredPassword = "Password123!",
            Role = UserRole.Patient
        };
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(r => r.ExistsAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(userId);

        _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<User>(), command.Password))
                           .Returns("hashedPassword");
        _configurationMock.Setup(c => c["Frontend:BaseUrl"]).Returns("https://frontend.test");

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(command.Email, result.Email);
        Assert.Equal(UserRole.Patient, result.Role);

        _emailSenderMock.Verify(e => e.SendEmailAsync(
            command.Email,
            It.IsAny<string>(),
            It.Is<string>(b => b.Contains("confirm-email")),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
