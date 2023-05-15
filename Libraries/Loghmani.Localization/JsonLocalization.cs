using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using Loghmani.Localization.Models;
using Microsoft.Extensions.Localization;

namespace Loghmani.Localization;

public class JsonLocalization<T> : IStringLocalizer<T>
{
    private Cultures Cache { get; }
    protected JsonLocalizationOption Option { get; }

    public JsonLocalization(Cultures cache, JsonLocalizationOption option)
    {
        Cache = cache;
        Option = option;
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        using (FileStream stream = Streamer(FileAccess.Read, FileShare.Read))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                JsonElement content = JsonSerializer.Deserialize<JsonElement>(reader.ReadToEnd());

                foreach (JsonProperty property in content.EnumerateObject())
                {
                    yield return new LocalizedString(
                        name: property.Name,
                        value: property.Value.GetRawText(),
                        resourceNotFound: false
                        );
                }

            }
        }
    }

    public LocalizedString this[string name]
    {
        get
        {
            string result = Find(name: name);
            return new LocalizedString(name: name, value: result, resourceNotFound: string.IsNullOrEmpty(result));
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            LocalizedString result = this[name];
            return new LocalizedString(
                name: name, 
                value: string.Format(format: result.Value, args: arguments),
                resourceNotFound: result.ResourceNotFound
                );
        }
    }

    /// <summary>
    /// Find translation of the key
    /// </summary>
    /// <param name="name"><see cref="string"/></param>
    /// <returns><see cref="string"/></returns>
    public string Find(string name)
    {
        string result = string.Empty;

        try
        {
            result = Cache[CurrentCultureName()][TypeName()][name];
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(KeyNotFoundException))
            {
                result = ReadFromFile(name);

                Cache
                    .Add(name: CurrentCultureName())
                    .Add(name: TypeName())
                    .Add(name: name, value: result);
            }

            Debug.WriteLine(string.Format("\n{0}\n", e.Message));
        }

        return result;

    }

    /// <summary>
    /// Return localization from the file
    /// </summary>
    /// <param name="name"><see cref="string"/></param>
    /// <returns><see cref="string"/></returns>
    protected string ReadFromFile(string name)
    {
        string result = string.Empty;
        try
        {
            using (FileStream stream = Streamer(FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    JsonElement content = JsonSerializer.Deserialize<JsonElement>(reader.ReadToEnd());


                    result = content.GetProperty(name)
                        .GetRawText();
                }
            }
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(KeyNotFoundException))
            {
                WriteToFile(name);
            }

            Debug.WriteLine(string.Format(format: "\n{0}\n", args: new object?[]{ e.Message }));
        }

        return result;
    }

    /// <summary>
    /// Save localization to file
    /// </summary>
    /// <param name="name"><see cref="string"/></param>
    protected void WriteToFile(string name)
    {
        using (FileStream stream = Streamer(FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            string content;

            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }

            bool found = false;
            int index = content.Length;
            do
            {
                if (content[index] == '}')
                {
                    found = true;
                }
                else
                {
                    index -= 1;
                }
            } while (!found);

            string value = string.Empty;

            content = string.Format("{0}{1}", content.Substring(startIndex: 0, length: (index - 1)), string.Format("\t\"{0}\": \"{1}\"\n}}", name, value));

            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(value: content);
            }
        }
    }

    /// <summary>
    /// Open file for read or write base on access and share
    /// </summary>
    /// <param name="access"><see cref="FileAccess"/></param>
    /// <param name="share"><see cref="FileShare"/></param>
    /// <returns><see cref="FileStream"/></returns>
    protected FileStream Streamer(FileAccess access, FileShare share)
    {
        return new FileStream(path: FilePath(), mode: FileMode.Open, access: access, share: share);
    }

    /// <summary>
    /// The file address, create if the file doesn't exist 
    /// </summary>
    /// <returns><see cref="string"/></returns>
    protected string FilePath()
    {
        string result =
            Path.GetFullPath(path: string.Format("{0}/{1}/{2}.json", Option.Address, CurrentCultureName(), TypeName()));

        if (File.Exists(path: result))
        {
            using (StreamWriter writer = File.CreateText(path: result))
            {
                writer.Write(new StringBuilder(JsonSerializer.Serialize(new JsonElement())));
            }
        }

        return result;
    }

    /// <summary>
    /// Return full name type of
    /// </summary>
    /// <returns><see cref="string"/></returns>
    protected string TypeName()
    {
        return (typeof(T).FullName ?? typeof(T).Name).Replace(oldChar: '.', newChar: '-');
    }

    /// <summary>
    /// Return current culture name
    /// </summary>
    /// <returns><see cref="string"/></returns>
    protected string CurrentCultureName()
    {
        return Thread.CurrentThread.CurrentCulture.Name;
    }
}
