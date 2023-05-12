namespace DotMonkey.Parser.AST
{
    /// <summary>
    /// Statment don't produces value.
    /// </summary>
    public interface IStatement : INode
    {
        //INode Node { get; }
        void StatementNode();
    }
}
