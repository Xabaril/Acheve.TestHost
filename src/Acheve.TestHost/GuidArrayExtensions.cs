using System;
using System.Collections.Generic;
using System.Linq;

namespace Acheve.TestHost
{
    public static class GuidArrayExtensions
    {
        public static Guid[] GetArray(this IEnumerable<Guid> list)
        {
            return list != null
           ? list.ToArray()
           : Array.Empty<Guid>();
        }
    }
}