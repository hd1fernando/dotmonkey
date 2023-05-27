namespace DotMonkey.Parser.Object;


public struct ObjectType
{
    public const string INTERGER_OBJ = "INTEGER";
    public const string BOOLEAN_OBJ = "BOOLEAN";
    public const string NULL_OBJ = "NULL";
    public const string RETURN_VALUE_OBJ = "RETURN_VALUE";
    public const string ERROR_OBJ = "ERROR";
    public const string FUNCTION_OBJ = "FUNCTION";
    public const string STRING_OBJ = "STRING";
}

public interface IObject
{
    public string Type();
    public string Inspect();
}