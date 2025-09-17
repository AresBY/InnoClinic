using InnoClinic.Authorization.API.Controllers;
using InnoClinic.Authorization.Application.DTOs;
using InnoClinic.Authorization.Application.Features.Auth.Commands.RegisterUser;
using InnoClinic.Authorization.Application.Features.Auth.Queries.GetAllUsers;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace InnoClinic.Authorization.UnitTests.API.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        [Fact]
        public async Task RegisterPatient_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var command = new RegisterUserCommand { Email = "test@mail.com", Password = "123456" };
            var resultDto = new UserDto { Id = Guid.NewGuid(), Email = command.Email };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultDto);

            // Act
            var response = await _controller.RegisterPatient(command, CancellationToken.None);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(response);
            var value = Assert.IsType<UserDto>(created.Value);
            Assert.Equal(command.Email, value.Email);
            Assert.Equal(UserRole.Patient, command.Role);
        }

        [Fact]
        public async Task Register_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var command = new RegisterUserCommand { Email = "doctor@mail.com", Password = "123456", Role = UserRole.Doctor };
            var resultDto = new UserDto { Id = Guid.NewGuid(), Email = command.Email };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultDto);

            // Act
            var response = await _controller.Register(command, CancellationToken.None);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(response);
            var value = Assert.IsType<UserDto>(created.Value);
            Assert.Equal(command.Email, value.Email);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOk()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new() { Id = Guid.NewGuid(), Email = "a@mail.com" }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            var controller = new AuthController(mediatorMock.Object);

            // Act
            var response = await controller.GetAllUsers(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsAssignableFrom<IEnumerable<UserDto>>(ok.Value);
            Assert.Single(result);
        }
    }
}
