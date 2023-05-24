namespace DotMonkey.Parser.Object;

public struct Error : IObject
{
    public string Message { get; private set; }

    public Error(string message) => Message = message;

    public string Inspect() => "ERROR: " + Message;

    public string Type() => ObjectType.ERROR_OBJ;
}
