using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Acheve.TestHost.Routing.Models;

public class ParamWithDifferentFroms
{
    [FromRoute]
    public string ParamFromRoute { get; set; }

    [FromQuery]
    public string ParamFromQuery { get; set; }

    [FromHeader]
    public string ParamFromHeader { get; set; }

    [FromBody]
    public ParamWithSeveralTypes ParamFromBody { get; set; }
}
