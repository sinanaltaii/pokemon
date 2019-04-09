﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Pokemon.Api.Validation;
using Pokemon.Core.Models;
using Pokemon.Core.Services;
using System.Linq;

namespace Pokemon.Api.Filters
{
    public class ValidatePokemonDtoModelAttribute : ActionFilterAttribute
    {
        private IPokemonService _pokemonService;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _pokemonService = context.HttpContext.RequestServices.GetService<IPokemonService>();

            if (context.ActionArguments.Values.FirstOrDefault() is PokemonForCreationDto)
            {
                var dto = new PokemonForCreationDto();
                dto = context.ActionArguments.Values.Cast<PokemonForCreationDto>().FirstOrDefault();

                if (dto != null)
                {
                    if (string.IsNullOrEmpty(dto.ImageUrl))
                    {
                        context.ModelState.AddModelError(nameof(dto.ImageUrl), "Image is required.");
                    }

                    if (!_pokemonService.NameIsUnique(dto.Name))
                    {
                        context.ModelState.AddModelError(nameof(dto.Name), "Name is not unique.");
                    }
                }
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
