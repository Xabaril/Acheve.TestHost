using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/{version}/values")]
    [ApiController]
    public class ValuesV2Controller
        : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get(int id = 0)
        {
            return Ok();
        }
    }
}
