namespace Acheve.TestHost.Routing
{
    public class TestServerArgument
    {
        public TestServerArgument(object instance, bool isBody = false, bool isForm = false)
        {
            Instance = instance;
            IsBody = isBody;
            IsForm = isForm;
        }

        public object Instance { get; private set; }

        public bool IsBody { get; private set; }
        public bool IsForm { get; private set; }
    }
}