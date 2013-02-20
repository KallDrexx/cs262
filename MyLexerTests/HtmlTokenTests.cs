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
    public class HtmlTokenTests
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

            Assert.AreEqual(7, tokens.Length, "Incorrect number of tokens returned");

            var token = tokens[0];
            Assert.AreEqual(HtmlTokenDefinitions.StartTagOpenTokenType, token.TokenType, "First token's type was incorrect");
            Assert.AreEqual("<", token.Value, "First token's value was incorrect");

            token = tokens[1];
            Assert.AreEqual(HtmlTokenDefinitions.WordTokenType, token.TokenType, "Second token's type was incorrect");
            Assert.AreEqual("b", token.Value, "Second token's value was incorrect");

            token = tokens[2];
            Assert.AreEqual(HtmlTokenDefinitions.TagCloseTokenType, token.TokenType, "Third token's type was incorrect");
            Assert.AreEqual(">", token.Value, "Third token's value was incorrect");

            token = tokens[3];
            Assert.AreEqual(HtmlTokenDefinitions.WordTokenType, token.TokenType, "Fourth token's type was incorrect");
            Assert.AreEqual("Test", token.Value, "Fourth token's value was incorrect");

            token = tokens[4];
            Assert.AreEqual(HtmlTokenDefinitions.EndTagOpenTokenType, token.TokenType, "Fifth token's type was incorrect");
            Assert.AreEqual("</", token.Value, "Fifth token's value was incorrect");

            token = tokens[5];
            Assert.AreEqual(HtmlTokenDefinitions.WordTokenType, token.TokenType, "Sixth token's type was incorrect");
            Assert.AreEqual("b", token.Value, "Sixth token's value was incorrect");

            token = tokens[6];
            Assert.AreEqual(HtmlTokenDefinitions.TagCloseTokenType, token.TokenType, "Seventh token's type was incorrect");
            Assert.AreEqual(">", token.Value, "Seventh token's value was incorrect");
        }
    }
}
