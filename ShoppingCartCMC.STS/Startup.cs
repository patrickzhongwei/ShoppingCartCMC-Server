using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using ServiceStack.Redis;
using ShoppingCartCMC.Server.Shared.Common;
using ShoppingCartCMC.Server.Shared.DB.Identity;
using ShoppingCartCMC.Server.Shared.Identity;
using ShoppingCartCMC.STS.Filters;
using ShoppingCartCMC.STS.Resources;
using ShoppingCartCMC.STS.Services;
using ShoppingCartCMC.STS.Services.Certificate;

namespace ShoppingCartCMC.STS
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        private readonly IWebHostEnvironment _environment;

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("IdentityServer4", LogEventLevel.Warning)
                .Enrich.WithProperty("App", "STS")
                .Enrich.FromLogContext()
                // .WriteTo.Seq("http://localhost:5341")    //PW: no need, from Serilog.Sinks.RollingFile in Nuget.
                .WriteTo.File(baseFolder + @"Logs\STS-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            _environment = env;

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cert = GetCertificates(_environment, Configuration).GetAwaiter().GetResult();
            AddLocalizationConfigurations(services);

            services.AddCors(options =>
            {
                var corsAllowOrigins = Configuration.GetSection("CorsAllowOrigins").Get<string[]>();
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });


            services.AddDbContext<MyIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("ShoppingCartCmc.Server.Shared")));


            services.AddIdentity<MyIdentityUser, IdentityRole>(options =>
            {   //PW: change default lockout setting.
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //PW: default is 5 minutes.
                options.Lockout.MaxFailedAccessAttempts = 10; //PW: default is 5 times.
            })
            .AddErrorDescriber<StsIdentityErrorDescriber>()
            .AddDefaultTokenProviders()                         //PW: must be provided, otherwise 2FA not working.
            //.AddDefaultUI(UIFramework.Bootstrap4)             //PW: 这行代码千万不能加，不然，又会与Asp.net Core的Identity挂钩！
            .AddEntityFrameworkStores<MyIdentityDbContext>();  //PW: match db table 'AspNet_xxx'


            services.AddControllersWithViews();


            //PW: add logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddSerilog();
            });




            //PW: add HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddTransient<IProfileService, IdentityWithAdditionalClaimsProfileService>();


            //PW: IEntityRepositoryHelper //不能用Singleton
            services.AddScoped<IEntityRepositoryHelper, EntityRepositoryHelper>();


            //PW: IRedisEntityRepositoryHelper //不能用Scoped
            services.AddSingleton<IRedisEntityRepositoryHelper, RedisEntityRepositoryHelper>();


            //PW: Redis client
            string redisHost = Configuration.GetSection("Redis-Host").Value;
            if (redisHost == null || redisHost == string.Empty) redisHost = "localhost";
            services.AddSingleton<IRedisClientsManager>(c => new RedisManagerPool(redisHost));


            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });


            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new SecurityHeadersAttribute());
            })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };
                })
                .AddNewtonsoftJson();


            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddSigningCredential(cert.ActiveCertificate)
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());

                //PW: from DB
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ////PW: this adds the config data from DB (clients, resources), match db table 'Api_xxx', 'Client_xxx'
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = builder =>
                //        builder.UseSqlServer(connectionString,
                //            sql => sql.MigrationsAssembly(migrationsAssembly));
                //})
                ////PW: this adds the operational data from DB (codes, tokens, consents), match db table 'PersistedGrants'
                //.AddOperationalStore(options =>
                //{
                //    options.ConfigureDbContext = builder =>
                //        builder.UseSqlServer(connectionString,
                //            sql => sql.MigrationsAssembly(migrationsAssembly));
                //})
                //.AddAspNetIdentity<MyIdentityUser>()
                //.AddProfileService<IdentityWithAdditionalClaimsProfileService>();
                //--------------------------------------------------------------------------------------------


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowAllOrigins");

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }





        private static async Task<(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate)> GetCertificates(IWebHostEnvironment environment, IConfiguration configuration)
        {
            var certificateConfiguration = new CertificateConfiguration
            {
                // Use an Azure key vault
                CertificateNameKeyVault = configuration["CertificateNameKeyVault"], //"StsCert",
                KeyVaultEndpoint = configuration["AzureKeyVaultEndpoint"], // "https://damienbod.vault.azure.net"

                // Use a local store with thumbprint
                //UseLocalCertStore = Convert.ToBoolean(configuration["UseLocalCertStore"]),
                //CertificateThumbprint = configuration["CertificateThumbprint"],

                // development certificate
                //PW: to use certificate below in production, open applicationPool "sts.guruhedge.com" in IIS, set "Load User Profile" to "True".
                DevelopmentCertificatePfx = Path.Combine(environment.ContentRootPath, "certificate_combined.pfx"),
                DevelopmentCertificatePassword = "9876" //configuration["DevelopmentCertificatePassword"] //"1234",
            };

            (X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate) certs = await CertificateService.GetCertificates(certificateConfiguration).ConfigureAwait(false);

            return certs;
        }


        private static void AddLocalizationConfigurations(IServiceCollection services)
        {
            services.AddSingleton<LocService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                                new CultureInfo("en-US"),
                                new CultureInfo("zh-CN"),
                                new CultureInfo("ja-JP"),
                                new CultureInfo("ko-KR")
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;

                    var providerQuery = new LocalizationQueryProvider
                    {
                        QueryParameterName = "ui_locales"
                    };

                    options.RequestCultureProviders.Insert(0, providerQuery);
                });
        }


        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (DisallowsSameSiteNone(userAgent))
                {
                    // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }

        private static bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking stack
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }

    }

}
