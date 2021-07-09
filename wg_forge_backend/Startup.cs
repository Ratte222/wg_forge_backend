using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL.EF;
using DAL.Entities;
using DAL.Interface;
using DAL;  
using BLL.Services;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System;

namespace wg_forge_backend
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CatContext>(options => options.UseSqlServer(connection));
            services.AddScoped<ITaskService, TaskServices>();
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(CatProfile));
            //services.AddScoped(typeof(IRepository<Cat>), typeof(Repository<Cat>));
            //services.AddScoped(typeof(IRepository<CatColorInfo>), typeof(Repository<CatColorInfo>));
            //services.AddScoped(typeof(IRepository<CatStat>), typeof(Repository<CatStat>));
            services.AddScoped<IRepository<Cat>, Repository<Cat>>();
            services.AddScoped<IRepository<CatOwner>, Repository<CatOwner>>();
            services.AddScoped<IRepository<CatColorInfo>, Repository<CatColorInfo>>();
            services.AddScoped<IRepository<CatStat>, Repository<CatStat>>();
            services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cat API", Version = "v1", });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cat API v1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ExeptionMeddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{*catchall}");
            });
        }
    }
}
