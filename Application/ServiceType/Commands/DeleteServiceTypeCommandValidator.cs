using FluentValidation;

namespace Application.ServiceTypes.Commands
{
    public class DeleteServiceTypeCommandValidator : AbstractValidator<DeleteServiceTypeCommand>
    {
        public DeleteServiceTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ServiceType ID is required");
        }
    }
}
