using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.API.Command.Coffee
{
    public class DeleteCoffeeCommand : IRequest<IActionResult>
    {
        public int CoffeeId { get; }

        public DeleteCoffeeCommand(int coffeeId)
        {
            this.CoffeeId = coffeeId;
        }
    }
}
