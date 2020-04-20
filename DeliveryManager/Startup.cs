using AutoMapper;
using DeliveryManager.Infrastructure.Exception;
using DeliveryManager.Repositories;
using DeliveryManager.Repositories.Contracts;
using DeliveryManager.Services;
using DeliveryManager.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeliveryManager
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
            services
                .AddScoped<IDeliveryWindowService, DeliveryWindowService>()
                .AddDbContext<IDeliveryWindowRepo, DeliveryWindowRepo>()
                .AddControllers();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Delivery Manager API", Version = "v1" });
                c.ExampleFilters();
                c.EnableAnnotations();
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionMiddleware(env.IsDevelopment());

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Path.Value.EndsWith("swagger.json"))
                    ctx.Response.Headers["Access-Control-Allow-Origin"] = "*";
                await next();
            });

            app.UseSwagger(options =>
            {
                options.RouteTemplate = env.IsDevelopment()
                    ? $"swagger/{Constants.ApiSegment.Api}/{options.RouteTemplate}"
                    : $"{Constants.ApiSegment.Api}/{options.RouteTemplate}";
            });

            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{Constants.ApiSegment.Api}/swagger/v1/swagger.json", "Delivery Manager API");
                });
            }

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
