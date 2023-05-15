using System.Collections.Generic;

namespace Loghmani.Localization.Models;

public class Localize
{
    protected IDictionary<string, string> Data { get; }

    public Localize()
    {
        Data = new Dictionary<string, string>();
    }

    public string this[string name]
    {
        get
        {
            if (Data.ContainsKey(name))
            {
                return Data[name];
            }

            throw new KeyNotFoundException(name);
        }
    }

    public string Add(string name, string value)
    {
        if (!Data.ContainsKey(name))
        {
            Data.Add(key: name, value: value);
        }

        return this[name];
    }
}