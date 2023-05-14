namespace DotMonkey.Parser.AST.Interfaces;

/// <summary>
/// Statments don't produces value.
/// </summary>
public interface IStatement : INode
{
    void StatementNode();
}
