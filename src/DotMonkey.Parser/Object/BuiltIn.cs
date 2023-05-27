namespace DotMonkey.Parser.Object;

public class BuiltIn : IObject
{
    public ObjectType.BuiltInFunction Fn;

    public BuiltIn(ObjectType.BuiltInFunction fn)
    {
        Fn = fn;
    }

    public string Inspect() => "builtin function";

    public string Type() => ObjectType.BUILTIN_OBJ;
}
