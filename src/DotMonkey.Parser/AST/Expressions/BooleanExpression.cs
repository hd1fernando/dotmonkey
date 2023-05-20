using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;

namespace DotMonkey.Parser.AST.Expressions;

public class BooleanExpression : IExpression
{
    public Token Token { get; init; }
    public bool Value { get; init; }

    public BooleanExpression(Token token, bool value)
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
