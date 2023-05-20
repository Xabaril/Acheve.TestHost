using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UnitTests.Acheve.TestHost.Routing.Models;

namespace UnitTests.Acheve.TestHost.Builders;

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

    [HttpGet("arrayPerson")]
    public ActionResult<Person[]> PersonArraySupport([FromQuery] Person[] param1)
    {
        return Ok(param1);
    }

    [HttpPost("{test_id:guid}")]
    public ActionResult<RouterAndBodyParamsResponse> AllowRouterAndBodyParams([FromRoute] Guid test_id, [FromBody] Person person)
    {
        return Ok(new RouterAndBodyParamsResponse { TestId = test_id, Person = person });
    }

    [HttpGet("{param1:int:min(1)}/params/{param2:int:min(1)}")]
    public ActionResult<string> GetWithSeveralColon(int param1, int param2)
    {
        return Ok($"{param1}/{param2}");
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult ActionNameInRoute()
    {
        return Ok();
    }

    [HttpGet(nameof(GetWithObject))]
    public ActionResult<ParamWithSeveralTypes> GetWithObject([FromQuery] ParamWithSeveralTypes param)
        => Ok(param);

    [HttpPost(nameof(PostWithObject))]
    public ActionResult<ParamWithSeveralTypes> PostWithObject([FromBody] ParamWithSeveralTypes param)
        => Ok(param);

    [HttpPost(nameof(PostWithObjectFromForm))]
    public ActionResult<ParamWithSeveralTypes> PostWithObjectFromForm([FromForm] ParamWithSeveralTypes param)
        => Ok(param);

    [HttpGet(nameof(GetWithListParam))]
    public ActionResult<IEnumerable<string>> GetWithListParam([FromQuery] IEnumerable<string> param)
        => Ok(param);

    [HttpPost(nameof(PostWithListParam))]
    public ActionResult<IEnumerable<string>> PostWithListParam([FromBody] IEnumerable<string> param)
        => Ok(param);

    [HttpGet(nameof(GetWithDatetimeParam))]
    public ActionResult<DateTime> GetWithDatetimeParam([FromQuery] DateTime param)
        => Ok(param);

    [HttpPost(nameof(PostWithDatetimeParam))]
    public ActionResult<DateTime> PostWithDatetimeParam([FromBody] DateTime param)
        => Ok(param);

    [HttpGet(nameof(GetWithDatetimeListParam))]
    public ActionResult<IEnumerable<DateTime>> GetWithDatetimeListParam([FromQuery] IEnumerable<DateTime> param)
    => Ok(param);

    [HttpPost(nameof(PostWithDatetimeListParam))]
    public ActionResult<IEnumerable<DateTime>> PostWithDatetimeListParam([FromBody] IEnumerable<DateTime> param)
        => Ok(param);
}