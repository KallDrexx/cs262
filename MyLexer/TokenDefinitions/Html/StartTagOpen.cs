namespace MyLexer.TokenDefinitions.Html
{
    public class StartTagOpen : BaseTokenDefinition
    {
        public override string RegexString { get { return "<"; } }
    }
}
