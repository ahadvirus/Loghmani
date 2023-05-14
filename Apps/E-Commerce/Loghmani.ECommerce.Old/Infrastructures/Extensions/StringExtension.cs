using Microsoft.AspNetCore.Mvc;

namespace Loghmani.ECommerce.Old.Infrastructures.Extensions;

public static class StringExtension
{
    public static string RemoveController(this string value)
    {
        return value.Replace(
            nameof(Controller),
            string.Empty
        );
    }
}