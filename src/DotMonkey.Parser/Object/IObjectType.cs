using System.Diagnostics.CodeAnalysis;

namespace DotMonkey.Parser.Object;


public struct ObjectType
{  
    public const string INTERGER_OBJ = "INTERGER";
    public const string BOOLEAN_OBJ = "BOOLEAN";
    public const string NULL_OBJ = "NULL";
}

public interface IObject
{
    public string Type();
    public string Inspect();
}

public struct NULL : IObject
{
    public string Inspect() => "NULL";

    public string Type() => ObjectType.NULL_OBJ;
}

public struct _Boolean : IObject
{
    public bool Value { get; private set; }

    public _Boolean(bool value)
    {
        Value = value;
    }

    public static implicit operator _Boolean(bool value) => new(value);

    public string Inspect() => Value.ToString();

    public string Type() => ObjectType.BOOLEAN_OBJ;

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if(obj is not _Boolean)
            return false;

        var o = (_Boolean)obj;

        return o.Value == Value;
    }
}

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
}
