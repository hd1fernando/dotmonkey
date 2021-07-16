namespace DotMonkey.LexicalAnalizer
{
    public static class Constants
    {
        public const string ILLEGAL = nameof(ILLEGAL);
        public const string EOF = nameof(EOF);

        public const string IDENT = nameof(IDENT);
        public const string INT = nameof(INT);

        public const string ASSING = "=";
        public const string PLUS = "+";
        public const string MINUS = "-";
        public const string BANG = "!";
        public const string ASTERISK = "*";
        public const string SLASH = "/";

        public const string LT = "<";
        public const string GT = ">";

        public const string COMMA = ",";
        public const string SEMICOLON = ";";

        public const string LPARENT = "(";
        public const string RPARENT = ")";
        public const string LBRACE = "{";
        public const string RBRACE = "}";

        public const string FUNCTION = nameof(FUNCTION);
        public const string LET = nameof(LET);
    }
}
