using FluentValidation;

namespace Application.RepairOrders.Commands;

public class UpdateRepairOrderCommandValidator : AbstractValidator<UpdateRepairOrderCommand>
{
    public UpdateRepairOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.InstrumentId)
            .NotEmpty().WithMessage("InstrumentId is required");

        RuleFor(x => x.OrderDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters");
    }
}
