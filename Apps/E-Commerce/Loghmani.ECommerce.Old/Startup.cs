using Loghmani.ECommerce.Old.Areas.Auth.Controllers;
using Loghmani.ECommerce.Old.Data;
using Loghmani.ECommerce.Old.Infrastructures.Configurations;
using Loghmani.ECommerce.Old.Infrastructures.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Loghmani.ECommerce.Old;

public static class Startup
{
    public static void ConfigurationService(IServiceCollection services)
    {
        services.AddSingleton<DatabaseContext>();

        services.AddAuthentication()
            .AddCookie(options =>
            {
                options.AccessDeniedPath = string.Format(
                    "/{0}/{1}/{2}",
                    nameof(Area.Admin),
                    nameof(AccessController).RemoveController(),
                    nameof(AccessController.Denied)
                    );

                options.LoginPath = string.Format(
                    "/{0}/{1}",
                    nameof(Area.Admin),
                    nameof(LoginController).RemoveController()
                );

                options.LogoutPath = string.Format(
                    "/{0}/{1}",
                    nameof(Area.Admin),
                    nameof(LogoutController).RemoveController()
                );
            });

        services.AddControllersWithViews();

        services.AddRouting(options =>
        {
            options.LowercaseQueryStrings = true;
            options.LowercaseUrls = true;
        });
    }

    public static void Configuration(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }
}