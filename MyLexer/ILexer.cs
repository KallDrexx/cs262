using System.Collections.Generic;
using MyLexer.TokenDefinitions;

namespace MyLexer
{
    public interface ILexer
    {
        TokenDefinition[] TokenDefinitions { set; }
        bool IgnoreWhitespace { get; set; }
        IEnumerable<ParseResult> Tokenize(string stringToParse);
    }
}