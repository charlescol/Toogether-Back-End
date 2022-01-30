using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

[assembly: FunctionsStartup(typeof(EventCommand.Startup))]
namespace EventCommand
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("MongoDBConnectionString"));
            builder.Services.AddSingleton((s) =>
            {
                return client.GetDatabase("QueryDB");
            });
        }
    }
}
