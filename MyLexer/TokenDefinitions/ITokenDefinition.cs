namespace MyLexer.TokenDefinitions
{
    public interface ITokenDefinition
    {
        string RegexString { get; }
        string TokenName { get; }
    }
}
