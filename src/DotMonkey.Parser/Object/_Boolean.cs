using System.Diagnostics.CodeAnalysis;

namespace DotMonkey.Parser.Object;

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
        if (obj is not _Boolean)
            return false;

        var o = (_Boolean)obj;

        return o.Value == Value;
    }
}
