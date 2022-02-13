using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : BaseDataAccessController<IRecipeDTO>
    {

        public RecipeController(IMediator mediator) : base(mediator) { }

        [Route("Find")]
        [HttpGet]
        public async Task<IRecipeDTO[]> Find(string ingredients, string cuisines = "", string traits="", int matchMinimum=1, int from=0, int to=5)
        {
            return await _mediator.Send(new RecipeSearchFeature.Query(ingredients.FromCSV(), cuisines.FromCSV(), 0, 10));
        }
    }
}
