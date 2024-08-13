using System.Collections;
using System.Linq;

namespace System;

internal static class TypeExtensions
{
    internal static bool IsPrimitiveType(this Type typeToInspect)
    {
        var type = Nullable.GetUnderlyingType(typeToInspect) ?? typeToInspect;

        return type.IsPrimitive
            || type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(Guid)
            || type.IsDateTime()
            || type.IsEnum;
    }

    internal static bool IsDateTime(this Type typeToInspect)
    {
        return typeToInspect == typeof(DateTime) ||
            typeToInspect == typeof(DateTimeOffset) ||
            typeToInspect == typeof(DateOnly) ||
            typeToInspect == typeof(TimeSpan) ||
            typeToInspect == typeof(TimeOnly);
    }

    internal static bool IsEnumerable(this Type typeToInspect)
    {
        return typeof(IEnumerable).IsAssignableFrom(typeToInspect) && typeToInspect != typeof(string);
    }

    internal static Type GetEnumerableElementType(this Type typeToInspect)
    {
        return typeToInspect.GetElementType() ?? typeToInspect.GetGenericArguments().FirstOrDefault();
    }
}