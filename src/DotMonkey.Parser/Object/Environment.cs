using System.Collections.Generic;

namespace DotMonkey.Parser.Object;

public class Environment
{
    private Dictionary<string, IObject> _store = new Dictionary<string, IObject>();
    public Environment Outer
    { get; set; } = null;

    public IObject Set(string name, IObject value)
    {
        _store[name] = value;

        return value;
    }

    public (IObject val, bool ok) Get(string name)
    {
        var exists = _store.TryGetValue(name, out var value);

        if (exists == false && Outer is not null)
            return Outer.Get(name);

        return (value, exists);
    }

    public static Environment NewEnclosedEnvironment(Environment outer)
    {
        var env = new Environment();
        env.Outer = outer;
        return env;
    }
}