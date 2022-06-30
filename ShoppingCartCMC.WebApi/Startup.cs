using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.DB.Trading;
using ShoppingCartCMC.Server.Shared.MarketData;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Server.Shared.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartCMC.Server.Shared.DB.Identity;
using Microsoft.IdentityModel.Tokens;
using ShoppingCartCMC.Shared.Common;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using ShoppingCartCMC.Server.Shared.Pricing;
using ShoppingCartCMC.WebApi.SignalrHubs.Pricing;
using ServiceStack.Redis;
using ShoppingCartCMC.Server.Shared.ReferenceData;
using ShoppingCartCMC.WebApi.SignalrHubs.Transport;

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

            // pricing
            services.AddSingleton<IPricePublisher, PricePublisher>();
            services.AddSingleton<IPriceFeed, PriceFeedSimulator>();  
            services.AddSingleton<IPriceLastValueCache, PriceLastValueCache>();

            // reference data
            services.AddSingleton<ICurrencyPairRepository, CurrencyPairRepository>();


            ////PW: Redis client
            //string redisHost = Configuration.GetSection("Redis-Host").Value;
            //if (redisHost == null || redisHost == string.Empty) redisHost = "localhost";
            //services.AddSingleton<IRedisClientsManager>(c => new RedisManagerPool(redisHost));

            //inflastructure
            services.AddSingleton<IContextHolder, ContextHolder>(); //PW: must be singleton, otherwise cient-side timeout.



            //PW: add logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddSerilog();
            });

            //PW: add controller
            /*
            * Patrick: this is for Newtonsoft to serialize             * 
            */
            //services.AddControllers().AddNewtonsoftJson();

            /*
             * Patrick: this is for System.Text.Json to serialize             * 
             */
            services.AddControllers().AddJsonOptions(option =>
            {
                //PW: below should set to 'true', as Angular property nameing is Camel-case while c# is Pascal-case.However, as Dto properties are minified into single character, it doesn't matter.
                //option.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });


            services.AddSignalR(); //.AddNewtonsoftJsonProtocol(); //PW: not used

            //PW: add Swagger
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingCartCMC.WebApi", Version = "v1" });
                option.AddSignalRSwaggerGen(); 
            });



            //PW: add Database context
            services.AddDbContext<ShoppingCartCmcTradingContext>((serviceProvider, options) =>
                    options.UseSqlServer(Configuration.GetConnectionString("ShoppingCartCmcTradingConnection"))
             );


            services.AddDbContext<ShoppingCartCmcIdentityContext>((serviceProvider, options) =>
                    options.UseSqlServer(Configuration.GetConnectionString("ShoppingCartCmcIdentityConnection"))
             );



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
                       .WithOrigins(new string[] { "http://localhost:4200/" })
                       //.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                       ;
                /*****************************/
            }));
           


            //PW: below is authentication related settings, comment line at dev environment
            //**************************************************************************************************
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //var tokenValidationParameters = new TokenValidationParameters()
            //{
            //    ValidIssuer = Configuration.GetValue<string>("IdentityServer.TokenValidIssuer"),
            //    ValidAudience = StsSetting.ApiResourceName,  //"ShoppingCartCMC.WebApi",
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StsSetting.ApiResourceSecrets)),
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
            //    options.Audience = StsSetting.ApiResourceName; // "ShoppingCartCMC.WebApi";
            //    options.IncludeErrorDetails = true;
            //    options.SaveToken = true;
            //    options.SecurityTokenValidators.Clear();
            //    options.SecurityTokenValidators.Add(jwtSecurityTokenHandler);
            //    options.TokenValidationParameters = tokenValidationParameters;
            //    options.Events = new JwtBearerEvents
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            if (context.Request.Path.Value.StartsWith("/api/billing") && context.Request.Query.TryGetValue("token", out StringValues token) )
            //            {
            //                context.Token = token;
            //            }

            //            return Task.CompletedTask;
            //        },
            //        OnAuthenticationFailed = context =>
            //        {
            //            var te = context.Exception;
            //            return Task.CompletedTask;
            //        }
            //    };
            //});
            //-------------------------------------------------------------------------------------------

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyCorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCartCMC.WebApi v1"));
            }

            app.UseHttpsRedirection();   

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                endpoints.MapHub<PricingHub>("/signalrPricing");
            });
        }
    }
}
