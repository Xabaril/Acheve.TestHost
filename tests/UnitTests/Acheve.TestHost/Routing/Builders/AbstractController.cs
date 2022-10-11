using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class AbstractController : ControllerBase
    {
        [HttpGet(nameof(VirtualMethod))]
        public virtual IActionResult VirtualMethod()
        {
            return Ok();
        }

        [HttpGet(nameof(Virtual2Method))]
        public virtual IActionResult Virtual2Method()
        {
            return Ok();
        }

        [HttpGet(nameof(AbstractMethod))]
        public abstract IActionResult AbstractMethod();
    }
}
