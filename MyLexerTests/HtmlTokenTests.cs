using System;
using System.Linq;
using Moq;
using MyLexer;
using MyLexer.TokenDefinitions;
using MyLexer.TokenDefinitions.Html;
using NUnit.Framework;

namespace MyLexerTests
{
    [TestFixture]
    public class HtmlTokenTests : BaseLexerTests
    {
        private HtmlTokenDefinitions _htmlDefinitions;
        private Lexer _realLexer;

        [SetUp]
        public void Setup()
        {
            _htmlDefinitions = new HtmlTokenDefinitions();
            _realLexer = new Lexer();

            _htmlDefinitions.SetupLexer(_realLexer);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptionThrownIfNullLexerIsSetup()
        {
            _htmlDefinitions.SetupLexer(null);
        }

        [Test]
        public void DefinitionSetsLexerToIgnoreWhitespace()
        {
            var mockedLexer = new Mock<ILexer>();
            _htmlDefinitions.SetupLexer(mockedLexer.Object);

            mockedLexer.VerifySet(x => x.IgnoreWhitespace = true);
        }

        [Test]
        public void HtmlTokenDefinitionsPassedToLexer()
        {
            var mockedLexer = new Mock<ILexer>();
            _htmlDefinitions.SetupLexer(mockedLexer.Object);

            mockedLexer.VerifySet(x => x.TokenDefinitions = It.IsAny<TokenDefinition[]>());
        }

        [Test]
        public void CanTokenizeBoldedHtml()
        {
            const string test = "<b>Test</b>";
            var tokens = _realLexer.Tokenize(test).ToArray();

            var expected = new[]
            {
                new ParseResult
                {
                    IsValidToken = true,
                    TokenType = HtmlTokenDefinitions.StartTagOpenTokenType,
                    Value = "<"
                },

                new ParseResult {IsValidToken = true, TokenType = HtmlTokenDefinitions.WordTokenType, Value = "b"},
                new ParseResult {IsValidToken = true, TokenType = HtmlTokenDefinitions.TagCloseTokenType, Value = ">"},
                new ParseResult {IsValidToken = true, TokenType = HtmlTokenDefinitions.WordTokenType, Value = "Test"},
                new ParseResult
                {
                    IsValidToken = true,
                    TokenType = HtmlTokenDefinitions.EndTagOpenTokenType,
                    Value = "</"
                },

                new ParseResult {IsValidToken = true, TokenType = HtmlTokenDefinitions.WordTokenType, Value = "b"},
                new ParseResult {IsValidToken = true, TokenType = HtmlTokenDefinitions.TagCloseTokenType, Value = ">"},
            };

            TestTokenResults(expected, tokens);
        }
    }
}
