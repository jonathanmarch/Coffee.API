using Coffee.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.API.Command.Rating
{
    public class UpdateCoffeeRatingCommand : IRequest<IActionResult>
    {
        public int CoffeeRatingId { get; }

        public UpdateCoffeeRatingDto UpdateCoffeeRating { get; }

        public UpdateCoffeeRatingCommand(int coffeeRatingId, UpdateCoffeeRatingDto updateCoffeeRating)
        {
            this.CoffeeRatingId = coffeeRatingId;
            this.UpdateCoffeeRating = updateCoffeeRating;
        }
    }
}
