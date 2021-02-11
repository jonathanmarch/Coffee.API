using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Handlers;
using Coffee.API.Models;
using Coffee.API.Repositories;
using Coffee.API.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Command.Coffee
{
    public class CreateCoffeeCommandHandler : ActionResultHandler<CreateCoffeeCommand>, IRequestHandler<CreateCoffeeCommand, IActionResult>
    {
        private readonly ICoffeeRepository coffeeRepository;
        private readonly IValidator<CreateCoffeeCommand> createCoffeeCommandValidator;
        private readonly ILogger logger;
        
        public CreateCoffeeCommandHandler(
            ICoffeeRepository coffeeRepository,
            IValidator<CreateCoffeeCommand> createCoffeeCommandValidator,
            ILoggerFactory logger)
        {
            this.coffeeRepository = coffeeRepository ?? throw new ArgumentNullException(nameof(CoffeeRepository));
            this.createCoffeeCommandValidator = createCoffeeCommandValidator ?? throw new ArgumentNullException(nameof(CreateCoffeeCommandValidator));
            this.logger = logger.CreateLogger(nameof(CreateCoffeeCommandHandler));
        }

        public async Task<IActionResult> Handle(CreateCoffeeCommand command, CancellationToken cancellationToken)
        {
            var validation = await this.createCoffeeCommandValidator.ValidateAsync(command, cancellationToken);

            logger.LogInformation("Creating a new coffee.");
            
            if (!validation.IsValid)
            {
                var validationErrorsDto = MapValidationErrorsDto(validation.Errors);

                logger.LogError($"Error creating a new coffee, validation error occurred.");

                return BadRequest(validationErrorsDto);
            }

            var createCoffeeItemId = await this.coffeeRepository.Create(command.CoffeeItem);

            logger.LogInformation($"Successfully created a new coffee with id: {createCoffeeItemId}");

            return CreatedAtAction("GetCoffee", new { coffeeId = createCoffeeItemId }, new { Id = createCoffeeItemId });
        }
    }
}
