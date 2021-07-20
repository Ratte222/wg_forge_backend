using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL.EF;
using DAL.Entities;
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
using BLL.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;


namespace wg_forge_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                            .AddJsonFile("MailAddressConfig.json")
                           .AddConfiguration(configuration);
            // create config
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddMvcCore().AddApiExplorer().AddAuthorization();
            //--------- HealthCheck settingd ---------------------
            //https://docs.microsoft.com/ru-ru/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0
            services.AddHealthChecks().AddDbContextCheck<CatContext>()//added different for examples
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
            //--------- HealthCheck settingd ---------------------

            //--------- config settingd ---------------------
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var mailAddresConfigSection = Configuration.GetSection("MailAddresConfig");
            services.Configure<EmailService>(mailAddresConfigSection);
            var mailSettings = mailAddresConfigSection.Get<EmailService>();

            //--------- config settingd ---------------------

            //--------- JWT settingd ---------------------

            services.AddIdentity<CatOwner, IdentityRole>()
               .AddEntityFrameworkStores<CatContext>()
               .AddDefaultTokenProviders(); ;

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                options.SignIn.RequireConfirmedEmail = true;
            });
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
          {
              options.RequireHttpsMetadata = true;//if false - do not use SSl
              options.SaveToken = true;
              options.Events = new JwtBearerEvents()
              {
                  OnAuthenticationFailed = (ctx) =>
                  {
                      if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                      {
                          ctx.Response.StatusCode = 401;
                      }

                      return Task.CompletedTask;
                  },
                  OnForbidden = (ctx) =>
                  {
                      if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                      {
                          ctx.Response.StatusCode = 403;
                      }

                      return Task.CompletedTask;
                  }
              };
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  // specifies whether the publisher will be validated when validating the token 
                  ValidateIssuer = true,
                  // a string representing the publisher
                  ValidIssuer = appSettings.Issuer,

                  // whether the consumer of the token will be validated 
                  ValidateAudience = true,
                  // token consumer setting 
                  ValidAudience = appSettings.Audience,
                  // whether the lifetime will be validated 
                  ValidateLifetime = true,

                  // security key installation 
                  //IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                  IssuerSigningKey = new SymmetricSecurityKey(key),
                  // security key validation 
                  ValidateIssuerSigningKey = true,
              };
          });
            //--------- JWT settingd ---------------------

            //---------Identity settingd ---------------------


            //services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            //    options.LoginPath = "Account/Login";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.SlidingExpiration = true;
            //});
            //---------Identity settingd ---------------------

            services.AddDbContext<CatContext>(options => options.UseSqlServer(connection));

            services.AddScoped<ICatService, CatService>();
            services.AddScoped<ICatOwnerService, CatOwnerService>();
            services.AddScoped<IRepository<CatColorInfo>, Repository<CatColorInfo>>();
            services.AddScoped<IRepository<CatStat>, Repository<CatStat>>();

            services.AddScoped<ITaskService, TaskServices>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddRouting();

            services.AddAutoMapper(typeof(CatProfile));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cat API", Version = "v1", });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            CatContext catContext, RoleManager<IdentityRole> roleManager)
        {
            catContext.Database.Migrate();
            InitializeIdentityRole(roleManager);

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
            //app.UseStaticFiles();
            app.UseSerilogRequestLogging();//в зависимотсти от того, где находиться это объявление будут логгироваться разные
            //части обработки запроса. Например если написать логгер выше UseStaticFiles он будет логгировать скачивание
            //статических файлов 
           
            app.UseMiddleware<ExeptionMeddleware>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{*catchall}");
            });
        }

        private void InitializeIdentityRole(RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToListAsync().GetAwaiter().GetResult();
            foreach (string role in typeof(AccountRole).GetAllPublicConstantValues<string>())
            {
                if (roles.Find(f=>f.Name == role) == null)
                {
                    roleManager.CreateAsync(new IdentityRole(role));
                }
            } 
        }
        
    }    
}
