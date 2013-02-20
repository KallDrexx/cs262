namespace MyLexer.TokenDefinitions.Html
{
    public class TagClose : BaseTokenDefinition
    {
        public override string RegexString { get { return ">"; } }
    }
}
