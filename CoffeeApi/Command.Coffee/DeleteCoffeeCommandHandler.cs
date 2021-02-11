using System;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Handlers;
using Coffee.API.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Command.Coffee
{
    public class DeleteCommandHandler : ActionResultHandler<DeleteCoffeeCommand>, IRequestHandler<DeleteCoffeeCommand, IActionResult>
    {
        private readonly ICoffeeRepository coffeeRepository;
        private readonly ILogger logger;

        public DeleteCommandHandler(
            ICoffeeRepository coffeeRepository,
            ILoggerFactory logger)
        {
            this.coffeeRepository = coffeeRepository ?? throw new ArgumentNullException(nameof(CoffeeRepository));
            this.logger = logger.CreateLogger(nameof(CreateCoffeeCommandHandler));
        }

        public async Task<IActionResult> Handle(DeleteCoffeeCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Deleting coffee with id: {command.CoffeeId}");

            var coffee = await coffeeRepository.FindById(command.CoffeeId);
            
            if (coffee == null)
            {
                this.logger.LogWarning($"Cannot find coffee with id: {command.CoffeeId}");
                return NotFound();
            }

            await coffeeRepository.Delete(coffee);

            logger.LogInformation($"Successfully deleted coffee with id: {command.CoffeeId}");
            
            return Ok(coffee);
        }
    }
}
