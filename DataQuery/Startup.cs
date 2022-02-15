using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Security.Authentication;

[assembly: FunctionsStartup(typeof(EventCommand.Startup))]
namespace EventCommand
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(Environment.GetEnvironmentVariable("MongoDBConnectionString")));
            settings.SslSettings =new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            builder.Services.AddSingleton((s) =>
            {
                return client.GetDatabase("QueryDB");
            });
        }
    }
}
