using System;
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

        [SetUp]
        public void Setup()
        {
            _htmlDefinitions = new HtmlTokenDefinitions();
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
    }
}
