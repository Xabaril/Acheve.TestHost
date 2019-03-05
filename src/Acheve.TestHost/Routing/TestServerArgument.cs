namespace Acheve.TestHost.Routing
{
    public class TestServerArgument
    {
        public TestServerArgument(object instance, bool isFromBody = false, bool isFromForm = false)
        {
            Instance = instance;
            IsFromBody = isFromBody;
            IsFromForm = isFromForm;
        }

        public object Instance { get; private set; }

        public bool IsFromBody { get; private set; }
        public bool IsFromForm { get; private set; }
    }
}