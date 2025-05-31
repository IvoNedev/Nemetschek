using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nemetschek.API.Contract.Constants;
using Nemetschek.API.Contract.Auth.Request;
using Nemetschek.API.Contract.Auth.Response;
using Nemetschek.Services;
using Nemetschek.Services.Interfaces;

namespace Nemetschek.API.User.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Auth)]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService= authService;

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] AuthRequest req)
        {
            var auth = await _authService.AuthenticateAsync(req);

            if (auth is null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(auth);
        }
    }
}
