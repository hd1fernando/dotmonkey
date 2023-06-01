using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.Object;

public struct Hash : IObject
{
    public Dictionary<HashKey, HashPair> Pair { get; init; }

    public Hash(Dictionary<HashKey, HashPair> pair)
    {
        Pair = pair;
    }

    public string Inspect()
    {
        var sb = new StringBuilder();
        var pairs = new List<string>();

        foreach (var pair in Pair)
        {
            pairs.Add($"{pair.Value.Key.Inspect()}:{pair.Value.Value.Inspect()}");
        }

        sb.Append("{");
        sb.Append(string.Join(",", pairs));
        sb.Append("}");

        return sb.ToString();
    }

    public string Type() => ObjectType.HASH_OBJ;
}
