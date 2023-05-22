using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Microsoft.AspNetCore.TestHost
{
    /// <summary>
    /// An implementation of <see cref="RequestContentOptions"/> that includes 
    /// the [FromBody] parameter as <see cref="StringContent"/> with application/json media Type
    /// </summary>
    public class IncludeContentAsJson : RequestContentOptions
    {
        /// <inheritdoc/>
        public override bool IncludeFromBodyAsContent => true;

        /// <inheritdoc/>
        public override bool IncludeFromFormAsContent => false;

        /// <inheritdoc/>
        public override Func<object, HttpContent> ContentBuilder =>
            content => new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");
    }
}