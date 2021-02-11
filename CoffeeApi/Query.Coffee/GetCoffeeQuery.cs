using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.API.Query.Coffee
{
    public class GetCoffeeQuery : IRequest<IActionResult>
    {
        public int CoffeeId { get; }
        
        public GetCoffeeQuery(int coffeeId)
        {
            this.CoffeeId = coffeeId;
        }
    }
}
