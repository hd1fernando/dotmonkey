using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class ArrayLiteral : IExpression
{
    public Token Token { get; init; }
    public List<IExpression> Elements { get; init; }

    public ArrayLiteral(Token token, List<IExpression> elements)
    {
        Token = token;
        Elements = elements;
    }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();
        List<string> elements = new();

        foreach(var element in Elements)
        {
            elements.Add(element.String());
        }

        sb.Append("[");
        sb.Append(string.Join(", ", elements));
        sb.Append("]");

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
