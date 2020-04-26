using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/values")]
    [ApiController]
    public class ValuesV3Controller
        : ControllerBase
    {
        [HttpGet("get1")]
        public IActionResult Get1([FromQuery]Pagination pagination)
        {
            return Ok();
        }

        [HttpGet("get2/{pageindex}/{pagecount}")]
        public IActionResult Get2([FromQuery]Pagination pagination)
        {
            return Ok();
        }

        [Route("get3"), HttpGet]
        public IActionResult Get3([FromQuery]Pagination pagination)
        {
            return Ok();
        }

        [Route("get4/{pageindex}/{pagecount}"), HttpGet]
        public IActionResult Get4([FromQuery]Pagination pagination)
        {
            return Ok();
        }

        [HttpGet("get5")]
        public IActionResult Get5([FromHeader]string custom, [FromQuery]Pagination pagination)
        {
            if (string.IsNullOrEmpty(custom))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("get6")]
        public IActionResult Get6([FromHeader]string custom1, [FromHeader]int custom2, [FromQuery]Pagination pagination)
        {
            if (string.IsNullOrEmpty(custom1))
            {
                return BadRequest();
            }

            if (custom2 == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("get7")]
        public IActionResult Get7([FromHeader]string custom)
        {
            if (string.IsNullOrEmpty(custom))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("")]
        public IActionResult Post1([FromBody]Pagination pagination)
        {
            return Ok();
        }

        [HttpPost("post/{id:int}")]
        public IActionResult Post2(int id, [FromBody]Pagination pagination1)
        {
            if (pagination1 == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("post3")]
        public IActionResult Post3([FromForm]Pagination pagination)
        {
            return Ok();
        }

        [HttpPost("post4/{id:int}")]
        public IActionResult Post4(int id, [FromForm]Pagination pagination1)
        {
            if (pagination1 == null)
            {
                return BadRequest();
            }

            if (pagination1.PageCount == 0 || pagination1.PageIndex == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("post5")]
        public IActionResult Post5([FromHeader]string custom)
        {
            if (string.IsNullOrEmpty(custom))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("post6")]
        public IActionResult Post6([FromHeader]string custom, [FromBody]Pagination pagination1)
        {
            if (string.IsNullOrEmpty(custom))
            {
                return BadRequest();
            }

            if (pagination1 == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        //[HttpPost("post/{id:int}/{pageIndex}/{pagecount}")]
        //public IActionResult Post3(int id, [FromBody]Pagination pagination1, Pagination pagination2)
        //{
        //    return Ok();
        //}

    }

    public class Pagination
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}
