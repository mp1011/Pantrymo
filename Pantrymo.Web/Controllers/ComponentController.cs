using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : BaseDataAccessController<Component>
    {
        public ComponentController(IMediator mediator) : base(mediator) { }

        [HttpGet()]
        [Route("FullHierarchy")]
        public async Task<FullHierarchy[]> GetFullHierarchy()
        {
            return await _mediator.Send(new FullHierarchyFeature.Query());
        }
    }
}
