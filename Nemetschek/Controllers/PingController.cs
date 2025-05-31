using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nemetschek.API.Contract.Constants;
using Nemetschek.Services.Interfaces;

namespace Nemetschek.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Ping)]
    public class PingController : ControllerBase
    {

        private readonly IPingtService _pingService;
        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger, IPingtService pingtService)
        {
            _logger = logger;
            _pingService = pingtService;
        }

        [HttpGet]

        //Small little endpoint to test if the API is alive and connected to the DB
        //In production it would be protected behind [Authorize] and/or never part of the final code
        public IActionResult Get()
        {
            return Ok(new { alive = _pingService.Ping() });
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult Secure()
        {
            return Ok(new { alive = _pingService.Ping() });
        }
    }
}
