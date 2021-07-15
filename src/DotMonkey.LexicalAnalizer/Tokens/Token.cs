namespace DotMonkey.LexicalAnalizer.Tokens
{
    public struct Token
    {
        public string Type { get; }
        public string Literal { get; }

        public Token(string type, string literal)
        {
            Type = type;
            Literal = literal;
        }
        


    }
}
