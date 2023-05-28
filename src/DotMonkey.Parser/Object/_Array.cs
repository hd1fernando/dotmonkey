using System.Collections.Generic;
using System.Text;

namespace DotMonkey.Parser.Object;

public struct _Array : IObject
{
    public List<IObject> Elements { get; private init; }

    public _Array(List<IObject> elements)
    {
        Elements = elements;
    }

    public string Inspect()
    {
        var sb = new StringBuilder();
        var elements = new List<string>();

        foreach( var elem in Elements)
        {
            elements.Add(elem.Inspect());
        }

        sb.Append("[");
        sb.Append(string.Join(",", elements));
        sb.Append(']');

        return sb.ToString();
        
    }

    public string Type() => ObjectType.ARRAY_OBJ;
}
