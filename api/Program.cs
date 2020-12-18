using System;
using ClosestCity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetTopologySuite.IO.Converters;
using Microsoft.AspNetCore.Builder;
using NetTopologySuite.Geometries;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;
using ClosestCity.Examples;

// Configure Server
var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((host, services) =>
        {
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new GeoJsonConverterFactory()));
            services.AddSwaggerGen(options => 
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ClosestCity", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerExamplesFromAssemblyOf<ByLocationExamples>();

            services.AddSingleton<GeometryFactory>(
                x => NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(host.Configuration.GetValue<int>("SRID")));

            services.AddDbContextPool<ApiContext>(options =>
            {
                options.UseNpgsql(host.Configuration.GetValue<string>("ConnectionStrings:Database"), options => options.UseNetTopologySuite());
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.LogTo(x => Console.WriteLine(x));
                options.EnableServiceProviderCaching(true);
                options.EnableDetailedErrors(true);
            });
        });

        webBuilder.Configure((host, app) =>
        {
            if (host.HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClosestCity v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    });


// Start Server
host.Build().Run();