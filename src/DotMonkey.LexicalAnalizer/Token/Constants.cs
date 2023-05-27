namespace DotMonkey.LexicalAnalizer
{
    public static class Constants
    {
        // Special types
        public const string ILLEGAL = nameof(ILLEGAL); // A token or charachter we don't know.
        public const string EOF = nameof(EOF);

        // Identifiers + Literals
        public const string IDENT = nameof(IDENT);
        public const string INT = nameof(INT);

        // Operators
        public const string ASSING = "=";
        public const string PLUS = "+";
        public const string MINUS = "-";
        public const string BANG = "!";
        public const string ASTERISK = "*";
        public const string SLASH = "/";

        public const string EQ = "==";
        public const string NOT_EQ = "!=";

        public const string LT = "<";
        public const string GT = ">";

        // Delimiters
        public const string COMMA = ",";
        public const string SEMICOLON = ";";

        public const string LPARENT = "(";
        public const string RPARENT = ")";
        public const string LBRACE = "{";
        public const string RBRACE = "}";

        // Keywords
        public const string FUNCTION = nameof(FUNCTION);
        public const string LET = nameof(LET);
        public const string TRUE = nameof(TRUE);
        public const string FALSE = nameof(FALSE);
        public const string IF = nameof(IF);
        public const string ELSE = nameof(ELSE);
        public const string RETURN = nameof(RETURN);

        public const string STRING = "STRING";  
    }
}
