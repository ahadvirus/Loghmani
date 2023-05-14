using Microsoft.AspNetCore.Http;

namespace Loghmani.ECommerce.Old.Infrastructures.Extensions;

public static class HttpContextExtension
{
    public static string GetHostName(this HttpContext context)
    {
        return string.Format("{0}://{1}", context.Request.Scheme, context.Request.Host);
    }
}