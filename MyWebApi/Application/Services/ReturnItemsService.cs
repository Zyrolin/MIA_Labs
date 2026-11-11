using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using MyWebApi.Exceptions;

namespace MyWebApi.Services;

public class ReturnItemService : IReturnItemsService
{
    private readonly IGearItemRepository _gearItems;
    private readonly ICustomerRepository _customers;
    private readonly IGearRentalContractRepository _contracts;
    public ReturnItemService(IGearItemRepository gearItemRepository, ICustomerRepository customerRepository, IGearRentalContractRepository gearRentalContractRepository)
    {
        _gearItems = gearItemRepository;
        _customers = customerRepository;
        _contracts= gearRentalContractRepository;
    }
    public async Task<GearRentalContract> ReturnAsync(ReturnItemsDto request)
    {
        if(request.contractId is null)
            throw new BusinessRuleException("Вкажіть id контракту");

        if (request.ActualEndDate is null)
            throw new BusinessRuleException("Вкажіть фактичну дату повернення");

        var contract = await _contracts.GetByIdAsync(request.contractId);

        if (contract is null)
            throw new NotFoundException($"Контракт з id={request.contractId} не знайдено");

        if (contract.Status != ContractStatus.Active)
            throw new BusinessRuleException("Договір не є активним");

        if (contract.Customer is null)
            throw new BusinessRuleException("У договорі не вказаний клієнт");

        if (request.gearConditions is null ||
            contract.Items.Count != request.gearConditions.Count)
        {
            throw new BusinessRuleException("Кількість статусів спорядження та кількість спорядження різна");
        }

        contract.ActualEndDate = request.ActualEndDate.Value;

        decimal additionalCharges = 0m;
        int overdueDays = contract.OverdueDays;

        for (int i = 0; i < contract.Items.Count; i++)
        {
            var item = contract.Items[i];
            var condition = request.gearConditions[i];

            if (overdueDays > 0)
                additionalCharges += item.DailyRate * 2 * overdueDays;

            if (condition == GearConditionStatus.Broken)
                additionalCharges += item.ReplacementValue / 2;
            else if (condition == GearConditionStatus.CanNotBeUsed)
                additionalCharges += item.ReplacementValue;

            var newStatus = condition == GearConditionStatus.Good
                ? GearStatus.Available
                : GearStatus.Maintence;

            var updatedItem = await _gearItems.UpdateAsync(
                item.Id,
                new UpdateGearItemDto
                {
                    Status = newStatus,
                    Condition = condition
                });

            if (updatedItem is null)
                throw new BusinessRuleException($"Не вдалося оновити спорядження id={item.Id}");

            item.Status = newStatus;
            item.Condition = condition;
        }

        var customer = contract.Customer;
        var depositUsed = Math.Min(contract.DepositAmount, additionalCharges);
        var remainingCharges = additionalCharges - depositUsed;

        customer.BlockedDeposit = Math.Max(0m, customer.BlockedDeposit - contract.DepositAmount);
        customer.CardBalance -= depositUsed;

        var availableBalance = Math.Max(0m, customer.CardBalance - customer.BlockedDeposit);
        var extraAmountPaid = Math.Min(availableBalance, remainingCharges);

        customer.CardBalance -= extraAmountPaid;
        customer.OutstandingDebt += remainingCharges - extraAmountPaid;

        contract.TotalRentalCost = contract.TotalPayAmount + additionalCharges;
        contract.Status = ContractStatus.Completed;

        var updatedCustomer = await _customers.UpdateAsync(
        customer.UserId,
        new UpdateCustomerDto
        {
            CardBalance = customer.CardBalance,
            BlockedDeposit = customer.BlockedDeposit,
            OutstandingDebt = customer.OutstandingDebt
        });

        if (updatedCustomer is null)
            throw new BusinessRuleException("Не вдалося зберегти зміни клієнта");

        var updatedContract = await _contracts.UpdateAsync(
            contract.Id,
            new UpdateGearRentalContractDto
            {
                Customer = updatedCustomer,
                Items = contract.Items,
                ActualEndDate = contract.ActualEndDate,
                Status = contract.Status,
                TotalRentalCost = contract.TotalRentalCost
            });

        if (updatedContract is null)
            throw new NotFoundException("Не вдалося зберегти зміни контракту");
        
        return updatedContract;
    }
}