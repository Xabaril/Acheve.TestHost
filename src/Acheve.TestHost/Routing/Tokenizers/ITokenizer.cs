namespace Acheve.TestHost.Routing.Tokenizers
{
    public interface ITokenizer
    {
        void AddTokens<TController>(TestServerAction action,TestServerTokenCollection tokens)
            where TController : class;
    }
}
