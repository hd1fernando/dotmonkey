using System.Linq;
using System.Text;

namespace DotMonkey.Parser.Object;

public struct _String : IObject, IHashTable
{
    public string Value { get; init; }

    public _String(string value)
    {
        Value = value;
    }

    public string Inspect() => Value;

    public string Type() => ObjectType.STRING_OBJ;

    public HashKey HashKey()
    {
        var bytes = Encoding.UTF8.GetBytes(Value);
        var sum = bytes.Sum(_ => (int)_);
        return new HashKey
        {
            Type = Type(),
            Value = (ulong)sum,
        };
    }
}
