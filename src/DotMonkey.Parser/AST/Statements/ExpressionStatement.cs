using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;

namespace DotMonkey.Parser.AST.Statements;

public class ExpressionStatement : IStatement
{
    public Token Token { get; set; } // The first token of the expression
    public IExpression Expression { get; set; }

    public ExpressionStatement(Token token)
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
        if (Expression is not null)
            return Expression.String();

        return string.Empty;
    }
}
