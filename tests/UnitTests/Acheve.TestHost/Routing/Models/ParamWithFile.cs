using Microsoft.AspNetCore.Http;
using System;

namespace UnitTests.Acheve.TestHost.Routing.Models;

public class ParamWithFile
{
    public int Id { get; set; }
    public IFormFile File { get; set; }

    public ParamWithFile() { }

    public ParamWithFile(IFormFile file)
    {
        var random = new Random();

        Id = random.Next();
        File = file;
    }
}
