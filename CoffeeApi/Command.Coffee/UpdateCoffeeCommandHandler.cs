using System;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Exceptions;
using Coffee.API.Handlers;
using Coffee.API.Repositories;
using Coffee.API.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Command.Coffee
{
    public class UpdateCoffeeCommandHandler : ActionResultHandler<UpdateCoffeeCommand>, IRequestHandler<UpdateCoffeeCommand, IActionResult>
    {
        private readonly ICoffeeRepository coffeeRepository;
        private readonly IValidator<UpdateCoffeeCommand> updateCoffeeCommandValidator;
        private readonly ILogger logger;
        
        public UpdateCoffeeCommandHandler(
            ICoffeeRepository coffeeRepository,
            IValidator<UpdateCoffeeCommand> updateCoffeeCommandValidator, 
            ILoggerFactory logger)
        {
            this.coffeeRepository = coffeeRepository ?? throw new ArgumentNullException(nameof(CoffeeRepository));
            this.updateCoffeeCommandValidator = updateCoffeeCommandValidator ?? throw new ArgumentNullException(nameof(UpdateCoffeeCommandValidator));
            this.logger = logger.CreateLogger(nameof(UpdateCoffeeCommandHandler));
        }

        public async Task<IActionResult> Handle(UpdateCoffeeCommand command, CancellationToken cancellationToken)
        {
            var validation = await this.updateCoffeeCommandValidator.ValidateAsync(command, cancellationToken);

            logger.LogInformation($"Updating existing coffee with id: {command.CoffeeId}");
            
            if (!validation.IsValid)
            {
                var validationErrorsDto = MapValidationErrorsDto(validation.Errors);

                logger.LogError($"Error updating a coffee with id: {command.CoffeeId}, validation error occurred.");
                
                return BadRequest(validationErrorsDto);
            }

            try
            {
                await this.coffeeRepository.Update(command.CoffeeItem);
            }
            catch (EntityNotFoundException)
            {
                logger.LogError($"Error updating a coffee with id: {command.CoffeeId}, entity not found in database.");
                
                return NotFound();
            }

            logger.LogInformation($"Successfully updated coffee with id: {command.CoffeeId}");
            
            return NoContent();
        }
    }
}
