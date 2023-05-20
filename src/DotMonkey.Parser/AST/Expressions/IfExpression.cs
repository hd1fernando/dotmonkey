using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class IfExpression : IExpression
{
    public Token Token { get; init; }

    public IExpression Condition { get; set; }
    public BlockStatement Consequence { get; set; }
    public BlockStatement Alternative { get; set; }

    public IfExpression(Token token)
    {
        Token = token;
    }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();
        sb.Append("if");
        sb.Append(Condition.String());
        sb.Append(" ");
        sb.Append(Consequence.String());

        if (Alternative is not null)
        {
            sb.Append("else");
            sb.Append(Alternative.String());
        }

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
