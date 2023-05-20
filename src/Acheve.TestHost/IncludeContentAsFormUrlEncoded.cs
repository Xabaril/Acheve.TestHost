using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace Microsoft.AspNetCore.TestHost;

/// <summary>
/// An implementation of <see cref="RequestContentOptions"/> that includes 
/// the [FromForm] parameter as <see cref="IDictionary<>"/>
/// </summary>
public class IncludeContentAsFormUrlEncoded : RequestContentOptions
{
    /// <inheritdoc/>
    public override bool IncludeFromBodyAsContent => false;

    /// <inheritdoc/>
    public override bool IncludeFromFormAsContent => true;

    /// <inheritdoc/>
    public override Func<object, HttpContent> ContentBuilder =>
        content => new FormUrlEncodedContent(ToKeyValue(content));

    private IDictionary<string, string> ToKeyValue(object metaToken)
    {
        if (metaToken is null)
        {
            return null;
        }

        if (metaToken is not JToken token)
        {
            return ToKeyValue(JObject.FromObject(metaToken));
        }

        if (token.HasValues)
        {
            var contentData = new Dictionary<string, string>();
            var childrenContent = token.Children()
                                .AsEnumerable()
                                .Select(ToKeyValue)
                                .Where(childrenContent => childrenContent is not null);
            foreach (var childContent in childrenContent)
            {
                contentData = contentData.Concat(childContent)
                                         .ToDictionary(k => k.Key, v => v.Value);
            }

            return contentData;
        }

        var jValue = token as JValue;
        if (jValue?.Value == null)
        {
            return null;
        }

        var value = jValue?.Type switch
        {
            JTokenType.Date => jValue?.ToString("o", CultureInfo.InvariantCulture),
            JTokenType.Float => jValue?.ToString(),
            _ => jValue?.ToString(CultureInfo.InvariantCulture)
        };

        return new Dictionary<string, string> { { token.Path, value } };
    }
}
