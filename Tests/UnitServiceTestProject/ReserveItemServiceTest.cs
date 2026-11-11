using MyWebApi.Interfaces;
using MyWebApi.Entities;
using MyWebApi.Services;
using Moq;
using Xunit;

namespace UnitServiceTestProject;

public class ReserveItemServiceTest
{
    [Fact]
    public async Task GetAverageDailyRate_WithMockedDataService_ReturnsCorrectResult()
    {
        var gearItems = new List<GearItem>
        {
            new GearItem { Id = "1", Name = "Tent", DailyRate = 10.0m },
            new GearItem { Id = "2", Name = "Sleeping Bag", DailyRate = 5.0m },
            new GearItem { Id = "3", Name = "Backpack", DailyRate = 7.5m }
        };

        var mockGearRepository = new Mock<IGearItemRepository>();
        mockGearRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(gearItems);

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        var mockContractRepository = new Mock<IGearRentalContractRepository>();

        var service = new RentalService(mockGearRepository.Object, mockCustomerRepository.Object, mockContractRepository.Object);

        var result = await service.GetAverageDailyRateAsync();

        Assert.Equal(7.5m, result);
        mockGearRepository.Verify(repo => repo.GetAllAsync(), Times.Once);

    }

    [Fact]
    public void GetAverageDailyRate_WithEmptyData_ReturnsZero()
    {
        var mockGearRepository = new Mock<IGearItemRepository>();
        mockGearRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<GearItem>());

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        var mockContractRepository = new Mock<IGearRentalContractRepository>();

        var service = new RentalService(mockGearRepository.Object, mockCustomerRepository.Object, mockContractRepository.Object);

        var result = service.GetAverageDailyRateAsync().Result;

        Assert.Null(result);
        mockGearRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

}
