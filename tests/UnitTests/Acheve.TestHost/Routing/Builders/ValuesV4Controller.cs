using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Builders;

[Route("api/values")]
[ApiController]
public class ValuesV4Controller
    : ControllerBase
{
    [HttpGet("~/get1/{index}")]
    public IActionResult Get1(int index)
    {
        return Ok();
    }

    [HttpPost("~/post1/{index}")]
    public IActionResult Post1(int index)
    {
        return Ok();
    }

    [HttpPut("~/put1/{index}")]
    public IActionResult Put1(int index)
    {
        return Ok();
    }

    [HttpDelete("~/delete1/{index}")]
    public IActionResult Delete1(int index)
    {
        return Ok();
    }


    [HttpGet("~/get2/{index}")]
    public IActionResult Get2(int index, Pagination pagination)
    {
        return Ok();
    }

    [HttpPost("~/post2/{index}")]
    public IActionResult Post2(int index, Pagination pagination)
    {
        return Ok();
    }

    [HttpPut("~/put2/{index}")]
    public IActionResult Put2(int index, Pagination pagination)
    {
        return Ok();
    }

    [HttpDelete("~/delete2/{index}")]
    public IActionResult Delete2(int index, Pagination pagination)
    {
        return Ok();
    }

    [Route("~/get3/{index}"), HttpGet]
    public IActionResult Get3(int index, Pagination pagination)
    {
        return Ok();
    }

    [Route("~/post3/{index}"), HttpPost]
    public IActionResult Post3(int index, Pagination pagination)
    {
        return Ok();
    }

    [Route("~/put3/{index}"), HttpPut]
    public IActionResult Put3(int index, Pagination pagination)
    {
        return Ok();
    }

    [Route("~/delete3/{index}"), HttpDelete]
    public IActionResult Delete3(int index, Pagination pagination)
    {
        return Ok();
    }
}
