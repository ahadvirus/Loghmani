using System.Globalization;
using System.IO;
using Loghmani.ECommerce.Old.Areas.Auth.Controllers;
using Loghmani.ECommerce.Old.Data;
using Loghmani.ECommerce.Old.Infrastructures.Configurations;
using Loghmani.ECommerce.Old.Infrastructures.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

namespace Loghmani.ECommerce.Old;

public static class Startup
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    public static void ConfigurationService(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddSingleton<DatabaseContext>();


        string localizationPath = nameof(LocalizationOptions.ResourcesPath).Replace(oldValue: nameof(Path), newValue: string.Empty);
        
        /* string.Format(
        format: "{0}{1}Localization",
        args: new object[] { 
            environment.WebRootPath.Replace(
                oldValue: string.Format(
                    format: "{0}{1}",
                    args: new object[] { environment.ContentRootPath, Path.DirectorySeparatorChar }
                ),
                newValue: string.Empty
            ),
                Path.DirectorySeparatorChar
            }
        ); */

        //nameof(LocalizationOptions.ResourcesPath).Replace(oldValue: nameof(Path), newValue: string.Empty); 

        /* Path.GetFullPath(
            path: string.Format(format: "{0}{1}Localization",
                args: new object[] { environment.WebRootPath, Path.DirectorySeparatorChar })
        ); */

        services.AddLocalization(options => options.ResourcesPath = localizationPath);

        services.AddAuthentication()
            .AddCookie(options =>
            {
                options.AccessDeniedPath = string.Format(
                    "/{0}/{1}/{2}",
                    nameof(Area.Auth),
                    nameof(AccessController).RemoveController(),
                    nameof(AccessController.Denied)
                    );

                options.LoginPath = string.Format(
                    "/{0}/{1}",
                    nameof(Area.Auth),
                    nameof(LoginController).RemoveController()
                );

                options.LogoutPath = string.Format(
                    "/{0}/{1}",
                    nameof(Area.Auth),
                    nameof(LogoutController).RemoveController()
                );
            });

        services.AddControllersWithViews()
            .AddViewLocalization(options => options.ResourcesPath = localizationPath)
            .AddDataAnnotationsLocalization();


        services.Configure<RequestLocalizationOptions>(LocalizationOption);

        services.AddRouting(options =>
        {
            options.LowercaseQueryStrings = true;
            options.LowercaseUrls = true;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public static void Configuration(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseRequestLocalization(LocalizationOption);

        app.UseRequestCulture();

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    private static void LocalizationOption(RequestLocalizationOptions options)
    {
        string persianCulture = "fa-IR";
        string englishCulture = "en-US";

        CultureInfo[] supportedCultures = new CultureInfo[]
        {
            new CultureInfo(name: englishCulture),
            new CultureInfo(name: persianCulture)
        };

        options.DefaultRequestCulture = new RequestCulture(culture: persianCulture, uiCulture: persianCulture);
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.SetDefaultCulture(persianCulture);
    }
}