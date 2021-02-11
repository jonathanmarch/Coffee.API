using Coffee.API.Command.Coffee;
using FluentValidation;

namespace Coffee.API.Validators
{
    public class CreateCoffeeCommandValidator : AbstractValidator<CreateCoffeeCommand>
    {
        public CreateCoffeeCommandValidator()
        {
            RuleFor(command => command.CoffeeItem.Name)
                .NotNull().NotEmpty().WithMessage("Name is required.")
                .Length(3, 50).WithMessage("Name should be between 3 and 50 characters.");
            
            RuleFor(command => command.CoffeeItem.Description)
                .NotNull().NotEmpty().WithMessage("Description is required.")
                .Length(3, 150).WithMessage("Description should be between 3 and 150 characters.");
            
            RuleFor(command => command.CoffeeItem.CaffeineContent)
                .NotNull().WithMessage("Caffeine Content is required.")
                .InclusiveBetween(5, 400).WithMessage("Caffeine Content should be between 5 and 400.");
        }
    }
}
