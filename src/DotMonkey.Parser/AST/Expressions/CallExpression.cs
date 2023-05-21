using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class CallExpression : IExpression
{

    public Token Token { get; init; }
    public IExpression Function { get; init; }
    public List<IExpression> Arguments { get; private set; } = new List<IExpression>();

    public CallExpression(Token token, IExpression function, List<IExpression> arguments)
    {
        Token = token;
        Function = function;
        Arguments = arguments;
    }

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();
        var args = new List<string>();

        foreach (var arg in Arguments)
        {
            args.Add(arg.String());
        }

        sb.Append(Function.String());
        sb.Append("(");
        sb.Append(string.Join(",", args));
        sb.Append(")");

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
