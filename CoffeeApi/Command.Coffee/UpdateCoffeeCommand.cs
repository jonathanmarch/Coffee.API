using Coffee.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.API.Command.Coffee
{
    public class UpdateCoffeeCommand : IRequest<IActionResult>
    {
        public int CoffeeId { get; }
        
        public CoffeeItem CoffeeItem { get; }
        
        public UpdateCoffeeCommand(int coffeeCoffeeId, CoffeeItem coffeeItem)
        {
            this.CoffeeId = coffeeCoffeeId;
            this.CoffeeItem = coffeeItem;
        }
    }
}
