using DotMonkey.LexicalAnalizer;

namespace DotMonkey.Parser.AST
{
    /// <summary>
    /// Represents the name in a varible binding.
    /// </summary>
    public class Identifier : IExpression
    {
        public Token Token { get; init; }
        public string Value { get; init; }


        public void ExpressionNode()
        {
            throw new System.NotImplementedException();
        }

        public string TokenLiteral()
            => Token.Literal;
    }
}
