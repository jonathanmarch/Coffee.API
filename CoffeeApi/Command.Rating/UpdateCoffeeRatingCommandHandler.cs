using System;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Command.Coffee;
using Coffee.API.Exceptions;
using Coffee.API.Handlers;
using Coffee.API.Models;
using Coffee.API.Repositories;
using Coffee.API.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Command.Rating
{
    public class UpdateCoffeeRatingCommandHandler : ActionResultHandler<UpdateCoffeeRatingCommand>, IRequestHandler<UpdateCoffeeRatingCommand, IActionResult>
    {
        private readonly ICoffeeRatingRepository coffeeRatingRepository;
        private readonly IValidator<UpdateCoffeeRatingCommand> updateCoffeeRatingCommandValidator;
        private readonly ILogger logger;

        public UpdateCoffeeRatingCommandHandler(
            ICoffeeRatingRepository ratingRepository,
            IValidator<UpdateCoffeeRatingCommand> updateCoffeeRatingCommandValidator,
            ILoggerFactory logger)
        {
            this.coffeeRatingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(CoffeeRatingRepository));
            this.updateCoffeeRatingCommandValidator = updateCoffeeRatingCommandValidator ?? throw new ArgumentNullException(nameof(UpdateCoffeeRatingCommandValidator));
            this.logger = logger.CreateLogger(nameof(CreateCoffeeCommandHandler));
        }

        public async Task<IActionResult> Handle(UpdateCoffeeRatingCommand command, CancellationToken cancellationToken)
        {
            var validation = await this.updateCoffeeRatingCommandValidator.ValidateAsync(command, cancellationToken);

            logger.LogInformation($"Updating existing coffee rating with id: {command.CoffeeRatingId}");

            if (!validation.IsValid)
            {
                var validationErrorsDto = MapValidationErrorsDto(validation.Errors);

                logger.LogError($"Error updating a coffee rating with id: {command.CoffeeRatingId}, validation error occurred.");

                return BadRequest(validationErrorsDto);
            }

            try
            {
                await this.coffeeRatingRepository.Update(command.UpdateCoffeeRating);
            }
            catch (EntityNotFoundException)
            {
                logger.LogError($"Error updating a coffee with id: {command.CoffeeRatingId}, entity not found in database.");

                return NotFound();
            }

            logger.LogInformation($"Successfully updated coffee rating with id: {command.CoffeeRatingId}");

            return NoContent();
        }
    }
}
