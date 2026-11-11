using MyWebApi.DTOs;
using MyWebApi.Documents;
using MyWebApi;
using MongoDB.Driver;
using System.Runtime.Serialization;
using System.Net;
using MyWebApi.Entities;
using Xunit;

namespace MyWebApi.Tests;

public class GearItemControllerTest : BaseIntegrationTest
{
    public GearItemControllerTest(CustomWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task GetGearItem_ReturnsSeededItems()
    {
        var response = await Client.GetAsync("/api/RentalService/gear");

        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<List<GearItemDocument>>();

        Assert.NotNull(items);
        Assert.Equal(2, items.Count);
        Assert.Contains(items, x => x.Name == "Test Tent");
    }
    [Fact]
    public async Task GetGearById_ReturnsCorrectItem()
    {
        var list = await Client.GetFromJsonAsync<List<GearItemDocument>>("/api/RentalService/gear");
        Assert.NotNull(list);
        var firstId = list[0].Id;

        var response = await Client.GetAsync($"/api/RentalService/gear/{firstId}");

        response.EnsureSuccessStatusCode();

        var item = await response.Content.ReadFromJsonAsync<GearItemDocument>();

        Assert.NotNull(item);
        Assert.Equal(firstId, item.Id);
    }
    [Fact]
    public async Task GetGearById_ReturnsNotFound_ForNonExistingId()
    {
        var response = await Client.GetAsync("/api/RentalService/gear/6748a68293acd000228f0000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task CreateGearItem_ShouldReturnCreatedItem()
    {

        var newItem = new CreateGearItemDto
        {
            Name = "Test Item",
            DailyRate = 10m,
            ReplacementValue = 100m,
            PassedInspection = true
        };


        var response = await Client.PostAsJsonAsync("/api/RentalService/gear", newItem);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<GearItemDocument>();

        Assert.NotNull(created);
        Assert.Equal("Test Item", created.Name);
        Assert.False(string.IsNullOrWhiteSpace(created.Id));

        var list = await Client.GetFromJsonAsync<List<GearItemDocument>>("/api/RentalService/gear");
        Assert.NotNull(list);
        Assert.Equal(3, list.Count);
    }
    [Fact]
    public async Task ReplaceGear_ReturnsNoContent_AndUpdatesInDatabase()
    {
        var list = await Client.GetFromJsonAsync<List<GearItemDocument>>("/api/RentalService/gear");
        Assert.NotNull(list);
        var item = list.First();

        var updated = new ReplaceGearItemDto
        {
            Name = "Updated Item Name",
            PassedInspection = true,
            ReplacementValue = 1000,
            DailyRate = 40,
            Status = GearStatus.Available,
            Condition = GearConditionStatus.Good
        };

        var response = await Client.PutAsJsonAsync($"/api/RentalService/gear/{item.Id}", updated);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var updatedFromDb = await Client.GetFromJsonAsync<GearItemDocument>($"/api/RentalService/gear/{item.Id}");

        Assert.NotNull(updatedFromDb);
        Assert.Equal("Updated Item Name", updatedFromDb.Name);
        Assert.True(updatedFromDb.PassedInspection);
    }

    [Fact]
    public async Task ReplaceGear_ReturnsNotFound_ForNonExistingId()
    {
        var updated = new ReplaceGearItemDto
        {
            Name = "Updated Item Name",
            PassedInspection = true,
            ReplacementValue = 1000,
            DailyRate = 40,
            Status = GearStatus.Available,
            Condition = GearConditionStatus.Good
        };

        var response = await Client.PutAsJsonAsync("/api/RentalService/gear/6748a68293acd000228f0000", updated);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task DeleteGear_RemovesItemFromDatabase()
    {
        var list = await Client.GetFromJsonAsync<List<GearItemDocument>>("/api/RentalService/gear");
        Assert.NotNull(list);
        var item = list.First();

        var response = await Client.DeleteAsync($"/api/RentalService/gear/{item.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var allAfter = await Client.GetFromJsonAsync<List<GearItemDocument>>("/api/RentalService/gear");

        Assert.NotNull(allAfter);
        Assert.Equal(1, allAfter.Count);
    }

    [Fact]
    public async Task DeleteGear_ReturnsNotFound_ForNonExistingId()
    {
        var response = await Client.DeleteAsync("/api/RentalService/gear/6748a68293acd000228f0000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
