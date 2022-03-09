using Microsoft.AspNetCore.Mvc;
using System;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController
        : ControllerBase
    {
        [HttpGet("{param1}/{param2}")]
        public IActionResult GuidSupport(string param1,Guid param2)
        {
            return Ok();
        }

        [HttpGet("array")]
        public ActionResult<Guid[]> GuidArraySupport([FromQuery]Guid[] param1)
        {
            return Ok(param1);
        }

        [HttpGet("array")]
        public ActionResult<Tuple<Guid[], Guid[]>> TwoGuidArraySupport([FromQuery] Guid[] param1, Guid[] param2)
        {
            return Ok(new Tuple<Guid[], Guid[]>(param1, param2));
        }
    }
}
