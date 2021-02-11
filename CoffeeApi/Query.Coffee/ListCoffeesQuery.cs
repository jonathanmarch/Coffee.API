using System.Collections.Generic;
using Coffee.API.Models;
using MediatR;

namespace Coffee.API.Query.Coffee
{
    public class ListCoffeesQuery : IRequest<IEnumerable<CoffeeItem>>
    {
    }
}
