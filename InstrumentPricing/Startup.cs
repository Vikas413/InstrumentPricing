using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstrumentPricing.Interfaces;
using InstrumentPricing.Models.Config;
using InstrumentPricing.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag.SwaggerGeneration;
namespace InstrumentPricing
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

            services.Configure<InstrumentInputs>(this.Configuration.GetSection("InstrumentInputs"));
            services.Configure<InstrumentProcessingSettings>(this.Configuration.GetSection("InstrumentProcessingSettings"));

            services.AddSingleton<IInstrumentReader, InstrumentReader>();
            services.AddSingleton<InstrumentCache.InstrumentCache, InstrumentCache.InstrumentCache>();

            //services.AddSingleton<IHostedService, InstrumentProcessingService>();
            services.AddHostedService<InstrumentProcessingService>();

            services.AddControllers();

            services.AddOpenApiDocument();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOpenApi(); // serve OpenAPI/Swagger documents
            app.UseSwaggerUi3(); // serve Swagger UI
            app.UseReDoc(); // serve ReDoc UI

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
