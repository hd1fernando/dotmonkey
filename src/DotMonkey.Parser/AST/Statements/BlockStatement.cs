using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DotMonkey.Parser.AST.Statements;

public class BlockStatement : IStatement
{
    public Token Token { get; set; } // the { token
    public List<IStatement> Statements { get; private set; } = new List<IStatement>();

    public BlockStatement(Token token)
    {
        Token = token;
    }

    public void AddStatement(IStatement statement) => Statements.Add(statement);

    public void StatementNode()
    {
        throw new System.NotImplementedException();
    }

    public string String()
    {
        var sb = new StringBuilder();

        foreach (var statement in Statements)
        {
            sb.Append(statement.String());
        }

        return sb.ToString();
    }

    public string TokenLiteral() => Token.Literal;


}
