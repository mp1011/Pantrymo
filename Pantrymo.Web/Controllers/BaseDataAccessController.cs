using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Application.Features;
using Pantrymo.Domain.Models;

namespace Pantrymo.Web.Controllers
{
    public class BaseDataAccessController<T> : ControllerBase
        where T:IWithLastModifiedDate
    {
        protected readonly IMediator _mediator;

        public BaseDataAccessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("GetByDate/{dateFrom}")]
        public async Task<T[]> GetByDate(DateTime dateFrom) 
            => await _mediator.Send(new GetByDateFeature.Query<T>(dateFrom));
    }
}
