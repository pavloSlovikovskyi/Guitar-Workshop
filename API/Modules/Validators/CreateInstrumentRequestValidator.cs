using API.Dtos;
using FluentValidation;

namespace API.Modules.Validators;

public class CreateInstrumentRequestValidator : AbstractValidator<CreateInstrumentRequest>
{
    public CreateInstrumentRequestValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MinimumLength(5).WithMessage("Model: min 5 chars")
            .MaximumLength(50).WithMessage("Model: max 50 chars");

        RuleFor(x => x.SerialNumber)
            .NotEmpty().WithMessage("Serial number is required")
            .MinimumLength(7).WithMessage("Serial number: min 7 chars")
            .MaximumLength(100).WithMessage("Serial number: max 100 chars");
    }
}
