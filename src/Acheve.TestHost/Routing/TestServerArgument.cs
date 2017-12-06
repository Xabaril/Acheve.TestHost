namespace Acheve.TestHost.Routing
{
    public class TestServerArgument
    {
        public TestServerArgument(object instance, bool isBody = false)
        {
            Instance = instance;
            IsBody = isBody;
        }

        public object Instance { get; private set; }

        public bool IsBody { get; private set; }
    }
}