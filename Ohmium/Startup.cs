using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ohmium.Models.EFModels;
using Ohmium.Repository;
using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Ohmium.Models;
using Ohmium.Models.TemplateModels;
using Microsoft.AspNetCore.ResponseCompression;

namespace Ohmium
{
    public class Startup
    {
        //readonly string MyAllowSpecificOrigins = "MyAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true; // Optional: enable compression for HTTPS requests
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Ohmium")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDbContext<SensorContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SensorContext")));
            services.AddDbContext<CacheContext>(options =>
        options.UseSqlite(Configuration.GetConnectionString("CacheContext")));
            SQLitePCL.Batteries.Init();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddSpaStaticFiles();
            services.AddScoped<IRMCCRepository<Org>, RMCCRepository<Org>>();
            services.AddScoped<Models.EFModels.Org>();
            services.AddScoped<SensorContext>();
            services.AddSingleton<Models.EFModels.Device>();
            services.AddSingleton<Models.EFModels.EquipmentConfiguration>();
            //services.AddSingleton<Models.EFModels.SensorData>();
            services.AddScoped<IRMCCRepository<Device>, RMCCRepository<Device>>();
            services.AddScoped<IRMCCRepository<EquipmentConfiguration>, RMCCRepository<EquipmentConfiguration>>();
            services.AddScoped<IRMCCRepository<SequenceLibrary>, RMCCRepository<SequenceLibrary>>();
            services.AddScoped<IRMCCRepository<ScriptLibrary>, RMCCRepository<ScriptLibrary>>();
            //services.AddSingleton<IRMCCRepository<StackTestRunHours>, RMCCRepository<StackTestRunHours>>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".MyApp.Session";
                options.Cookie.IsEssential = true;
            });
            services.AddControllersWithViews();
            //services.AddScoped<GlobalFilter>();
            services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ohmium Application", Version = "v1" });
            });

            //services.AddSession();
            //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            services.AddAuthentication();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                //app.UseCors("MyAllowSpecificOrigins");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseResponseCompression();
            app.UseRouting();
            //app.UseCors("MyAllowSpecificOrigins");
            //app.UseCors(options => options.WithOrigins("http://localhost:4200", "https://mtsclient.azurewebsites.net/").AllowAnyMethod().AllowAnyHeader());

            //app.UseCors(options => options.WithOrigins(MyAllowSpecificOrigins));
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/");
                endpoints.MapRazorPages();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ohmium Application v1"));
        }
    }
}
