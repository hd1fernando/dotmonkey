namespace DotMonkey.Parser.Object;

public struct _String : IObject
{
    public string Value { get; init; }

    public _String(string value)
    {
        Value = value;
    }

    public string Inspect() => Value;

    public string Type() => ObjectType.STRING_OBJ;
}
