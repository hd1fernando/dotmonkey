using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Text;

namespace DotMonkey.Parser.AST.Statements;

public class ReturnStatement : IStatement
{
    public Token Token { get; set; }
    public IExpression ReturnValue { get; set; }

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

        if (ReturnValue is not null)
        {
            sb.AppendLine(ReturnValue.String());
        }

        sb.AppendLine(";");

        return sb.ToString();
    }
}
