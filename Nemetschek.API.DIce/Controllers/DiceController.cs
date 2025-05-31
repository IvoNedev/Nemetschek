using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nemetschek.API.Contract.Constants;
using Nemetschek.API.Contract.Dice.Request;
using Nemetschek.API.Contract.Dice.Response;
using Nemetschek.Data.Models.dice;
using Nemetschek.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nemetschek.API.Dice.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Dice)]
    [Authorize]
    public class DiceController : ControllerBase
    {
        private readonly IDiceService _diceService;
        public DiceController(IDiceService diceService) {
            _diceService = diceService;
        }
        [HttpPost("roll")]
        [Authorize]
        [ProducesResponseType(typeof(DiceRollResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Roll()
        {
            var userGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userGuid) || !Guid.TryParse(userGuid, out var userId))
                return Unauthorized();

            var response = await _diceService.RollAsync(userId);
            return Ok(response);
        }

        [HttpGet("history")]
        [ProducesResponseType(typeof(GetDiceRollsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> History([FromQuery] GetDiceHistoryRequest request)
        {
            var userGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userGuid) || !Guid.TryParse(userGuid, out var userId))
                return Unauthorized();

            var response = await _diceService.GetAsync(userId, request);
            return Ok(response);
        }

        [HttpPost("populateTestData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PopulateTestDataForFiltering()
        {
            var userGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userGuid) || !Guid.TryParse(userGuid, out var userId))
                return Unauthorized();

            await _diceService.PopulateTestDataForFilteringAsync(userId);
            return Ok();
        }
    }
}
