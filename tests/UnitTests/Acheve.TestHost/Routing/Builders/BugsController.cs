using Microsoft.AspNetCore.Mvc;
using System;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/[controller]")]
    public class BugsController
        : Controller
    {
        [HttpGet("{param1}/{param2}")]
        public IActionResult GuidSupport(string param1,Guid param2)
        {
            return Ok();
        }
    }
}
