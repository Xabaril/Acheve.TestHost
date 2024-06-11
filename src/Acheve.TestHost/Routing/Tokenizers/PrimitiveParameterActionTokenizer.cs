using System;
using System.Globalization;
using System.Text.Json;

namespace Acheve.TestHost.Routing.Tokenizers;

internal class PrimitiveParameterActionTokenizer
    : ITokenizer
{
    public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens)
        where TController : class
    {
        foreach (var argument in action.ArgumentValues.Values)
        {
            if (IgnoreHeader(argument))
            {
                continue;
            }

            Type parameterType = argument.Type;
            if (!parameterType.IsPrimitiveType())
            {
                continue;
            }

            object tokenValue = argument.Instance;
            if (tokenValue == null)
            {
                continue;
            }

            string tokenName = argument.Name.ToLowerInvariant();
            tokens.AddToken(tokenName, PrimitiveValueToString(tokenValue), isConventional: false);
        }
    }

    public static string PrimitiveValueToString<T>(T value)
        => value switch
        {
            (DateTime or DateTimeOffset) and IFormattable dateTimeValue => dateTimeValue.ToString("o", CultureInfo.InvariantCulture),
            float or double or long or decimal => JsonSerializer.Serialize(value),
            _ => value.ToString()
        };

    private static bool IgnoreHeader(TestServerArgument parameter)
        => parameter.FromType.HasFlag(TestServerArgumentFromType.Header);
}