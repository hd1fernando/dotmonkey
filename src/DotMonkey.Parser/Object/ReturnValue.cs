namespace DotMonkey.Parser.Object;

public struct ReturnValue : IObject
{
    public IObject Value { get; private set; }

    public ReturnValue(IObject value)
    {
        Value = value;
    }

    public string Inspect() => Value.Inspect();

    public string Type() => ObjectType.RETURN_VALUE_OBJ;
}
