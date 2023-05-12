namespace DotMonkey.Parser.AST
{
    /// <summary>
    /// Expressions produces value.
    /// </summary>
    public interface IExpression : INode
    {
        //INode Node { get; }
        void ExpressionNode();
    }
}
