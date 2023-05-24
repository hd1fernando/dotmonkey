using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Statements;
using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.Object;


public struct ObjectType
{
    public const string INTERGER_OBJ = "INTEGER";
    public const string BOOLEAN_OBJ = "BOOLEAN";
    public const string NULL_OBJ = "NULL";
    public const string RETURN_VALUE_OBJ = "RETURN_VALUE";
    public const string ERROR_OBJ = "ERROR";
    public const string FUNCTION_OBJ = "FUNCTION";
}

public interface IObject
{
    public string Type();
    public string Inspect();
}

public struct Function : IObject
{
    public List<Identifier> Parameters { get; private set; }
    public BlockStatement Body { get; private set; }
    public Environment Environment { get; private set; }

    public Function(List<Identifier> parameters, BlockStatement body, Environment environment)
    {
        Parameters = parameters;
        Body = body;
        Environment = environment;
    }

    public string Inspect()
    {
        var sb = new StringBuilder();
        var @params = new List<string>();

        foreach (var param in Parameters)
            @params.Add(param.String());

        sb.Append("fn");
        sb.Append("(");
        sb.Append(string.Join(",", @params));
        sb.AppendLine(")");
        sb.AppendLine(Body.String());

        return sb.ToString();
    }


    public string Type() => ObjectType.FUNCTION_OBJ;
}
