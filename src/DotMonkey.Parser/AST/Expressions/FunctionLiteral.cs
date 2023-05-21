using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.AST.Expressions;

public class FunctionLiteral : IExpression
{
    public Token Token { get; init; }   
    public List<Identifier> Parameters { get; private set; } = new List<Identifier>();
    public BlockStatement Body { get; set; }

    public FunctionLiteral(Token token)
    {
        Token = token;
    }

    public void AddParameter(List<Identifier> parameters) => Parameters.AddRange(parameters);

    public void ExpressionNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();

        var @params = new List<string>();

        foreach (var parameter in Parameters)
        {
            @params.Add(parameter.String());
        }

        sb.Append(TokenLiteral());
        sb.Append("(");
        sb.Append(string.Join(",", @params));
        sb.Append(")");
        sb.Append(Body.String());

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;
}
