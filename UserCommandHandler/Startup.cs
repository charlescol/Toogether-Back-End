using AggregateBase.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(UserCommand.Startup))]
namespace UserCommand
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            var context = new Model.UserCommandDBContext(new DbContextOptionsBuilder<Model.UserCommandDBContext>().UseSqlServer(SqlConnection).Options);
            var service = new Service.EventStore(context);
            builder.Services.AddTransient<IEventStoreService<Guid, Guid, Aggregate.User.UserAggregate>, Service.EventStore>((_) =>
            {
                return new Service.EventStore(context);
            });
        }
    }
}
