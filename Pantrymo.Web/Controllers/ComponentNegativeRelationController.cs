using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentNegativeRelationController : BaseDataAccessController<IComponentNegativeRelation>
    {
        public ComponentNegativeRelationController(IMediator mediator) : base(mediator) { }        
    }
}
