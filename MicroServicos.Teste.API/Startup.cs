using MicroServicos.Teste.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MicroServicos.Teste.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthService(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddGlobalExceptionHandlerMiddleware();

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

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Teste HTTP API",
                    Version = "v1",
                    Description = "Serviço Teste HTTP API"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Teste.API V1");
               });

            app.UseHsts();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseGlobalExceptionHandlerMiddleware();
            app.UseResponseCompression(); // Comprimir todas as requisicoes.
            app.UseAuthentication();
            app.UseHttpsRedirection(); // Redireciona todas as chamadas para HTTPS
            app.UseCookiePolicy(); // Ajuste relacionado à GDPR
            app.UseMvc();
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            string authenticationProviderKey = "IdentityApiKey";
            string secret = Configuration.GetValue<string>("Secret");

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
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
                    ValidateAudience = false
                };
                x.Audience = "microservicos";
            });
        }
    }
}
