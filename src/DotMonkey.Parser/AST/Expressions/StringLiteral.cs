using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;

namespace DotMonkey.Parser.AST.Expressions;

public class StringLiteral : IExpression
{
    public Token Token { get; init; }
    public string Value { get; init; }

    public StringLiteral(Token token, string value)
    {
        Token = token;
        Value = value;
    }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String() => Token.Literal;

    public string TokenLiteral() => Token.Literal;
}
