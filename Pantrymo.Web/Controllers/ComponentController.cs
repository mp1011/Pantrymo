using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;

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
            return await _mediator.Send(new GetFullHierarchyQuery());
        }
    }
}
