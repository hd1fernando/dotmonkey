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
    public IExpression Value { get; set; }

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

        sb.Append(TokenLiteral()+" ");
        sb.Append(Name.String());
        sb.Append(" = ");

        if (Value is not null)
        {
            sb.Append(Value.String());
        }

        sb.Append(";");

        return sb.ToString();
    }
}
