using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisineController : BaseDataAccessController<ICuisine>
    {
        public CuisineController(IMediator mediator) : base(mediator) { }        
    }
}
