using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : BaseDataAccessController<ISite>
    {
        public SiteController(IMediator mediator) : base(mediator) { }        
    }
}
