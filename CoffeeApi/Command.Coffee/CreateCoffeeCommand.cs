using Coffee.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.API.Command.Coffee
{
    public class CreateCoffeeCommand : IRequest<IActionResult>
    {
        public CoffeeItemDto CoffeeItem { get; }

        public CreateCoffeeCommand(CoffeeItemDto coffeeItem)
        {
            this.CoffeeItem = coffeeItem;
        }
    }
}