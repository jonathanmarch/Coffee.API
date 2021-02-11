using Coffee.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.API.Command.Rating
{
    public class CreateCoffeeRatingCommand : IRequest<IActionResult>
    {
        public CoffeeRatingDto CoffeeRating { get; }

        public CreateCoffeeRatingCommand(CoffeeRatingDto coffeeRating)
        {
            this.CoffeeRating = coffeeRating;
        }
    }
}
