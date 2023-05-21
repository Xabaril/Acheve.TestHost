using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

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
        content => GetMultipartFormDataContent(content);

    private MultipartFormDataContent GetMultipartFormDataContent(object data)
    {
        if (data is null)
        {
            return new MultipartFormDataContent();
        }

        var multipartContent = new MultipartFormDataContent();
        AddToMultipartFormDataContent(multipartContent, data);

        return multipartContent;
    }

    private void AddToMultipartFormDataContent(MultipartFormDataContent multipartContent, object data, string propertyName = null)
    {
        switch (data)
        {
            case CancellationToken:
                break;
            case IFormFile file:
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileContent = new ByteArrayContent(ms.ToArray());

                    multipartContent.Add(fileContent, file.Name, file.FileName);
                }
                break;
            case object when data.GetType().IsPrimitiveType():
                multipartContent.Add(new StringContent(PrimitiveValueToString(data)), propertyName);
                break;
            case object when data.GetType().IsEnumerable():
                foreach (var item in (IList)data)
                {
                    AddToMultipartFormDataContent(multipartContent, item, propertyName);
                }
                break;
            default:
                data.GetType().GetProperties().ToList().ForEach(p =>
                {
                    var pName = string.IsNullOrEmpty(propertyName) ? p.Name : $"{propertyName}.{p.Name}";
                    var value = p.GetValue(data);
                    if (value is not null)
                    {
                        AddToMultipartFormDataContent(multipartContent, value, pName);
                    }
                });
                break;
        }
    }

    private static string PrimitiveValueToString<T>(T value)
    => value switch
    {
        (DateTime or DateTimeOffset) and IFormattable dateTimeValue => dateTimeValue.ToString("o", CultureInfo.InvariantCulture),
        _ => value.ToString()
    };
}
