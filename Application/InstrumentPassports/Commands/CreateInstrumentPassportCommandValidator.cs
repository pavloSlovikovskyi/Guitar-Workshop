using FluentValidation;

namespace Application.InstrumentPassports.Commands
{
    public class CreateInstrumentPassportCommandValidator : AbstractValidator<CreateInstrumentPassportCommand>
    {
        public CreateInstrumentPassportCommandValidator()
        {
            RuleFor(x => x.InstrumentId)
                .NotEmpty().WithMessage("InstrumentId is required");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithMessage("Issue date is required");

            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("Details are required")
                .MaximumLength(2000).WithMessage("Details max length is 2000 characters");
        }
    }
}
