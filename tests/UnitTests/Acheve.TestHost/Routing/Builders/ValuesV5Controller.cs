﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Acheve.TestHost.Routing.Models;

namespace UnitTests.Acheve.TestHost.Builders;

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

    [HttpPost(nameof(PostFile))]
    public async Task<ActionResult<string>> PostFile(IFormFile file)
    {
        if (file is null)
            return BadRequest();

        var content = await ReadFormFile(file);

        return Ok(content);
    }

    [HttpPost(nameof(PostObjectWithFile))]
    public async Task<ActionResult<string>> PostObjectWithFile([FromForm] ParamWithFile model)
    {
        if (model is null)
            return BadRequest();

        var content = await ReadFormFile(model.File);

        return Ok($"{model.Id}+{content}");
    }

    private static async Task<string> ReadFormFile(IFormFile file)
    {
        if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file.Name) || string.IsNullOrEmpty(file.ContentType))
        {
            throw new ArgumentException("Error in file", nameof(file));
        }

        using var reader = new StreamReader(file.OpenReadStream());
        return await reader.ReadToEndAsync();
    }

    [HttpPost($"{nameof(PostWithDifferentFroms)}/{{{nameof(ParamWithDifferentFroms.ParamFromRoute)}}}")]
    public ActionResult<ParamWithDifferentFroms> PostWithDifferentFroms(ParamWithDifferentFroms request)
        => Ok(request);

    [HttpPut($"{nameof(PutWithDifferentFroms)}/{{{nameof(ParamWithDifferentFroms.ParamFromRoute)}}}")]
    public ActionResult<ParamWithDifferentFroms> PutWithDifferentFroms(ParamWithDifferentFroms request)
        => Ok(request);
}
