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
using DAL.Service;  
using BLL.Services;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DAL.Helpers;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Net;

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
            //https://docs.microsoft.com/ru-ru/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0
            services.AddHealthChecks().AddDbContextCheck<CatContext>()//понапихал тут всякого себе для примеров
                .AddCheck("Foo", () =>
                {
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(
                        "https://localhost:5001/ping");
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return HealthCheckResult.Healthy("Ping is OK!");
                    }
                    else
                    {
                        return HealthCheckResult.Unhealthy("Ping is unhealthy!");
                    }
                }, tags: new[] { "ping_tag" })
                .AddCheck("Bar", i =>
                     HealthCheckResult.Unhealthy("Bar is unhealthy!"), tags: new[] { "bar_tag" })
                .AddCheck("Baz", () =>
                    HealthCheckResult.Healthy("Baz is OK!"), tags: new[] { "baz_tag" }); ;
            services.AddDbContext<CatContext>(options => options.UseSqlServer(connection));
            services.AddAutoMapper(typeof(CatProfile));
            //services.AddScoped(typeof(IRepository<Cat>), typeof(Repository<Cat>));
            //services.AddScoped(typeof(IRepository<CatColorInfo>), typeof(Repository<CatColorInfo>));
            //services.AddScoped(typeof(IRepository<CatStat>), typeof(Repository<CatStat>));
            services.AddScoped<ICatService, CatService>();
            services.AddScoped<ICatOwnerService, CatOwnerService>();
            services.AddScoped<IRepository<CatColorInfo>, Repository<CatColorInfo>>();
            services.AddScoped<IRepository<CatStat>, Repository<CatStat>>();
            services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cat API", Version = "v1", });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;//if false - do not use SSl
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = appSettings.Issuer,

                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = appSettings.Audience,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,

                    // установка ключа безопасности
                    //IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });
            services.AddScoped<ITaskService, TaskServices>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddControllersWithViews();
            //var hexColorSection = Configuration.GetSection("HEXColor");
            //var hexColor = hexColorSection.Get<Dictionary<string, string>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CatContext catContext)
        {
            catContext.Database.Migrate();
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
            app.UseSerilogRequestLogging();//в зависимотсти от того, где находиться это объявление будут логгироваться разные
            //части обработки запроса. Например если написать логгер выше UseStaticFiles он будет логгировать скачивание
            //статических файлов
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExeptionMeddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    Predicate = (check) => check.Tags.Contains("ping_tag") ||
                        check.Tags.Contains("baz_tag")
                });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{*catchall}");
            });
        }
    }
}
