using DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Services;


namespace WebAPI
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger for Web APIs", Version = "v1" });
                
            });

            //SQL connection string "WebAPIDBConnection" is defined in appsettings file.
            services.AddDbContext<WebAPIContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WebAPIDBConnection")));

            //resolve dependency
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ICommandService, CommandService>();

            //Automapper registration
            services.AddAutoMapper(typeof(Startup));

            // Other service configurations

    services.AddLogging(configure =>
    {
        configure.AddConsole(); // Add Console logger
        // Add other log providers if needed
    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

      
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger for Web APIs"));

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
