namespace DotMonkey.Parser.Object;


public struct ObjectType
{
    public const string INTERGER_OBJ = "INTERGER";
    public const string BOOLEAN_OBJ = "BOOLEAN";
    public const string NULL_OBJ = "NULL";
    public const string RETURN_VALUE_OBJ = "RETURN_VALUE";

}

public interface IObject
{
    public string Type();
    public string Inspect();
}

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
