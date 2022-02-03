using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Domain.Extensions;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("Find")]
        [HttpGet]
        public async Task<RecipeDetail[]> Find(string ingredients, string cuisines = "", string traits="", int matchMinimum=1, int from=0, int to=5)
        {
            return await _mediator.Send(new RecipeSearchFeature.Query(ingredients.FromCSV(), cuisines.FromCSV(), 0, 10));
        }
    }
}
