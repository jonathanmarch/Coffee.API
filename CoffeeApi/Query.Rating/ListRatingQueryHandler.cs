using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Models;
using Coffee.API.Query.Coffee;
using Coffee.API.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Query.Rating
{
    public class ListRatingQueryHandler : IRequestHandler<ListRatingQuery, IEnumerable<CoffeeRating>>
    {
        private readonly ICoffeeRatingRepository coffeeRatingRepository;
        private readonly ILogger logger;

        public ListRatingQueryHandler(ICoffeeRatingRepository coffeeRatingRepository, ILoggerFactory logger)
        {
            this.coffeeRatingRepository = coffeeRatingRepository ?? throw new ArgumentNullException(nameof(CoffeeRepository));
            this.logger = logger.CreateLogger(nameof(ListCoffeesQueryHandler));
        }

        public async Task<IEnumerable<CoffeeRating>> Handle(ListRatingQuery request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Fetching a list of all rating.");

            return await coffeeRatingRepository.GetAllItems();
        }
    }
}
