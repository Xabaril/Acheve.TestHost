using Acheve.TestHost.Routing;
using System;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.TestHost;

public static class TestServerExtensions
{
    /// <summary>
    /// Create a <see cref="Microsoft.AspNetCore.TestHost.RequestBuilder"/> configured automatically
    /// with the generated uri from the <paramref name="actionSelector"/>.
    /// At this moment this method only resolver HTTP API using ASP.NET CORE Attribute Routing
    /// </summary>
    /// <typeparam name="TController">The controller to use</typeparam>
    /// <param name="server">The TestServer</param>
    /// <param name="actionSelector">The action selector used to discover the uri</param>
    /// <param name="tokenValues">The optional token values used to create the uri</param>
    /// <param name="contentOptions">Determines if [FromBody] arguments are included as request content. 
    ///      By default they are included as application/json content</param>
    /// <returns>RequestBuilder configured automatically</returns>
    public static RequestBuilder CreateHttpApiRequest<TController>(this TestServer server,
        Expression<Func<TController, object>> actionSelector,
        object tokenValues = null,
        RequestContentOptions contentOptions = null)
        where TController : class
        => UriDiscover.CreateHttpApiRequest<TController>(server, actionSelector, tokenValues, contentOptions);

    /// <summary>
    /// Create a <see cref="Microsoft.AspNetCore.TestHost.RequestBuilder"/> configured automatically
    /// with the generated uri from the <paramref name="actionSelector"/>.
    /// At this moment this method only resolver HTTP API using ASP.NET CORE Attribute Routing
    /// </summary>
    /// <typeparam name="TController">The controller to use</typeparam>
    /// <typeparam name="TActionResponse">actionSelector response type</typeparam>
    /// <param name="server">The TestServer</param>
    /// <param name="actionSelector">The action selector used to discover the uri</param>
    /// <param name="tokenValues">The optional token values used to create the uri</param>
    /// <param name="contentOptions">Determines if [FromBody] arguments are included as request content. 
    ///      By default they are included as application/json content</param>
    /// <returns>RequestBuilder configured automatically</returns>
    public static RequestBuilder CreateHttpApiRequest<TController, TActionResponse>(this TestServer server,
        Expression<Func<TController, TActionResponse>> actionSelector,
        object tokenValues = null,
        RequestContentOptions contentOptions = null)
        where TController : class
        => UriDiscover.CreateHttpApiRequest<TController>(server, actionSelector, tokenValues, contentOptions);
}
