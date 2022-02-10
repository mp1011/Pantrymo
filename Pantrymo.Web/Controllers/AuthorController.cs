using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : BaseDataAccessController<IAuthor>
    {
        public AuthorController(IMediator mediator) : base(mediator) { }        
    }
}
