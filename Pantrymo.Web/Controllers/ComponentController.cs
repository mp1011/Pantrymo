using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : BaseDataAccessController<Component>
    {
        public ComponentController(IMediator mediator) : base(mediator) { }
        
    }
}
