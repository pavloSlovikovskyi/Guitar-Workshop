using API.Dtos;
using FluentValidation;

namespace API.Modules.Validators;

public class CreateServiceTypeRequestValidator : AbstractValidator<CreateServiceTypeRequest>
{
    public CreateServiceTypeRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
