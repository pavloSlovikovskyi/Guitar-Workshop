using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Instruments.Commands
{
    public class UpdateInstrumentCommandValidator : AbstractValidator<UpdateInstrumentCommand>
    {
        private readonly IInstrumentRepository _instrumentRepository;

        public UpdateInstrumentCommandValidator(IInstrumentRepository instrumentRepository)
        {
            _instrumentRepository = instrumentRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Instrument ID is required")
                .MustAsync(InstrumentExists).WithMessage("Instrument does not exist");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(50).WithMessage("Model: max 50 chars")
                .MinimumLength(5).WithMessage("Model: min 5 chars");

            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage("Serial number is required")
                .MaximumLength(100).WithMessage("Serial number: max 100 chars")
                .MinimumLength(7).WithMessage("Serial number: min 7 chars");
        }

        private async Task<bool> InstrumentExists(InstrumentId id, CancellationToken cancellationToken)
        {
            try
            {
                var instrument = await _instrumentRepository.GetByIdAsync(id, cancellationToken);
                return instrument != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
