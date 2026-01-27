using MonetizationAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MonetizationAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimitTestController : ControllerBase
    {
        private readonly IRateLimitService _rateLimiter;
        private readonly IApiUsageLogger _logger;
        public LimitTestController(IRateLimitService rateLimiter, IApiUsageLogger logger)
        {
            _rateLimiter = rateLimiter;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromHeader] Guid customerId)
        {
            var allowed = await _rateLimiter.IsRequestAllowedAsync(customerId, "/api/LimitTest");
            await _logger.LogAsync(customerId, "/api/LimitTest", allowed);

            if (!allowed)
                return StatusCode(429, new { message = "Rate limit or monthly quota exceeded." });

            return Ok("API call successful");
        }
    }
}
