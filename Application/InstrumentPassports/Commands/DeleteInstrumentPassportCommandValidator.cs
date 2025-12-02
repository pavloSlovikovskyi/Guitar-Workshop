using FluentValidation;

namespace Application.InstrumentPassports.Commands
{
    public class DeleteInstrumentPassportCommandValidator : AbstractValidator<DeleteInstrumentPassportCommand>
    {
        public DeleteInstrumentPassportCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("InstrumentPassport Id is required");
        }
    }
}
