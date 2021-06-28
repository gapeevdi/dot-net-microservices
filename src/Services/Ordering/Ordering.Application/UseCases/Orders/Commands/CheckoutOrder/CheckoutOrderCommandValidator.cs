using FluentValidation;

namespace Ordering.Application.UseCases.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(command => command.UserName)
                .NotEmpty()
                .WithMessage("'UserName' is required")
                .MaximumLength(50)
                .WithMessage("'UserName' must not exceed 50 characters")
                .NotNull();

            RuleFor(p => p.EmailAddress)
                .NotEmpty()
                .WithMessage("'EmailAddress' is required");

            RuleFor(p => p.TotalPrice)
                .NotEmpty()
                .WithMessage("{TotalPrice} is required")
                .GreaterThan(0)
                .WithMessage("'TotalPrice' should be greater than zero");
        }
    }
}
