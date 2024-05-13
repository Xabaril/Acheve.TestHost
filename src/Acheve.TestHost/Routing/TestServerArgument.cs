using System;

namespace Acheve.TestHost.Routing
{
    [Flags]
    public enum TestServerArgumentFromType
    {
        None = 0,
        Body = 1,
        Form = 2,
        Header = 4,
        Query = 8,
        Route = 32,
    }

    public class TestServerArgument
    {
        public TestServerArgument(
             object instance,
             TestServerArgumentFromType fromType,
             bool neverBind,
             Type type,
             string name)
        {
            Instance = instance;
            FromType = fromType;
            Name = name;
            NeverBind = neverBind;
            Type = type;
        }

        public object Instance { get; }
        public TestServerArgumentFromType FromType { get; }
        public string Name { get; }
        public bool NeverBind { get; }
        public Type Type { get; }
    }
}