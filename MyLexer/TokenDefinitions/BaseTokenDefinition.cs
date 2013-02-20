namespace MyLexer.TokenDefinitions
{
    public abstract class BaseTokenDefinition : ITokenDefinition
    {
        public abstract string RegexString { get; }
        public virtual string TokenName { get { return GetType().ToString(); } }
    }
}
