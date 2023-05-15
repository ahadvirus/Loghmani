using System.Collections.Generic;

namespace Loghmani.Localization.Models;

public class Cultures
{
    protected IDictionary<string, Types> Data { get; }

    public Cultures()
    {
        Data = new Dictionary<string, Types>();
    }

    /// <summary>
    /// Get all types of the culture 
    /// </summary>
    /// <param name="name">The name of culture you want the data <see cref="string"/></param>
    /// <returns><see cref="Types"/></returns>
    /// <exception cref="KeyNotFoundException">The exception throw when culture not found</exception>
    public Types this[string name]
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
    /// To handel adding new culture to system
    /// </summary>
    /// <param name="name">The name of culture you want it to add <see cref="string"/></param>
    /// <returns>Access to all types after created culture <see cref="Types"/></returns>
    public Types Add(string name)
    {
        if (!Data.ContainsKey(name))
        {
            Data.Add(name, new Types());
        }

        return this[name];
    }
}