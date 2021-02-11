using Coffee.API.Command.Rating;
using FluentValidation;

namespace Coffee.API.Validators
{
    public class CreateCoffeeRatingCommandValidator : AbstractValidator<CreateCoffeeRatingCommand>
    {
        public CreateCoffeeRatingCommandValidator()
        {
            RuleFor(command => command.CoffeeRating.CoffeeId)
                .NotNull().NotEmpty().WithMessage("Coffee Id is required.");
            
            RuleFor(command => command.CoffeeRating.Comment)
                .NotNull().NotEmpty().WithMessage("Comment is required.")
                .Length(5, 150).WithMessage("Comment should be between 5 and 150 characters.");

            RuleFor(command => command.CoffeeRating.Rating)
                .NotNull().NotEmpty().WithMessage("Rating is required.")
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
        }
    }
}
