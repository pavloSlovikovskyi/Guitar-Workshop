using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using FluentValidation;

namespace Application.RepairOrders.Commands
{
    public class CreateRepairOrderCommandValidator : AbstractValidator<CreateRepairOrderCommand>
    {
        private readonly IInstrumentRepository _instrumentRepository;

        public CreateRepairOrderCommandValidator(IInstrumentRepository instrumentRepository)
        {
            _instrumentRepository = instrumentRepository;

            RuleFor(x => x.InstrumentId)
                .NotEmpty().WithMessage("Instrument ID is required")
                .MustAsync(InstrumentExists).WithMessage("Instrument does not exist");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date is required");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters");
        }

        private async Task<bool> InstrumentExists(InstrumentId instrumentId, CancellationToken cancellationToken)
        {
            try
            {
                var instrument = await _instrumentRepository.GetByIdAsync(instrumentId, cancellationToken);
                return instrument != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
