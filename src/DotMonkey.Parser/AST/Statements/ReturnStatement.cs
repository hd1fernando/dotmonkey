using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Text;

namespace DotMonkey.Parser.AST.Statements;

public class ReturnStatement : IStatement
{
    public Token Token { get; set; }
    public IExpression Value { get; init; }

    public ReturnStatement(Token token)
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

        sb.Append(TokenLiteral());

        if (Value is not null)
        {
            sb.AppendLine(Value.String());
        }

        sb.AppendLine(";");

        return sb.ToString();
    }
}
