using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Server.Shared.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCMC.WebApi
{
    public class Startup
    {       

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment _env { get; set; }


        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            //PW: configure logger
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.WithProperty("App", "ShoppingCartCMC-API")
                .Enrich.FromLogContext()
                // .WriteTo.Seq("http://localhost:5341")    //PW: no need, from Serilog.Sinks.RollingFile in Nuget.                
                .WriteTo.File(path: baseFolder + @"Logs\ShoppingCartCMC-API.log", rollingInterval: RollingInterval.Day) //PW: need to move to other location in production server
                .CreateLogger();

            _env = env;

            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //PW: add DI setting, using scoped.
            services.AddScoped<iBillingRepository, BillingRepository>();
            services.AddScoped<iForexEngineRepository, ForexEngineRepository>();
            services.AddScoped<iProductRepository, ProductRepository>();
            services.AddScoped<iShippingRepository, ShippingRepository>();  

            //PW: add logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddSerilog();
            });
            
            //PW: add controller
            services.AddControllers();

            //PW: add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingCartCMC.WebApi", Version = "v1" });
            });



            /** **********************************************************
            * Patrick: [todo in future].
            * PW: define CORS policy
            * ************************************************************
            */
            services.AddCors(o => o.AddPolicy("MyCorsPolicy", builder =>
            {
                /* Product setting */
                //***************************************************************************************** */
                //var corsAllowOrigins = Configuration.GetSection("CorsAllowOrigins").Get<string[]>();
                //builder
                //        .WithOrigins(corsAllowOrigins)
                //       .SetIsOriginAllowedToAllowWildcardSubdomains()
                //       .AllowAnyMethod()
                //       .AllowAnyHeader()
                //       .AllowCredentials();
                //*************************************************************************************** */

                /* development setting */
                //*************************** */
                builder
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                /*****************************/
            }));

            /** **********************************************************
            * Patrick: [todo in future].
            * PW: add DB context here for further development.
            * ************************************************************
            */
            //services.AddDbContext<CMC_DB_Context>((serviceProvider, options) =>
            //        options.UseSqlServer(Configuration.GetConnectionString("CMC_DB_Connection"))
            // );



            /** **********************************************************
            * Patrick: [todo in future].
            * PW: below is authentication related settings
            * ************************************************************
            */
            //var tokenValidationParameters = new TokenValidationParameters()
            //{
            //    ValidIssuer = Configuration.GetValue<string>("IdentityServer.TokenValidIssuer"),
            //    ValidAudience = StsSetting.ApiResourceName_GuruTraderNetCoreSignalR,  //"GuruTrader.NetCoreSignalR",
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StsSetting.ApiResourceSecrets_GuruTraderNetCoreSignalR)),
            //    NameClaimType = "name",
            //    RoleClaimType = "role",
            //};

            //var jwtSecurityTokenHandler = new JwtSecurityTokenHandler
            //{
            //    InboundClaimTypeMap = new Dictionary<string, string>()
            //};

            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.Authority = Configuration.GetValue<string>("IdentityServer.JwtBearerOptions.Authority");
            //    options.Audience = StsSetting.ApiResourceName_GuruTraderNetCoreSignalR; // "GuruTrader.NetCoreSignalR";
            //    options.IncludeErrorDetails = true;
            //    options.SaveToken = true;
            //    options.SecurityTokenValidators.Clear();
            //    options.SecurityTokenValidators.Add(jwtSecurityTokenHandler);
            //    options.TokenValidationParameters = tokenValidationParameters;
            //    options.Events = new JwtBearerEvents
            //    {                    
            //    };
            //});

            //services.AddAuthorization(options =>
            //{
            //    #region add authorization policies

            //    //staff
            //    options.AddPolicy(AuthorizationPolicyConstants.Staff, policyAdmin => policyAdmin.RequireClaim(ClaimTypeConstants.IsStaff, "true"));
            //    //Api scope
            //    options.AddPolicy(AuthorizationPolicyConstants.NetCoreSignalrApi, policyAdmin => policyAdmin.RequireClaim("scope", StsSetting.ApiResourceName_GuruTraderNetCoreSignalR));

            //    #endregion
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCartCMC.WebApi v1"));
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
