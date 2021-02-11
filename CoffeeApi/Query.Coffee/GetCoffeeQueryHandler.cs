using System;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Handlers;
using Coffee.API.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Query.Coffee
{
    public class GetCoffeeQueryHandler : ActionResultHandler<GetCoffeeQuery>, IRequestHandler<GetCoffeeQuery, IActionResult>
    {
        private readonly ICoffeeRepository coffeeRepository;
        private readonly ILogger logger;
        
        public GetCoffeeQueryHandler(ICoffeeRepository coffeeRepository, ILoggerFactory logger)
        {
            this.coffeeRepository = coffeeRepository ?? throw new ArgumentNullException(nameof(CoffeeRepository));
            this.logger = logger.CreateLogger(nameof(GetCoffeeQueryHandler));
        }

        public async Task<IActionResult> Handle(GetCoffeeQuery request, CancellationToken cancellationToken)
        {
            var coffee = await coffeeRepository.FindById(request.CoffeeId);

            this.logger.LogInformation($"Fetching coffee with id: {request.CoffeeId}");
            
            if (coffee == null)
            {
                this.logger.LogWarning($"Cannot find coffee with id: {request.CoffeeId}");
                return NotFound();
            }

            return Ok(coffee);
        }
    }
}
