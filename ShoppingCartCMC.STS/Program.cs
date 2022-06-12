using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ShoppingCartCMC.STS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TimeZoneInfo tzinfo = TimeZoneInfo.Local;

            //PW: Hosting server timezone must match to GuruTrader timezone!!
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();


            var seed = args.Contains("/seed");
            if (seed)
            {
                args = args.Except(new[] { "/seed" }).ToArray();
            }

            var host = BuildWebHost(args);

            if (seed)
            {
                //PW: comment this line in production
                //SeedData.EnsureSeedData(host.Services);
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
