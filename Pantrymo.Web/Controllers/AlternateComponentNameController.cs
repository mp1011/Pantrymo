using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlternateComponentNameController : BaseDataAccessController<IAlternateComponentName>
    {
        public AlternateComponentNameController(IMediator mediator) : base(mediator) { }        
    }
}
