using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;

namespace DotMonkey.Parser.AST.Statements;

public class LetStatement : IStatement
{
    public Token Token { get; init; }
    /// <summary>
    /// Hold the identifier of the binding.
    /// </summary>
    public Identifier Name { get; init; }
    /// <summary>
    /// Expression that produces the value.
    /// </summary>
    public IExpression Value { get; init; }

    public void StatementNode()
    {
        throw new System.NotImplementedException();
    }

    public string TokenLiteral()
        => Token.Literal;
}
