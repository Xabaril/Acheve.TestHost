using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

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
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
    }
}