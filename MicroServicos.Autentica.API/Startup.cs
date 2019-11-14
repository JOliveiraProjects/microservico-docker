using MicroServicos.Autentica.API.Data;
using MicroServicos.Autentica.API.Interfaces;
using MicroServicos.Autentica.API.Middleware;
using MicroServicos.Autentica.API.Repository;
using MicroServicos.Autentica.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MicroServicos.Autentica.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddGlobalExceptionHandlerMiddleware();

            services.AddDbContext<BancoContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:Database"]));

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAutenticaRepository, AutenticaRepository>();
            services.AddScoped<IAutenticaService, AutenticaService>();         

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

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Autentica HTTP API",
                    Version = "v1",
                    Description = "Serviço Autentica HTTP API"
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
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Autentica.API V1");
               });

            app.UseHsts();
            app.UseGlobalExceptionHandlerMiddleware();
            app.UseResponseCompression(); // Comprimir todas as requisicoes.
            app.UseHttpsRedirection(); // Redireciona todas as chamadas para HTTPS
            app.UseCookiePolicy(); // Ajuste relacionado à GDPR
            app.UseMvc();
        }
    }
}
