using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(NewPhotoFunction.Startup))]

namespace NewPhotoFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=aspnet-WebApplication;Trusted_Connection=True;MultipleActiveResultSets=true"));
        }
    }
}