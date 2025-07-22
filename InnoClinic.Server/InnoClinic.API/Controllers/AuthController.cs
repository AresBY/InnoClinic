using InnoClinic.Application.Features.Auth.Commands;
using InnoClinic.Application.Features.Auth.Queries;
using InnoClinic.Server.Application.Features.Auth.Commands;
using InnoClinic.Server.Application.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Server.WebAPI.Controllers
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
        /// <returns>Basic patient info (ID and email)</returns>
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] RegisterPatientCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
        }

        /// <summary>
        /// Returns a list of all registered patients.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of basic patient info (ID and email)</returns>
        [HttpGet(nameof(GetAllPatients))]
        public async Task<IActionResult> GetAllPatients(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllPatientsQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Confirms a patient's email address.
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
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(30)
                };

                Response.Cookies.Append("refreshToken", result.RefreshToken!, cookieOptions);
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
            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

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

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            Response.Cookies.Append("refreshToken", result.RefreshToken!, cookieOptions);

            return Ok(new
            {
                accessToken = result.AccessToken
            });
        }
    }
}
