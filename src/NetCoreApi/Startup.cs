using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetCoreApi.HealthChecks;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreApi", Version = "v1" });
            });

            services.AddHealthChecks()
                .AddCheck<PrimeiroHealthCheck>(nameof(PrimeiroHealthCheck));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    AllowCachingResponses = false,
                    ResponseWriter = WriteResponse
                });
            });
        }

        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var response = new
            {
                TempoTotalEmSegundos = result.TotalDuration.TotalSeconds,
                Status = result.Status.ToString(),
                Verificacoes = result.Entries.Select(x => new
                {
                    Nome = x.Key,
                    Descricao = x.Value.Description,
                    Status = x.Value.Status.ToString()
                })
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
