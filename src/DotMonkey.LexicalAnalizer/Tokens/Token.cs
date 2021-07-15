using System.Collections.Generic;

namespace DotMonkey.LexicalAnalizer.Tokens
{
    public struct Token
    {
        public string Type { get; }
        public string Literal { get; }
        public IDictionary<string, string> Keywords { get; }

        public Token(string type, string literal)
        {
            Type = type;
            Literal = literal;

            Keywords = new Dictionary<string, string>
            {
                {"fn",Constants.FUNCTION },
                {"let",Constants.LET }
            };
        }

        public string LookupIdent()
         => (Keywords.TryGetValue(Literal, out var result))
            ? result
            : Constants.IDENT;

    }
}
