using System.Collections.Generic;

namespace DotMonkey.Parser.Object;

public class Environment
{
    private Dictionary<string, IObject> _store = new Dictionary<string, IObject>();

    public IObject Set(string name, IObject value)
    {
        _store[name] = value;

        return value;
    }

    public (IObject val, bool ok) Get(string name)
    {
        var exists = _store.TryGetValue(name, out var value);

        return (value, exists);
    }
}