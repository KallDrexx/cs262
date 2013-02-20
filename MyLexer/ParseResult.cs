namespace MyLexer
{
    public class ParseResult
    {
        public bool IsValidToken { get; set; }
        public string TokenType { get; set; }
        public string Value { get; set; }
        public int StartLineIndex { get; set; }
        public int StartCharacterIndex { get; set; }
    }
}
