namespace Acheve.TestHost.Routing
{
    public class TestServerArgument
    {
        public TestServerArgument(
             object instance,
             bool isFromBody = false,
             bool isFromForm = false,
             bool isFromHeader = false,
             string headerName = null)
        {
            Instance = instance; Instance = instance;
            IsFromBody = isFromBody; IsFromBody = isFromBody;
            IsFromForm = isFromForm; IsFromForm = isFromForm;
            IsFromHeader = isFromHeader;
            HeaderName = isFromHeader ? headerName : null;
        }

        public object Instance { get; private set; }

        public bool IsFromBody { get; private set; }
        public bool IsFromForm { get; private set; }
        public bool IsFromHeader { get; private set; }
        public string HeaderName { get; private set; }
    }
}