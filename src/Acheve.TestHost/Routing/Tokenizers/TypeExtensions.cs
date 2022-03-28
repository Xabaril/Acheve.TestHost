using System;

namespace Acheve.TestHost.Routing.Tokenizers
{
    internal static class TypeExtensions
    {
        internal static bool IsPrimitiveType(this Type typeToInspect)
        {
            var type = Nullable.GetUnderlyingType(typeToInspect) ?? typeToInspect;

            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(Guid);
        }
    }
}