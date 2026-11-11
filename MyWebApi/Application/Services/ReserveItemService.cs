using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using MyWebApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApi.Services;
public class ReserveItemService : IReserveItemService
{
    private readonly IGearItemRepository _gearItems;
    private readonly ICustomerRepository _customers;
    private readonly IGearRentalContractRepository _contracts;
    public ReserveItemService(IGearItemRepository gearItemRepository, ICustomerRepository customerRepository, IGearRentalContractRepository gearRentalContractRepository)
    {
        _gearItems = gearItemRepository;
        _customers = customerRepository;
        _contracts= gearRentalContractRepository;
    }
    public async Task<GearRentalContract> ReserveAsync(ReserveItemDto request)
    {
        GearRentalContract contract = new GearRentalContract();
        contract.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

        var tempCustomer = await _customers.GetByIdCardAsync(request.CustomerIdCard);
        if (tempCustomer == null)
            throw new NotFoundException($"Немає клієнтів з таким idCard={request.CustomerIdCard}");
        contract.Customer = tempCustomer;

        List<GearItem> tempGearItems = new();
        if (request.ItemsId == null || request.ItemsId.Count == 0)
            throw new BusinessRuleException("Вкажіть щонайменше одну одиницю спорядження");
        if (request.ItemsId.Distinct().Count() != request.ItemsId.Count)
            throw new BusinessRuleException("Не можна вказувати однакове спорядження кілька разів");

        foreach (string id in request.ItemsId)
        {
            var item = await _gearItems.GetByIdAsync(id);
            if (item == null)
                throw new NotFoundException($"Немає спорядження з таким id = {id}");
            if (item.Status != GearStatus.Available)
                throw new BusinessRuleException($"Спорядження з id = {id} не доступне");
            if (!item.PassedInspection)
                throw new BusinessRuleException($"Спорядження з id = {id} не пройшло технічний огляд");
            tempGearItems.Add(item);
        }

        contract.Items = tempGearItems;

        contract.PlannedEndDate = request.PlannedEndDate;
        if (contract.PlannedEndDate <= contract.StartDate)
            throw new BusinessRuleException("Дата завершення оренди повинна бути пізніше дати початку");
        if (contract.PlannedRentalDays < 2)
            throw new BusinessRuleException("Мінімальна тривалість оренди становить 2 дні");

        contract.Status = ContractStatus.Reserved;

        foreach (var item in tempGearItems)
        {
            var updatedItem = await _gearItems.UpdateAsync(
                item.Id, new UpdateGearItemDto{ Status = GearStatus.Reserved});

            if (updatedItem is null)
                throw new BusinessRuleException($"Не вдалося оновити спорядження id={item.Id}");

            item.Status = updatedItem.Status;
        }
        await _contracts.CreateAsync(contract);
        return contract;
    }
}