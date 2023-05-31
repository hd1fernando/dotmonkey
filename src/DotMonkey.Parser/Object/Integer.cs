namespace DotMonkey.Parser.Object;

public struct Integer : IObject
{
    public long Value { get; private set; }

    public Integer(long value)
    {
        Value = value;
    }

    public static implicit operator Integer(long value) => new(value);

    public string Inspect() => Value.ToString();

    public string Type() => ObjectType.INTERGER_OBJ;

    public HashKey HashKey()
    {
        return new HashKey
        {
            Type = Type(),
            Value = (ulong)Value
        };
    }
}
