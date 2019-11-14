using MicroServicos.Gateway.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Text;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace MicroServicos.Gateway.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                        .AddJsonFile("configuration.json")
                        .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddHealthChecks()
            //    .AddCheck("self", () => HealthCheckResult.Healthy())
            //    .AddUrlGroup(new Uri(Configuration["AutenticaUrlHC"]), name: "autenticaapi-check", tags: new string[] { "autenticaapi" })
            //    .AddUrlGroup(new Uri(Configuration["TesteUrlHC"]), name: "testeapi-check", tags: new string[] { "testeapi" });

            services.AddGlobalExceptionHandlerMiddleware();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials());
            });

            var authenticationProviderKey = "IdentityApiKey";

            var secret = Configuration.GetValue<string>("Secret");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = authenticationProviderKey;
            })
            .AddJwtBearer(authenticationProviderKey, x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudiences = new[] { "microservicos" }
                };
            });

            services.AddOcelot(Configuration);

            // Ajustes relacionados à GDPR
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // habilita a gravacao de sessao.
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });
            services.AddResponseCompression(); // Comprimir todas as requisicoes.
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseHealthChecks("/liveness", new HealthCheckOptions
            //{
            //    Predicate = r => r.Name.Contains("self")
            //});

            app.UseHsts();
            app.UseGlobalExceptionHandlerMiddleware();
            app.UseResponseCompression(); // Comprimir todas as requisicoes.
            app.UseAuthentication();
            app.UseOcelot().Wait();
            app.UseHttpsRedirection(); // Redireciona todas as chamadas para HTTPS
            app.UseCookiePolicy(); // Ajuste relacionado à GDPR
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
