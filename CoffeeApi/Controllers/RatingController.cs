using System.Collections.Generic;
using System.Threading.Tasks;
using Coffee.API.Command.Coffee;
using Coffee.API.Command.Rating;
using Coffee.API.Models;
using Coffee.API.Query.Rating;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using static Coffee.API.Constants;

namespace Coffee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController
    {
        private readonly IMediator mediator;

        public RatingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Lists all of the coffee ratings.
        /// </summary>
        /// <returns>List of the coffee ratings.</returns>
        [HttpGet]
        public async Task<IEnumerable<CoffeeRating>> ListCoffeeRatings()
        {
            return await mediator.Send(new ListRatingQuery());
        }

        /// <summary>
        /// Creates a single rating.
        /// </summary>
        /// <param name="CoffeeRating"></param>
        /// <returns>Created rating.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRating(CoffeeRatingDto coffeeRating)
        {
            return await mediator.Send(new CreateCoffeeRatingCommand(coffeeRating));
        }


        /// <summary>
        /// Updates a single coffee rating.
        /// </summary>
        /// <param name="coffeeRatingId">Coffee rating ID</param>
        /// <param name="updateCoffeeRating">Coffee rating Object</param>
        /// <returns>Updated coffee rating.</returns>
        [HttpPut(RouteConstants.UpdateCoffeeRating)]
        public async Task<IActionResult> UpdateRating(int coffeeRatingId, UpdateCoffeeRatingDto updateCoffeeRating)
        {
            return await mediator.Send(new UpdateCoffeeRatingCommand(coffeeRatingId, updateCoffeeRating));
        }
    }
}
