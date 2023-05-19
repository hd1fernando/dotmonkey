using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class InfixExpression : IExpression
{
  
    public Token Token { get; init; }
    public IExpression Left { get; init; }
    public string Operator { get; init; }
    public IExpression Rigth { get; set; }

    public InfixExpression(Token token, IExpression left, string @operator)
    {
        Token = token;
        Left = left;
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
        sb.Append(Left.String());
        sb.Append(" " + Operator + " ");
        sb.Append(Rigth.String());
        sb.Append(")");

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
