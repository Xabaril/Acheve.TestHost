using System;
using System.Net.Http;

namespace Microsoft.AspNetCore.TestHost
{
    /// <summary>
    /// An implementation of <see cref="RequestContentOptions"/> that not includes 
    /// the [FromBody] parameter as the request content.
    /// </summary>
    public class NotIncludeContent : RequestContentOptions
    {
        /// <inheritdoc/>
        public override bool IncludeFromBodyAsContent => false;
        public override bool IncludeFromFormAsContent => false;

        /// <inheritdoc/>
        public override Func<object, HttpContent> ContentBuilder =>
            content => throw new InvalidOperationException("Unable to build content");
    }
}