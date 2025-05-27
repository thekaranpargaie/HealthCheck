using Microsoft.AspNetCore.Mvc;
using HealthCheck.Main.Services;
using HealthCheck.Main.Models;

namespace HealthCheck.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthChecksController : ControllerBase
    {
        private readonly IHealthCheckHistoryStore _healthCheckHistoryStore;

        public HealthChecksController(IHealthCheckHistoryStore healthCheckHistoryStore)
        {
            _healthCheckHistoryStore = healthCheckHistoryStore;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthCheckPingResult>>> Get()
        {
            var results = _healthCheckHistoryStore.GetAllHistory(DateTime.Now.AddDays(-1));
            return Ok(results);
        }
    }
}