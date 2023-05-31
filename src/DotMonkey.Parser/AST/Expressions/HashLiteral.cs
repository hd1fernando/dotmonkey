using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class HashLiteral : IExpression
{
    public Token Token { get; init; } // the { token
    public Dictionary<IExpression, IExpression> Pairs { get; init; } = new Dictionary<IExpression, IExpression>();

    public HashLiteral(Token token)
    {
        Token = token;
    }

    public void AddToPair(IExpression key, IExpression value)
        => Pairs.Add(key, value);

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();
        var pairs = new List<string>();

        foreach (var pair in Pairs)
        {
            pairs.Add($"{pair.Key}:{pair.Value}");
        }

        sb.Append("{");
        sb.Append(string.Join(',', pairs));
        sb.Append("}");

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
