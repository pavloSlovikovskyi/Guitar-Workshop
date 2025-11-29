using FluentValidation;

namespace Application.Instruments.Commands;

public class CreateInstrumentCommandValidator : AbstractValidator<CreateInstrumentCommand>
{
    public CreateInstrumentCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(50).WithMessage("Model: max 50 chars").MinimumLength(5);

        RuleFor(x => x.SerialNumber)
            .NotEmpty().WithMessage("Serial number is required")
            .MaximumLength(100).WithMessage("Serial number: max 100 chars").MinimumLength(7);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status is invalid");

       
    }
}
