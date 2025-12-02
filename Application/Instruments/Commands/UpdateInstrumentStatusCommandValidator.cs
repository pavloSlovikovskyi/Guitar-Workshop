using FluentValidation;

namespace Application.Instruments.Commands
{
    public class UpdateInstrumentStatusCommandValidator : AbstractValidator<UpdateInstrumentStatusCommand>
    {
        public UpdateInstrumentStatusCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
