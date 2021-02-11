using System.Collections.Generic;
using System.Threading.Tasks;
using Coffee.API.Command.Coffee;
using Coffee.API.Models;
using Coffee.API.Query.Coffee;
using Microsoft.AspNetCore.Mvc;
using MediatR;

using static Coffee.API.Constants;

namespace Coffee.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly IMediator mediator;

        public CoffeeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Lists all of the coffees.
        /// </summary>
        /// <returns>List of the coffees.</returns>
        [HttpGet]
        public async Task<IEnumerable<CoffeeItem>> ListCoffees()
        {
            return await mediator.Send(new ListCoffeesQuery());
        }

        /// <summary>
        /// Lists a single coffee.
        /// </summary>
        /// <param name="coffeeId">Coffee ID</param>
        /// <returns>Coffee item.</returns>
        [HttpGet(RouteConstants.GetCoffee)]
        public async Task<IActionResult> GetCoffee(int coffeeId)
        {
            return await mediator.Send(new GetCoffeeQuery(coffeeId));
        }

        /// <summary>
        /// Creates a single coffee.
        /// </summary>
        /// <param name="coffeeItem"></param>
        /// <returns>Created coffee item.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCoffee(CoffeeItemDto coffeeItem)
        {
            return await mediator.Send(new CreateCoffeeCommand(coffeeItem));
        }

        /// <summary>
        /// Updates a single coffee.
        /// </summary>
        /// <param name="coffeeId">Coffee ID</param>
        /// <param name="coffeeItem">Coffee Object</param>
        /// <returns>Updated coffee.</returns>
        [HttpPut(RouteConstants.UpdateCoffee)]
        public async Task<IActionResult> UpdateCoffee(int coffeeId, CoffeeItem coffeeItem)
        {
            return await mediator.Send(new UpdateCoffeeCommand(coffeeId, coffeeItem));
        }

        /// <summary>
        /// Deleres a single coffee.
        /// </summary>
        /// <param name="coffeeId">Coffee ID</param>
        /// <returns>Updated coffee.</returns>
        [HttpDelete(RouteConstants.DeleteCoffee)]
        public async Task<IActionResult> DeleteCoffee(int coffeeId)
        {
            return await mediator.Send(new DeleteCoffeeCommand(coffeeId));
        }
    }
}
