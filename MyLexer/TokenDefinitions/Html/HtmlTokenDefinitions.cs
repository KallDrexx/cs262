using System;

namespace MyLexer.TokenDefinitions.Html
{
    public class HtmlTokenDefinitions
    {
        public const string StartTagOpenTokenType = "StartTagOpen";
        public const string EndTagOpenTokenType = "EndTagOpen";
        public const string TagCloseTokenType = "TagClose";
        public const string WordTokenType = "Word";

        private readonly TokenDefinition[] _tokenDefinitions;

        public HtmlTokenDefinitions()
        {
            _tokenDefinitions = FormTokenDefinitions();
        }

        public void SetupLexer(ILexer lexer)
        {
            if (lexer == null)
                throw new ArgumentNullException("lexer");

            lexer.IgnoreWhitespace = true;
            lexer.TokenDefinitions = _tokenDefinitions;
        }

        private TokenDefinition[] FormTokenDefinitions()
        {
            return new[]
            {
                new TokenDefinition { RegexPattern = "</", TokenName = EndTagOpenTokenType },
                new TokenDefinition { RegexPattern = "<", TokenName = StartTagOpenTokenType },
                new TokenDefinition { RegexPattern = ">", TokenName = TagCloseTokenType },
                new TokenDefinition { RegexPattern = @"[a-zA-Z]+", TokenName = WordTokenType }
            };
        }
    }
}
