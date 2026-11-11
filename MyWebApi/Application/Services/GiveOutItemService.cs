using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using MyWebApi.Exceptions;

namespace MyWebApi.Services;

public class GiveOutItemService : IGiveOutItemService
{
    private readonly IGearRentalContractRepository _contracts;
    private readonly IGearItemRepository _gearItems;
    private readonly ICustomerRepository _customers;
    public GiveOutItemService(IGearRentalContractRepository gearRentalContractRepository, IGearItemRepository gearItemRepository, ICustomerRepository customerRepository)
    {
        _contracts= gearRentalContractRepository;
        _gearItems = gearItemRepository;
        _customers = customerRepository;
    }

    public async Task<GearRentalContract>GiveOutItemAsync(GiveOutItemsDto request)
    {
        var contract = await _contracts.GetByIdAsync(request.contractId);
        if (contract is null)
            throw new NotFoundException("Нeмає контракту з таким id");

        if (contract.Customer is null)
            throw new BusinessRuleException("У договорі не вказаний клієнт");

        if (contract.Customer.IdCard != request.customerIdCard)
            throw new BusinessRuleException("Невірний ідентифікаційний документ клієнта");

        if (contract.Status != ContractStatus.Reserved)
            throw new BusinessRuleException("Можна забирати спорядження тільки якщо контракт попередньо зарезервований");

        if (contract.Items.Count == 0 || contract.Items.Any(item => item.Status != GearStatus.Reserved))
            throw new BusinessRuleException("Не все спорядження договору має статус Reserved");

        var depositAmount = contract.Items.Sum(item => item.ReplacementValue);
        var rentalCost = contract.Items.Sum(item => item.DailyRate * contract.PlannedRentalDays);
        var availableBalance = contract.Customer.CardBalance - contract.Customer.BlockedDeposit;
        if (availableBalance < depositAmount + rentalCost)
            throw new BusinessRuleException("Не вистачає грошей на рахунку щоб забронювати спорядження");

        foreach (var item in contract.Items)
        {
            var updatedItem = await _gearItems.UpdateAsync(
                item.Id, new UpdateGearItemDto{ Status = GearStatus.RentedOut});

            if (updatedItem is null)
                throw new BusinessRuleException($"Не вдалося оновити спорядження з id={item.Id}");

            item.Status = GearStatus.RentedOut;
        }

        contract.Customer.CardBalance -= rentalCost;
        contract.Customer.BlockedDeposit += depositAmount;

        contract.DepositAmount = depositAmount;
        contract.TotalPayAmount = rentalCost;

        contract.GiftBonus = contract.TotalPayAmount >= 3000m? 
            "Отримайте безкоштовно гермомішок або ремкомплект":
                contract.GiftBonus = "Візьміть ще товарів щоб отримати подарунок";

        contract.Status = ContractStatus.Active;
        
        var updateContract = new UpdateGearRentalContractDto
        {
            Customer = contract.Customer,
            Items = contract.Items,
            Status = contract.Status,
            DepositAmount = contract.DepositAmount,
            TotalPayAmount = contract.TotalPayAmount,
            GiftBonus = contract.GiftBonus,
            StartDate = contract.StartDate
        };

        var updatedContract = await _contracts.UpdateAsync(
            contract.Id,
            updateContract);

        if (updatedContract is null)
            throw new NotFoundException("Не вдалося знайти контракт для оновлення");
        
        return updatedContract;
    }
     

}