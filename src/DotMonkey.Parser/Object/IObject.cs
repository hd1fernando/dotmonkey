using System.Collections.Generic;

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
    public const string BUILTIN_OBJ = "BUILTIN";
    public const string ARRAY_OBJ = "ARRAY";

    public delegate IObject BuiltInFunction(List<IObject> args);

}

public interface IObject
{
    public string Type();
    public string Inspect();
}