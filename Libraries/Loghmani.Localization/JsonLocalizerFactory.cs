using System;
using System.Linq;
using System.Reflection;
using Loghmani.Localization.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Loghmani.Localization;

public class JsonLocalizerFactory : IStringLocalizerFactory
{
    private JsonLocalizationOption Option { get; }
    private Cultures Cultures { get; }

    public JsonLocalizerFactory(IOptions<JsonLocalizationOption> option)
    {
        Option = option.Value;
        Cultures = new Cultures();
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return Instance(resourceSource: resourceSource);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return Instance(resourceSource: Type.GetType(baseName) ?? throw new Exception());
    }

    protected IStringLocalizer Instance(Type resourceSource)
    {
        Type localization = typeof(JsonLocalization<>);

        localization = localization.MakeGenericType(new Type[] { resourceSource });
        
        ConstructorInfo constructor = localization
            .GetConstructors()
            .First(constructor => constructor.GetParameters().Count() == 2 && 
                                  constructor.GetParameters().First().ParameterType == typeof(Cultures) &&
                                  constructor.GetParameters().Last().ParameterType == typeof(JsonLocalizationOption))
                                  ;

        return (IStringLocalizer)constructor.Invoke(new object?[] { Cultures, Option });
    }
}