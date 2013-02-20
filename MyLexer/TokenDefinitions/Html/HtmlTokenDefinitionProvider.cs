using System;

namespace MyLexer.TokenDefinitions.Html
{
    public static class HtmlTokenDefinitionProvider
    {
        public static void SetupLexer(Lexer lexer)
        {
            if (lexer == null)
                throw new ArgumentNullException("lexer");

            //lexer.TokenDefinitions = GetTokenDefinitions();
            lexer.IgnoreWhitespace = true;
        }

        private static ITokenDefinition[] GetTokenDefinitions()
        {
            return new ITokenDefinition[]
            {
                new StartTagOpen(),
                new EndTagOpen(),
                new TagClose()
            };
        }
    }
}
