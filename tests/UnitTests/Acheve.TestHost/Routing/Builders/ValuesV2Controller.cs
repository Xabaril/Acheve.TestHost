using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/{version}/values")]
    public class ValuesV2Controller
        : Controller
    {
        [HttpGet()]
        public IActionResult Get(int id = 0)
        {
            return Ok();
        }
    }
}
