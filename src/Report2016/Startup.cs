using Lykke.SettingsReader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lykke.Logs;
using Report2016.AzureRepositories;
using AzureStorage.Tables;
using Common.Log;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Report2016.Authentication;

namespace Report2016
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public static SettingsModel.ReportsModel Settings;

       // public static ILog Log;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            /*
            Settings = HttpSettingsLoader.Load<SettingsModel>().Report2016;

            var applicationName =
                          Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationName;

            Log = new LykkeLogToAzureStorage(
                applicationName,
                new AzureTableStorage<LogEntity>(Settings.LogsConnectionString, "VotesLogs", null),
                null);

            services.AddSingleton<ILog>(Log);


            services.BindAzureRepositories(Settings.VotesConnectionString, Log);


            services.Configure<ForwardedHeadersOptions>(options =>
            {
              options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });


			services.AddAuthentication(
	            options => { options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; });

*/



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();


            /*
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AutomaticAuthenticate = true,
				AutomaticChallenge = true,
				ExpireTimeSpan = TimeSpan.FromHours(24),
				LoginPath = new PathString("/signin"),
				AccessDeniedPath = "/Home/Error"
			});


			app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
			{
				RequireHttpsMetadata = true,
				SaveTokens = true,

				// Note: these settings must match the application details
				// inserted in the database at the server level.
				ClientId = Settings.Authentication.ClientId,
				ClientSecret = Settings.Authentication.ClientSecret,
				PostLogoutRedirectUri = Settings.Authentication.PostLogoutRedirectUri,
				CallbackPath = "/auth",

				// Use the authorization code flow.
				ResponseType = OpenIdConnectResponseType.Code,
				Events = new AuthEvent(Log),

				// Note: setting the Authority allows the OIDC client middleware to automatically
				// retrieve the identity provider's configuration and spare you from setting
				// the different endpoints URIs or the token validation parameters explicitly.
				Authority = Settings.Authentication.Authority,
				Scope = { "email profile" }
			});


			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});
*/
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{*url}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
