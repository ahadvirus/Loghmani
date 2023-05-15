using System.Collections.Generic;

namespace Loghmani.Localization.Models;

public class Types
{
    protected IDictionary<string, Localize> Data { get; }

    public Types()
    {
        Data = new Dictionary<string, Localize>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public Localize this[string name]
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

    /// <summary>
    /// You can add new Type doesn't exist in system
    /// </summary>
    /// <param name="name">The type name create new of it and access to localization <see cref="string"/></param>
    /// <returns><see cref="Localize"/></returns>
    public Localize Add(string name)
    {
        if (!Data.ContainsKey(name))
        {
            Data.Add(key: name, value: new Localize());
        }

        return this[name];
    }
}