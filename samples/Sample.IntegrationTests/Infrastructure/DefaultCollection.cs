using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sample.IntegrationTests.Infrastructure
{
    [CollectionDefinition(Collections.Default)]
    public class DefaultCollection : ICollectionFixture<DefaultFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
