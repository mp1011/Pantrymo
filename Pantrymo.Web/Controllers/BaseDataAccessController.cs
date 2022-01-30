using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;

namespace Pantrymo.Web.Controllers
{
    public class BaseDataAccessController<T> : ControllerBase
        where T:IWithLastModifiedDate
    {
        private readonly IMediator _mediator;

        public BaseDataAccessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("getByDate/{dateFrom}")]
        public async Task<T[]> GetByDate(DateTime dateFrom) 
            => await _mediator.Send(new GetByDateQuery<T>(dateFrom));
    }
}
