namespace DotMonkey.Parser.AST.Interfaces;

public interface INode
{
    /// <summary>
    /// Used only for debbugin or and testing.
    /// </summary>
    /// <returns>Literal value of the token it's associated with.</returns>
    public string TokenLiteral();

    public string String();
}
