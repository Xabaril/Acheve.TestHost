using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController
        : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get(int id = 0)
        {
            return Ok();
        }
        [HttpGet("stringasprimitive")]
        public IActionResult GetStringAsParameter(string value)
        {
            return Ok();
        }
        [HttpGet("decimalasprimitive")]
        public IActionResult GetDecimalAsParameter(decimal value)
        {
            return Ok();
        }
        [HttpGet("OverrideMethodName")]
        public IActionResult Get2(int id = 0)
        {
            return Ok();
        }

        [HttpGet("OverrideMethodName/{id}")]
        public IActionResult Get3(int id = 0)
        {
            return Ok();
        }

        [HttpGet("OverrideMethodName/{version}/{id}")]
        public IActionResult Get4(int id = 0)
        {
            return Ok();
        }


        [Route("OverrideRouteTemplateMethodName"),HttpGet]
        public IActionResult Get5(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName/{version}/{id}"), HttpGet]
        public IActionResult Get6(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName/{id:int}"),HttpGet]
        public IActionResult Get7(int id = 0)
        {
            return Ok();
        }

        [HttpGet("OverrideMethodName/{id:int}")]
        public IActionResult Get8(int id = 0)
        {
            return Ok();
        }

        [HttpPost("OverrideMethodName")]
        public IActionResult Post2(int id = 0)
        {
            return Ok();
        }

        [HttpPost("OverrideMethodName/{id}")]
        public IActionResult Post3(int id = 0)
        {
            return Ok();
        }

        [HttpPost("OverrideMethodName/{version}/{id}")]
        public IActionResult Post4(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName"),HttpPost]
        public IActionResult Post5(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName/{version}/{id}"), HttpPost]
        public IActionResult Post6(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName/{id:int}"), HttpPost]
        public IActionResult Post7(int id = 0)
        {
            return Ok();
        }

        [HttpPost("OverrideMethodName/{id:int}")]
        public IActionResult Post8(int id = 0)
        {
            return Ok();
        }

        [HttpPut("OverrideMethodName")]
        public IActionResult Put2(int id = 0)
        {
            return Ok();
        }

        [HttpPut("OverrideMethodName/{id}")]
        public IActionResult Put3(int id = 0)
        {
            return Ok();
        }

        [HttpPut("OverrideMethodName/{version}/{id}")]
        public IActionResult Put4(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName"), HttpPut]
        public IActionResult Put5(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName/{version}/{id}"), HttpPut]
        public IActionResult Put6(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName/{id:int}"), HttpPut]
        public IActionResult Put7(int id = 0)
        {
            return Ok();
        }

        [HttpPut("OverrideMethodName/{id:int}")]
        public IActionResult Put8(int id = 0)
        {
            return Ok();
        }

        [HttpDelete("OverrideMethodName")]
        public IActionResult Delete2(int id = 0)
        {
            return Ok();
        }

        [HttpDelete("OverrideMethodName/{id}")]
        public IActionResult Delete3(int id = 0)
        {
            return Ok();
        }

        [HttpDelete("OverrideMethodName/{version}/{id}")]
        public IActionResult Delete4(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName"), HttpDelete]
        public IActionResult Delete5(int id = 0)
        {
            return Ok();
        }

        [Route("OverrideRouteTemplateMethodName/{version}/{id}"), HttpDelete]
        public IActionResult Delete6(int id = 0)
        {
            return Ok();
        }
        [Route("OverrideRouteTemplateMethodName/{id:int}"), HttpDelete]
        public IActionResult Delete7(int id = 0)
        {
            return Ok();
        }
        [HttpDelete("OverrideMethodName/{id:int}")]
        public IActionResult Delete8(int id = 0)
        {
            return Ok();
        }
    }
}
