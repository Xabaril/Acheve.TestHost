using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders
{
    public class ImplementationAbstractController : AbstractController
    {
        public const string VIRTUAL_METHOD_RESULT = "from implementation";
        
        public override IActionResult AbstractMethod()
        {
            return Ok();
        }

        [HttpGet(nameof(VirtualMethod))]
        public override IActionResult VirtualMethod()
        {
            return Ok(VIRTUAL_METHOD_RESULT);
        }
    }
}
