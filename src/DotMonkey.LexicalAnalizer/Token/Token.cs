using System.Collections.Generic;

namespace DotMonkey.LexicalAnalizer
{
    public class Token
    {
        public string Type { get; }
        public string Literal { get; set; }
        public IDictionary<string, string> Keywords { get; }

        public Token(string type, string literal) : this()
        {
            Type = type;
            Literal = literal;

        }

        public Token()
        {
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
