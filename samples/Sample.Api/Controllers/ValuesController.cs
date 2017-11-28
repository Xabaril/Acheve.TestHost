using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Api.Controllers
{
    [Route("api/values")]
    public class ValuesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Values()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [HttpGet("public")]
        public IActionResult PublicValues()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}
