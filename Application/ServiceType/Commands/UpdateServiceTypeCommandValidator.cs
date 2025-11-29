using FluentValidation;

namespace Application.ServiceTypes.Commands;

public class UpdateServiceTypeCommandValidator : AbstractValidator<UpdateServiceTypeCommand>
{
    public UpdateServiceTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title max length is 200");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description max length is 1000");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
