using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;

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
}
