using System;
using System.Collections.Generic;
using System.Linq;
using Coffee.API.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Coffee.API.Handlers
{
    public abstract class ActionResultHandler<T>
        where T : class
    {
        protected IActionResult NotFound() => new NotFoundResult();

        protected IActionResult Ok(object obj) => new OkObjectResult(obj);

        protected BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
            => new BadRequestObjectResult(error);

        public virtual CreatedResult Created(string uri, [ActionResultObjectValue] object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return new CreatedResult(uri, value);
        }
        
        protected IActionResult CreatedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object value)
            => new CreatedAtActionResult(actionName, controllerName: null, routeValues: routeValues, value: value);

        protected NoContentResult NoContent()
            => new NoContentResult();
        
        protected ValidationErrorsDto MapValidationErrorsDto(IList<ValidationFailure> validationErrors)
        {
            return new ValidationErrorsDto()
            {
                ValidationErrors = validationErrors.Select(i => new ValidationError(i.PropertyName.Split(".").Last(), i.ErrorMessage))
            };
        }
    }
}
