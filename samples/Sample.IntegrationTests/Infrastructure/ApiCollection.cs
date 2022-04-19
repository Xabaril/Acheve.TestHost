﻿using Xunit;

namespace Sample.IntegrationTests.Infrastructure
{
    [CollectionDefinition(nameof(ApiCollection))]
    public class ApiCollection : ICollectionFixture<TestHostFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}