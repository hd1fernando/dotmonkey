using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class PrefixExpression : IExpression
{
    public Token Token { get; init; }
    public string Operator { get; init; }
    public IExpression Rigth { get; set; }

    public PrefixExpression(Token token, string @operator)
    {
        Token = token;
        Operator = @operator;
    }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();
        sb.Append("(");
        sb.Append(Operator);
        sb.Append(Rigth.String());
        sb.Append(")");

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
