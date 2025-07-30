using Application.Features.Auth.Examples;
using InnoClinic.Authorization.Application.Features.Auth.Commands;
using InnoClinic.Authorization.Application.Features.Auth.Queries;
using InnoClinic.Authorization.Domain.Common.Enums;
using InnoClinicCommon.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace InnoClinic.Authorization.API.Controllers
{
    /// <summary>
    /// Handles authentication-related operations (registration, login, etc.)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new patient and sends a confirmation email.
        /// </summary>
        /// <param name="command">Registration data including email and password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Basic user info (ID and email)</returns>
        [HttpPost(nameof(RegisterPatient))]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            command.Role = UserRole.Patient;

            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(RegisterPatient), new { id = result.Id }, result);
        }

        /// <summary>
        /// Registers a new doctor and sends a confirmation email.
        /// </summary>
        /// <param name="command">Registration data including email and password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Basic user info (ID and email)</returns>
        [HttpPost(nameof(Register))]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
        }


        /// <summary>
        /// Returns a list of all registered users.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of basic user info (ID and email)</returns>
        [HttpGet(nameof(GetAllUsers))]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Confirms a user's email address.
        /// </summary>
        /// <param name="command">User ID to confirm</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status 200 OK if confirmed</returns>
        [HttpPost(nameof(ConfirmEmail))]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Email confirmed successfully." });
        }

        /// <summary>
        /// Signs in a user with email and password.
        /// </summary>
        /// <param name="command">Contains email and password for sign in.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Success message or error message in case of failure.</returns>
        [HttpPost(nameof(SignIn))]
        [SwaggerRequestExample(typeof(SignInCommand), typeof(SignInCommandExample))]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                UpdateRefreshTokenCookie(result.RefreshToken!);

                return Ok(new
                {
                    result.IsSuccess,
                    result.Message,
                    result.AccessToken,
                    result.UserId
                });
            }
            else
                return Unauthorized(new { message = result.ErrorMessage });
        }


        /// <summary>
        /// Checks whether a user with the specified email exists.
        /// This endpoint is used on the client side when the email input loses focus during registration,
        /// to proactively inform the user if the email is already taken.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>
        /// Returns 200 OK with true if the email is already registered,  
        /// 200 OK with false if the email is available,  
        /// or 400 BadRequest if the email parameter is missing or empty.
        /// </returns>
        [HttpGet(nameof(CheckEmailExists))]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var exists = await _mediator.Send(new CheckEmailExistsQuery(email));
            return Ok(exists);
        }

        /// <summary>
        /// Logs the user out by deleting the refresh token cookie.
        /// This endpoint is typically called when the user explicitly signs out,
        /// ensuring that their session cannot be refreshed silently using the existing refresh token.
        /// </summary>
        /// <returns>
        /// Returns 200 OK with a confirmation message upon successful logout.  
        /// The refresh token is removed by setting an expired HttpOnly cookie with the same name.  
        /// </returns>
        [HttpPost(nameof(Logout))]
        public IActionResult Logout()
        {
            UpdateRefreshTokenCookie(remove:true);

            return Ok(new { message = "Logged out successfully." });
        }

        /// <summary>
        /// Refreshes the access token using the refresh token stored in HttpOnly cookies.
        /// This endpoint is typically called when the access token has expired or is about to expire.
        /// If the refresh token is valid, a new access token (and optionally a new refresh token) is issued.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Returns 200 OK with a new access token if the refresh token is valid,  
        /// or 401 Unauthorized if the refresh token is missing or invalid.
        /// </returns>
        /// 
        [HttpPost(nameof(RefreshToken))]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "Refresh token is missing." });

            var command = new RefreshTokenCommand { RefreshToken = refreshToken };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return Unauthorized(new { message = result.ErrorMessage });

            UpdateRefreshTokenCookie(result.RefreshToken!);

            return Ok(new
            {
                accessToken = result.AccessToken
            });
        }

        private void UpdateRefreshTokenCookie(string refreshToken = "", bool remove = false)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(remove ? -1 : 30)
            };

            Response.Cookies.Append("refreshToken", remove ? "" : refreshToken, cookieOptions);
        }
    }
}
