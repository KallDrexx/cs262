using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MyLexer.TokenDefinitions;

namespace MyLexer
{
    public class Lexer
    {
        public TokenDefinition[] TokenDefinitions { private get; set; }

        public bool IgnoreWhitespace { get; set; }

        public IEnumerable<ParseResult> Tokenize(string stringToParse)
        {
            if (TokenDefinitions == null || TokenDefinitions.Length == 0)
                throw new InvalidOperationException("No tokens set to match on");

            var results = new List<ParseResult>();
            var lineIndex = 0;
            var columnIndex = 0;

            while (stringToParse.Length > 0)
            {
                bool validTokenFound = false;
                foreach (var definition in TokenDefinitions)
                {
                    var regex = definition.RegexPattern;
                    var match = Regex.Match(stringToParse, regex);
                    if (!match.Success || match.Index != 0)
                        continue;

                    results.Add(new ParseResult
                    {
                        IsValidToken = true,
                        TokenType = definition.TokenName,
                        Value = match.Value,
                        StartLineIndex = lineIndex,
                        StartCharacterIndex = columnIndex
                    });

                    // Calculate the line and column for the next token
                    var lengthToNextToken = match.Value.Length;
                    var newlineMatches = Regex.Matches(match.Value, "\n");
                    if (newlineMatches.Count > 0)
                    {
                        lineIndex += newlineMatches.Count;
                        columnIndex = 0;

                        var lastMatch = newlineMatches[newlineMatches.Count - 1];
                        lengthToNextToken = match.Value.Length - lastMatch.Index + 1;
                    }

                    columnIndex += lengthToNextToken;

                    stringToParse = stringToParse.Substring(match.Value.Length);
                    validTokenFound = true;
                    break;
                }

                if (!validTokenFound)
                {
                    if (IgnoreWhitespace && (stringToParse[0] == ' ' || stringToParse[0] == '\t'))
                    {
                        columnIndex++;
                    }
                    else
                    {
                        results.Add(new ParseResult
                        {
                            IsValidToken = false,
                            TokenType = string.Empty,
                            Value = stringToParse[0].ToString(),
                            StartLineIndex = lineIndex,
                            StartCharacterIndex = columnIndex
                        });

                        if (stringToParse[0] == '\n')
                        {
                            lineIndex++;
                            columnIndex = 0;
                        }
                        else
                        {
                            columnIndex++;
                        }
                    }

                    stringToParse = stringToParse.Substring(1);
                }
            }

            return results;
        }
    }
}
