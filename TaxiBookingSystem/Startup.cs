using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using TaxiBookingSystemCommons;
using TaxiBookingSystemEntities;

namespace TaxiBookingSytem
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            //adding swagger doc
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Taxi Booking System", Version = "v1" });
            });

            //singletons
            services.AddSingleton(typeof(WorldMap));
            services.AddSingleton(typeof(SimpleTimeTracker<int>));
            services.AddSingleton(typeof(TaxiFleet));
            
            WorldMap.Instance.withOnly2DCities();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddConsole(); //keeping it simple console so that review of code is easy
            loggerFactory.AddDebug(); //added debug purposely so that loglevel info can also be seen via console.

            //use same logger factory across applicaiton.
            ApplicationLogging.LoggerFactory = loggerFactory;

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taxi Booking System api");
                
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSwagger();
            app.UseMvc();
        }
    }
}
