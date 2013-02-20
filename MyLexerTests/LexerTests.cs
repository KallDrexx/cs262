using System;
using System.Linq;
using MyLexer;
using MyLexer.TokenDefinitions;
using NUnit.Framework;

namespace MyLexerTests
{
    [TestFixture]
    public class LexerTests
    {
        private Lexer _lexer;

        [SetUp]
        public void Setup()
        {
            _lexer = new Lexer();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsExceptionWhenNoTokensProvided()
        {
            _lexer.Tokenize("abc");
        }

        [Test]
        public void LexerReturnsTokensBasedOnTokenDefinition()
        {
            var tokenDefinition = new TokenDefinition {RegexPattern = @"\d+", TokenName = "TestToken"};
            _lexer.TokenDefinitions = new[] {tokenDefinition};

            var results = _lexer.Tokenize("1234");
            Assert.IsNotNull(results, "Null results returned");

            var parseResults = results as ParseResult[] ?? results.ToArray();
            Assert.AreEqual(1, parseResults.Count(), "Incorrect number of tokens returned");

            var token = parseResults.First();
            Assert.IsTrue(token.IsValidToken, "Token was not marked as valid");
            Assert.AreEqual("TestToken", token.TokenType, "Token type was incorrect");
            Assert.AreEqual("1234", token.Value, "Token value was incorrect");
        }

        [Test]
        public void LexerReturnsTokensBasedOnOrderOfDefinitions()
        {
            var tokenDefinition1 = new TokenDefinition { RegexPattern = @"\d", TokenName = "TestToken1" };
            var tokenDefinition2 = new TokenDefinition { RegexPattern = @"\d+", TokenName = "TestToken2" };

            _lexer.TokenDefinitions = new[] { tokenDefinition1, tokenDefinition2 };

            var results = _lexer.Tokenize("1234");
            Assert.IsNotNull(results, "Null results returned");

            var parseResults = results as ParseResult[] ?? results.ToArray();
            Assert.AreEqual(4, parseResults.Count(), "Incorrect number of tokens returned");
            
            for (int x = 1; x <= 4; x++)
            {
                var result = parseResults.ElementAt(x - 1);
                Assert.IsTrue(result.IsValidToken, "Token was not marked as valid for result " + (x - 1));
                Assert.AreEqual("TestToken1", result.TokenType, "Token type was incorrect for result " + (x - 1));
                Assert.AreEqual(x.ToString(), result.Value, "Token value was incorrect for result " + (x - 1));
            }
        }

        [Test]
        public void InvalidResultTokenReturnedWhenNoDefinitionMatches()
        {
            var tokenDefinition = new TokenDefinition { RegexPattern = @"\d", TokenName = "TestToken1" };
            _lexer.TokenDefinitions = new[] {tokenDefinition};

            var results = _lexer.Tokenize("012\n456");
            var parseResults = results as ParseResult[] ?? results.ToArray();
            Assert.AreEqual(7, parseResults.Length, "Token count was incorrect");

            for (int x = 0; x < 7; x++)
            {
                var token = parseResults[x];
                if (x == 3)
                {
                    Assert.IsFalse(token.IsValidToken, "Token was incorrectly marked as valid for result " + x);
                    Assert.AreEqual("\n", token.Value, "Token value was incorrect for result " + x);
                }
                else
                {
                    Assert.IsTrue(token.IsValidToken, "Token was incorrectly marked as invalid for result " + x);
                    Assert.AreEqual("TestToken1", token.TokenType, "Token type was incorrect for result " + x);
                    Assert.AreEqual(x.ToString(), token.Value, "Token value was incorrect for result " + x);
                }
            }
        }

        [Test]
        public void TokenResultsShowStartingLineAndCharacterIndexOfTokenInString()
        {
            var tokenDefinition = new TokenDefinition { RegexPattern = @"\d", TokenName = "TestToken1" };
            _lexer.TokenDefinitions = new[] { tokenDefinition };

            var results = _lexer.Tokenize("01\n23\n4");
            var parseResults = results as ParseResult[] ?? results.ToArray();

            var token = parseResults[0];
            Assert.AreEqual(0, token.StartLineIndex, "First token line index was incorrect");
            Assert.AreEqual(0, token.StartCharacterIndex, "First token character index was incorrect");
            token = parseResults[1];
            Assert.AreEqual(0, token.StartLineIndex, "Second token line index was incorrect");
            Assert.AreEqual(1, token.StartCharacterIndex, "Second token character index was incorrect");
            token = parseResults[2];
            Assert.AreEqual(0, token.StartLineIndex, "Third token line index was incorrect");
            Assert.AreEqual(2, token.StartCharacterIndex, "Third token character index was incorrect");
            token = parseResults[3];
            Assert.AreEqual(1, token.StartLineIndex, "Fourth token line index was incorrect");
            Assert.AreEqual(0, token.StartCharacterIndex, "Fourth token character index was incorrect");
            token = parseResults[4];
            Assert.AreEqual(1, token.StartLineIndex, "Fifth token line index was incorrect");
            Assert.AreEqual(1, token.StartCharacterIndex, "Fifth token character index was incorrect");
            token = parseResults[5];
            Assert.AreEqual(1, token.StartLineIndex, "Sixth token line index was incorrect");
            Assert.AreEqual(2, token.StartCharacterIndex, "Sixth token character index was incorrect");
            token = parseResults[6];
            Assert.AreEqual(2, token.StartLineIndex, "Seventh token line index was incorrect");
            Assert.AreEqual(0, token.StartCharacterIndex, "Seventh token character index was incorrect");
        }

        [Test]
        public void IgnoresWhiteSpaceWhenSet()
        {
            var tokenDefinition = new TokenDefinition { RegexPattern = @"\d", TokenName = "TestToken1" };
            _lexer.TokenDefinitions = new[] { tokenDefinition };

            _lexer.IgnoreWhitespace = true;

            var results = _lexer.Tokenize("12 3");
            var parseResults = results as ParseResult[] ?? results.ToArray();

            Assert.AreEqual(3, parseResults.Length, "Token count was incorrect");
            Assert.AreEqual("1", parseResults[0].Value, "First token value was incorrect");
            Assert.AreEqual("2", parseResults[1].Value, "Second token value was incorrect");
            Assert.AreEqual("3", parseResults[2].Value, "Third token value was incorrect");
        }

        [Test]
        public void WhiteSpaceNotIgnoredWhenNotSet()
        {
            var tokenDefinition = new TokenDefinition { RegexPattern = @"\d", TokenName = "TestToken1" };
            _lexer.TokenDefinitions = new[] { tokenDefinition };

            _lexer.IgnoreWhitespace = false;

            var results = _lexer.Tokenize("12 3");
            var parseResults = results as ParseResult[] ?? results.ToArray();

            Assert.AreEqual(4, parseResults.Length, "Token count was incorrect");
            Assert.AreEqual("1", parseResults[0].Value, "First token value was incorrect");
            Assert.AreEqual("2", parseResults[1].Value, "Second token value was incorrect");
            Assert.AreEqual(" ", parseResults[2].Value, "Third token value was incorrect");
            Assert.AreEqual("3", parseResults[3].Value, "Fourth token value was incorrect");
        }

        [Test]
        public void TabCountedAsWhitespace()
        {
            var tokenDefinition = new TokenDefinition { RegexPattern = @"\d", TokenName = "TestToken1" };
            _lexer.TokenDefinitions = new[] { tokenDefinition };

            _lexer.IgnoreWhitespace = true;

            var results = _lexer.Tokenize("12\t3");
            var parseResults = results as ParseResult[] ?? results.ToArray();

            Assert.AreEqual(3, parseResults.Length, "Token count was incorrect");
            Assert.AreEqual("1", parseResults[0].Value, "First token value was incorrect");
            Assert.AreEqual("2", parseResults[1].Value, "Second token value was incorrect");
            Assert.AreEqual("3", parseResults[2].Value, "Third token value was incorrect");
        }
    }
}