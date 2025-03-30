using Lab2.DTOs;
using Lab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeatSolverController : ControllerBase
    {
        private IHeatingModelingService _service;

        public HeatSolverController (IHeatingModelingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> SetSettings([FromBody] SettingsDto settingsDto)
        {
            await _service.SetSettings(settingsDto);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<double[][][]>> Solve()
        {
            var result = await _service.Solve();

            if (result is null)
                return BadRequest("Not stable!");

            return Ok(result);
        }

    }
}
