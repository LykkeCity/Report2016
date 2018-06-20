using Lykke.SettingsReader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Report2016.AzureRepositories;
using AzureStorage.Tables;
using Common.Log;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Report2016.Authentication;
using Lykke.Common.ApiLibrary.Middleware;

namespace Report2016
{
    public class Startup
    {
        private IConfiguration _configuration;
        private SettingsModel _settings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var settingsManager = _configuration.LoadSettings<SettingsModel>();
            _settings = settingsManager.CurrentValue;
            var connectionStringManager = settingsManager.ConnectionString(x => x.Report2016.VotesConnectionString);

            //var log = new LogToAzureStorage(
            //    applicationName,
            //    new AzureTableStorage<LogEntity>(Settings.LogsConnectionString, "VotesLogs", null),
            //    null);        

            var log = new LogToConsole();
            services.AddSingleton<ILog>(log);
            services.BindAzureRepositories(connectionStringManager, log);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });

            var applicationName = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationName; 

            services.AddAuthentication(x =>
            {
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(x => {
                x.ExpireTimeSpan = TimeSpan.FromHours(24);
                x.LoginPath = new PathString("/Home/SignIn");
                x.AccessDeniedPath = "/404";
            })
            .AddOpenIdConnect(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveTokens = true;
                x.ResponseType = "code";
                x.ClientId = _settings.Report2016.Authentication.ClientId;
                x.ClientSecret = _settings.Report2016.Authentication.ClientSecret;
                x.CallbackPath = "/auth";
                x.Events = new AuthEvent(log);
                x.Authority = _settings.Report2016.Authentication.Authority;
                x.Scope.Add("email");
                x.Scope.Add("profile");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseLykkeForwardedHeaders();
            app.UseAuthentication();
            app.UseStaticFiles();

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
