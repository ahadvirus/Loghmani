using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace Loghmani.ECommerce.Old.Infrastructures.Middlewares;

public class RequestCultureMiddleware
{
    private RequestDelegate Next { get; }
    private CultureInfo Culture { get; }

    public RequestCultureMiddleware(RequestDelegate next, IOptions<RequestLocalizationOptions> options)
    {
        Next = next;

        Culture = options.Value.DefaultRequestCulture.Culture;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        CultureInfo.CurrentCulture = Culture;
        CultureInfo.CurrentUICulture = Culture;
        // Call the next delegate/middleware in the pipeline.
        await Next(context);
    }
}