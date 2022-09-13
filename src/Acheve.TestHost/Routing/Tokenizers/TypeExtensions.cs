using System.Collections;
using System.Linq;

namespace System
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

        internal static bool IsEnumerable(this Type typeToInspect)
        {
            return typeof(IEnumerable).IsAssignableFrom(typeToInspect);
        }
        
        internal static Type GetEnumerableElementType(this Type typeToInspect)
        {
            return typeToInspect.GetElementType() ?? typeToInspect.GetGenericArguments().FirstOrDefault();
        }
    }
}