using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;

namespace DotMonkey.Parser.AST.Expressions;

public class IntegerLiteral : IExpression
{
    public Token Token { get; init; }
    public int Value { get; set; }

    public IntegerLiteral(Token token)
    {
        Token = token;
    }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String() => Token.Literal;


    public string TokenLiteral() => Token.Literal;
}