using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Models;
using Coffee.API.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Query.Coffee
{
    public class ListCoffeesQueryHandler : IRequestHandler<ListCoffeesQuery, IEnumerable<CoffeeItem>>
    {
        private readonly ICoffeeRepository coffeeRepository;
        private readonly ILogger logger;
        
        public ListCoffeesQueryHandler(ICoffeeRepository coffeeRepository, ILoggerFactory logger)
        {
            this.coffeeRepository = coffeeRepository ?? throw new ArgumentNullException(nameof(CoffeeRepository));
            this.logger = logger.CreateLogger(nameof(ListCoffeesQueryHandler));
        }
        
        public async Task<IEnumerable<CoffeeItem>> Handle(ListCoffeesQuery request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Fetching a list of all coffees.");

            return  await coffeeRepository.GetAllItems();
        }
    }
}
