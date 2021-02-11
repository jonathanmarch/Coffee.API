using System.Collections.Generic;
using Coffee.API.Models;
using MediatR;

namespace Coffee.API.Query.Rating
{
    public class ListRatingQuery : IRequest<IEnumerable<CoffeeRating>>
    {
    }
}
