using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class IndexExpression : IExpression
{
    public IndexExpression(Token token, IExpression left)
    {
        Token = token;
        Left = left;
    }

    public Token Token { get; init; }
    public IExpression Left { get; set; }
    public IExpression Index { get; set; }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();

        sb.Append("(");
        sb.Append(Left.String());
        sb.Append("[");
        sb.Append(Index.String());
        sb.Append("])");

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
