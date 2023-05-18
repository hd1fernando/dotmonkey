using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using System.Text;

namespace DotMonkey.Parser.AST.Statements;

public class LetStatement : IStatement
{
    public Token Token { get; init; }
    /// <summary>
    /// Hold the identifier of the binding.
    /// </summary>
    public Identifier Name { get; set; }
    /// <summary>
    /// Expression that produces the value.
    /// </summary>
    public IExpression Value { get; init; }

    public LetStatement(Token token)
    {
        Token = token;
    }

    public void StatementNode()
    {
        throw new System.NotImplementedException();
    }

    public string TokenLiteral() => Token.Literal;

    public string String()
    {
        var sb = new StringBuilder();

        sb.AppendLine(TokenLiteral());
        sb.AppendLine(Name.String());
        sb.AppendLine("=");

        if (Value is not null)
        {
            sb.AppendLine(Value.String());
        }

        sb.AppendLine(";");

        return sb.ToString();
    }
}
