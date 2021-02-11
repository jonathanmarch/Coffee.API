using Coffee.API.Command.Rating;
using FluentValidation;

namespace Coffee.API.Validators
{
    public class UpdateCoffeeRatingCommandValidator : AbstractValidator<UpdateCoffeeRatingCommand>
    {
        public UpdateCoffeeRatingCommandValidator()
        {
            RuleFor(command => command.UpdateCoffeeRating.Id)
                .NotNull().NotEmpty().WithMessage("Id is required.");

            RuleFor(command => command.UpdateCoffeeRating.Id)
                .Must((command, val) => command.CoffeeRatingId == val).WithMessage("Id must match route parameter.");


            RuleFor(command => command.UpdateCoffeeRating.Comment)
                .NotNull().NotEmpty().WithMessage("Comment is required.")
                .Length(5, 150).WithMessage("Comment should be between 5 and 150 characters.");

            RuleFor(command => command.UpdateCoffeeRating.Rating)
                .NotNull().NotEmpty().WithMessage("Rating is required.")
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
        }
    }
}