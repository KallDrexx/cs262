namespace MyLexer.TokenDefinitions.Html
{
    public class EndTagOpen : BaseTokenDefinition
    {
        public override string RegexString { get { return "</"; } }
    }
}
