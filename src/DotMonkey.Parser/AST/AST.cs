using DotMonkey.LexicalAnalizer;
using System;

namespace DotMonkey.Parser.AST
{
    public interface INode
    {
        /// <summary>
        /// Used only for debbugin or and testing.
        /// </summary>
        /// <returns>Literal value of the token it's associated with.</returns>
        public string TokenLiteral();
    }

    public class Parser
    {
        public Lexer Lexer { get; init; }
        public Token CurrentToken { get; private set; }
        public Token PeekToken { get; private set; }

        public Parser(Lexer lexer)
        {
            Lexer = lexer;

            // Read two tokens, so CurrentToken and PeekToken are both set.
            NextToken();
            NextToken();
        }

        private void NextToken()
        {
            CurrentToken = PeekToken;
            PeekToken = Lexer.NextToken();
        }

        public Program ParserProgram()
        {
            return null;
        }
    }
}
