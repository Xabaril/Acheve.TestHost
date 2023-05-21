using System;
using System.Net.Http;

namespace Microsoft.AspNetCore.TestHost
{
    /// <summary>
    /// Define how the library should behave when there are arguments with the [FromBody] attribute.
    /// </summary>
    public abstract class RequestContentOptions
    {
        /// <summary>
        /// If true, if there are arguments with the [FromBody] attribute they are included as content in the request
        /// </summary>
        public abstract bool IncludeFromBodyAsContent { get; }

        /// <summary>
        /// If true, if there are arguments with the [FromForm] attribute they are included as content in the request
        /// </summary>
        public abstract bool IncludeFromFormAsContent { get; }

        /// <summary>
        /// Factory method to create the HttpContent
        /// </summary>
        public abstract Func<object, HttpContent> ContentBuilder { get; }
    }
}