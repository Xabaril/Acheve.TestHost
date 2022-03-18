﻿using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpGet("arrayGuid")]
        public ActionResult<Guid[]> GuidArraySupport([FromQuery] Guid[] param1)
        {
            return Ok(param1);
        }

        [HttpGet("arrayInt")]
        public ActionResult<int[]> IntArraySupport([FromQuery] int[] param1)
        {
            return Ok(param1);
        }

        [HttpGet("arrayString")]
        public ActionResult<string[]> StringArraySupport([FromQuery] string[] param1)
        {
            return Ok(param1);
        }
    }
}