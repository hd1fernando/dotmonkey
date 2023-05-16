using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;

namespace DotMonkey.Parser.AST;

/// <summary>
/// Represents the name in a varible binding.
/// </summary>
public class Identifier : IExpression
{
    public Token Token { get; init; } // The token.IDENT token
    public string Value { get; init; }

    public Identifier(Token token, string value)
    {
        Token = token;
        Value = value;
    }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string TokenLiteral()
        => Token.Literal;

    public string String()
    {
        return Value;
    }
}
