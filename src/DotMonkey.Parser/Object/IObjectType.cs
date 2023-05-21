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
    public const string BOOLEAN_OBJ = "BOOLEAN";
    public const string NULL_OBJ = "NULL";
}

public interface IObject
{
    public ObjectType Type();
    public string Inspect();
}

public struct NULL : IObject
{
    public string Inspect() => "NULL";

    public ObjectType Type() => ObjectType.NULL_OBJ;
}

public struct Boolean : IObject
{
    public bool Value { get; private set; }

    public Boolean(bool value)
    {
        Value = value;
    }

    public static implicit operator Boolean(bool value) => new(value);

    public string Inspect() => Value.ToString();

    public ObjectType Type() => ObjectType.BOOLEAN_OBJ;
}

public struct Interger : IObject
{
    public long Value { get; private set; }

    private Interger(long value)
    {
        Value = value;
    }

    public static implicit operator Interger(long value) => new(value);

    public string Inspect() => Value.ToString();

    public ObjectType Type() => ObjectType.INTERGER_OBJ;
}
