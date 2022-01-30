using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;

namespace Pantrymo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SitesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("getByDate/{dateFrom}")]
        public async Task<Site[]> GetByDate(DateTime dateFrom)
        {
            return await _mediator.Send(new GetByDateQuery<Site>(dateFrom));
        }
    }
}
