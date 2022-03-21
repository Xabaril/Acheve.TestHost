using Microsoft.AspNetCore.Mvc;
using System;
using UnitTests.Acheve.TestHost.Routing.Models;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController
        : ControllerBase
    {
        [HttpGet("{param1}/{param2}")]
        public IActionResult GuidSupport(string param1, Guid param2)
        {
            return Ok();
        }

        [HttpGet("{param_1:guid}/{param_2:int}")]
        public IActionResult UnderDashSupport(Guid param_1, int param_2)
        {
            return Ok();
        }

        [HttpGet("nullableQueryParams")]
        public ActionResult<NullableQueryParamsResponse> NullableQueryParams(bool? param1, Guid? param2)
        {
            return Ok(new NullableQueryParamsResponse { Param1 = param1, Param2 = param2 });
        }
    }
}