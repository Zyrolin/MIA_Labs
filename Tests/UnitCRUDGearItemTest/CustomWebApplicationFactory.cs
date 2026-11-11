using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using MyWebApi.Data;

namespace MyWebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MongoDbRunner? _runner;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var runner = MongoDbRunner.Start();
            _runner = runner;

            services.PostConfigure<MongoDbSettings>(settings =>
            {
                settings.ConnectionString = runner.ConnectionString;
                settings.DatabaseName = "IntegrationTestsDb";
            });

            var descriptor = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IMongoClient));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(runner.ConnectionString));

            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("IntegrationTestsDb");
            });
            services.AddSingleton<DatabaseSeeder>();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _runner?.Dispose();
    }
}