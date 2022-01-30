using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(BlobStorageService.Startup))]
namespace BlobStorageService
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<Service.IBlobStorage, Service.BlobStorage>((_) =>
            {
                return new Service.BlobStorage();
            });
        }
    }
}
