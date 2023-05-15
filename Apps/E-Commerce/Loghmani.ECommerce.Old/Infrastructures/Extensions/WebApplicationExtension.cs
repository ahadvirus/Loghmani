using Loghmani.ECommerce.Old.Infrastructures.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Loghmani.ECommerce.Old.Infrastructures.Extensions;

public static class WebApplicationExtension
{
    public static IApplicationBuilder UseRequestCulture(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestCultureMiddleware>();
    }
}