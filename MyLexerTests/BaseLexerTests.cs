using System;
using System.Collections.Generic;
using System.Linq;
using MyLexer;
using NUnit.Framework;

namespace MyLexerTests
{
    public class BaseLexerTests
    {
        protected void TestTokenResults(IEnumerable<ParseResult> expectedResults, IEnumerable<ParseResult> actualResults, bool checkPositionDetails = false)
        {
            Assert.IsNotNull(actualResults, "Resulting enumerable was null");

            var expectedArray = expectedResults.ToArray();
            var actualArray = actualResults.ToArray();
            var maxCount = Math.Max(expectedArray.Length, actualArray.Length);

            for (int x = 0; x < maxCount; x++)
            {
                Assert.IsTrue(expectedArray.Length > x, "Expected {0} tokens but {1} was returned", expectedArray.Length,
                              actualArray.Length);

                Assert.IsTrue(actualArray.Length > x, "Expected {0} tokens but {1} was returned", expectedArray.Length,
                              actualArray.Length);

                var expected = expectedArray[x];
                var actual = actualArray[x];

                Assert.AreEqual(expected.Value, actual.Value, "Token {0} had an incorrect value", x);
                Assert.AreEqual(expected.IsValidToken, actual.IsValidToken, "Token {0} had an incorrect valid state", x);
                Assert.AreEqual(expected.TokenType, actual.TokenType, "Token {0} had an incorrect token type", x);

                if (checkPositionDetails)
                {
                    Assert.AreEqual(expected.StartLineIndex, actual.StartLineIndex,
                                    "Token {0} had an incorrect starting line index", x);

                    Assert.AreEqual(expected.StartCharacterIndex, actual.StartCharacterIndex,
                                    "Token {0} had an incorrect starting character index", x);
                }
            }
        }
    }
}
