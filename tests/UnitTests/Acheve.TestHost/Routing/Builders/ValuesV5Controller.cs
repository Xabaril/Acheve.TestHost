using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/values")]
    [ApiController]
    public class ValuesV5Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult Post1(Pagination pagination)
        {
            return Ok();
        }

        [HttpPost("{id:int}")]
        public IActionResult Post2(int id, Pagination pagination1)
        {
            if (pagination1 == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch1(int id, Pagination pagination1)
        {
            if (pagination1 == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
