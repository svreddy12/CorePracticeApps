using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.Models;

namespace WebApplication1
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
            //services.AddControllers();

            services.AddControllers().AddNewtonsoftJson();

            services.AddCors();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new  Microsoft.OpenApi.Models.OpenApiInfo 
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API",
                    //TermsOfService = "None"
                   // Contact = new Contact() { Name = "Talking Dotnet", Email = "contact@talkingdotnet.com", Url = "www.talkingdotnet.com" }
                });
            });

            

            services.AddDbContext<APIDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection"))

                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (ctx, next) =>
            { 
            await next();

            if(ctx.Response.StatusCode==204)
            {
                ctx.Response.ContentLength = 0;
            }


            });

            app.UseCors(options=>options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
