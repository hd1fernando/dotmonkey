namespace DotMonkey.Parser.Object;


public struct ObjectType
{
    public string Value { get; private set; }

    private ObjectType(string value)
    {
        Value = value;
    }

    public static implicit operator ObjectType(string value) => new(value);

    public const string INTERGER_OBJ = "INTERGER";
}

public interface IObject
{
    public ObjectType Type();
    public string Inspect();
}

public struct Interger : IObject
{
    public long Value { get; private set; }

    public Interger(long value)
    {
        Value = value;
    }

    public static implicit operator Interger(long value) => new(value);

    public string Inspect() => Value.ToString();

    public ObjectType Type()
    {
        return ObjectType.INTERGER_OBJ;
    }
}
