using System;
using System.Threading;
using System.Threading.Tasks;
using Coffee.API.Command.Coffee;
using Coffee.API.Exceptions;
using Coffee.API.Handlers;
using Coffee.API.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffee.API.Command.Rating
{
    public class CreateCoffeeRatingCommandHandler : ActionResultHandler<CreateCoffeeRatingCommand>, IRequestHandler<CreateCoffeeRatingCommand, IActionResult>
    {
        private readonly ICoffeeRatingRepository coffeeRatingRepository;
        private readonly IValidator<CreateCoffeeRatingCommand> createCoffeeRatingCommandValidator;
        private readonly ILogger logger;

        public CreateCoffeeRatingCommandHandler(
            ICoffeeRatingRepository ratingRepository,
            IValidator<CreateCoffeeRatingCommand> createCoffeeRatingCommandValidator,
            ILoggerFactory logger)
        {
            this.coffeeRatingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(CoffeeRatingRepository));
            this.createCoffeeRatingCommandValidator = createCoffeeRatingCommandValidator ?? throw new ArgumentNullException(nameof(createCoffeeRatingCommandValidator));
            this.logger = logger.CreateLogger(nameof(CreateCoffeeCommandHandler));
        }

        public async Task<IActionResult> Handle(CreateCoffeeRatingCommand command, CancellationToken cancellationToken)
        {
            var validation = await this.createCoffeeRatingCommandValidator.ValidateAsync(command, cancellationToken);

            logger.LogInformation("Creating a new rating.");

            if (!validation.IsValid)
            {
                var validationErrorsDto = MapValidationErrorsDto(validation.Errors);

                logger.LogError($"Error creating a new rating, validation error occurred.");

                return BadRequest(validationErrorsDto);
            }

            var createRatingId = 0;

            try
            {
                createRatingId = await this.coffeeRatingRepository.Create(command.CoffeeRating);
            }
            catch (EntityNotFoundException)
            {
                logger.LogError($"Error creating relationship with rating, a coffee with id: {command.CoffeeRating.CoffeeId} entity not found in database.");

                return NotFound();
            }
            
            logger.LogInformation($"Successfully created a new rating with id: {createRatingId}");

            return Created("Rating",  new { Id = createRatingId });
        }
    }
}
