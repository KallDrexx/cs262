using System;

namespace MyLexer.TokenDefinitions.Html
{
    public class HtmlTokenDefinitions
    {
        public const string StartTagOpenTokenType = "StartTagOpen";
        public const string EndTagOpenTokenType = "EndTagOpen";
        public const string TagCloseTokenType = "TagClose";

        private readonly TokenDefinition[] _tokenDefinitions;

        public HtmlTokenDefinitions()
        {
            _tokenDefinitions = FormTokenDefinitions();
        }

        private TokenDefinition[] FormTokenDefinitions()
        {
            return new TokenDefinition[]
            {

            };
        }

        public void SetupLexer(ILexer lexer)
        {
            if (lexer == null)
                throw new ArgumentNullException("lexer");

            lexer.IgnoreWhitespace = true;
            lexer.TokenDefinitions = _tokenDefinitions;
        }
    }
}
