namespace DotMonkey.Parser.Object;

public struct NULL : IObject
{
    public string Inspect() => "NULL";

    public string Type() => ObjectType.NULL_OBJ;
}
