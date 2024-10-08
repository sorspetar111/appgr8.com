using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;

namespace Report
{
 
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<ReportBackgroundService>();
                    
                    //services.AddHttpClient<GeoLocationController>();
                    //services.AddHttpClient<ReportsController>();

                    services.AddDbContext<ReportDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                });
    }


}

