using FluentValidation;

namespace Application.InstrumentPassports.Commands
{
    public class UpdateInstrumentPassportCommandValidator : AbstractValidator<UpdateInstrumentPassportCommand>
    {
        public UpdateInstrumentPassportCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("InstrumentPassport Id is required");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithMessage("Issue date is required");

            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("Details are required")
                .MaximumLength(2000).WithMessage("Details max length is 2000 characters");
        }
    }
}
