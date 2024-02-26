using Application.Orders.Commands;
using FluentValidation;

namespace Application.Validators;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(value => value.Status)
            .NotEmpty()
            .Must(BeValidStatus).WithMessage("This status is not available.");
    }
    private bool BeValidStatus(string status)
    {
        return string.Equals(status, "paid", StringComparison.OrdinalIgnoreCase) 
            || string.Equals(status, "completed", StringComparison.OrdinalIgnoreCase);
    }
}
