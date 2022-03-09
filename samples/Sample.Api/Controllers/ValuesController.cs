using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Models;

namespace Sample.Api.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Authorize(Policy = "ValidateClaims")]
        [HttpGet]
        public IActionResult Values()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("schema")]
        public IActionResult ValuesWithSchema()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [HttpGet("public")]
        public IActionResult PublicValues()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [HttpGet("model")]
        public IActionResult ModelValues([FromQuery] ValueModel model)
        {
            if (model == null)
                return Values();
            
            return Ok(new[] { model.Value });
        }

        [HttpGet("primitive")]
        public IActionResult PrimitiveValues(string value)
        {
            if (value == null)
                return Values();

            return Ok(new[] { value });
        }
    }
}
