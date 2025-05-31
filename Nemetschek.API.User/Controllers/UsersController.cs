using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nemetschek.API.Contract.Constants;
using Nemetschek.API.Contract.User.Request;
using Nemetschek.API.Contract.User.Response;
using Nemetschek.Services.Interfaces;

namespace Nemetschek.API.User.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Users)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
        {
            var response = await _userService.CreateAsync(request);
            return Created(string.Empty, response);
        }


        //Not part of the assignment but natural next implementation
        //public async Task<IActionResult> GetById(int id){}
    }
}
