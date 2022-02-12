using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeDTOController : BaseDataAccessController<IRecipeDTO>
    {
        public RecipeDTOController(IMediator mediator) : base(mediator) { }
    }
}
