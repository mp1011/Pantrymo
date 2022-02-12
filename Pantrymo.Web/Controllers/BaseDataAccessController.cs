using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pantrymo.Domain.Features;
using Pantrymo.Domain.Models;

namespace Pantrymo.Web.Controllers
{
    public class BaseDataAccessController<T> : ControllerBase
        where T:IWithLastModifiedDate, IWithId
    {
        protected readonly IMediator _mediator;

        public BaseDataAccessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("GetByDate/{dateFrom}")]
        public async Task<T[]> GetByDate(DateTime dateFrom) 
            => await _mediator.Send(new DataAccessFeature.ByDateQuery<T>(dateFrom));

        [HttpPost]
        [Route("GetUpdatedRecords")]
        public async Task<T[]> GetUpdatedRecords(RecordUpdateTimestamp[] localTimestamps)
        {
            var result = await _mediator.Send(new DataAccessFeature.ChangedRecordsQuery<T>(localTimestamps));
            return result;
        }
    }
}
