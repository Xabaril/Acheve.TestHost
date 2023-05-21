using Microsoft.AspNetCore.Mvc;
using System.Threading;

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

        [HttpPost("{id}")]
        public IActionResult Post3(string id, Pagination pagination1)
        {
            if (pagination1 == null || string.IsNullOrWhiteSpace(id))
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

        [HttpGet(nameof(GetWithCancellationToken))]
        public ActionResult<string> GetWithCancellationToken([FromQuery] string value, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest();
            }

            return Ok(value);
        }

        [HttpPost(nameof(PostWithCancellationToken))]
        public ActionResult<string> PostWithCancellationToken([FromBody] string value, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest();
            }

            return Ok(value);
        }

        [HttpGet(nameof(GetBadRequest))]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet(nameof(GetOk))]
        public IActionResult GetOk()
        {
            return Ok();
        }
    }
}
