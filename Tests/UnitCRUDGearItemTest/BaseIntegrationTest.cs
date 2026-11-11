using Xunit;

namespace MyWebApi.Tests;
public class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly IServiceProvider _services;

    public BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
        _services = factory.Services;
    }
    public async Task InitializeAsync()
    {
        using var scope = _services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedTodosAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}