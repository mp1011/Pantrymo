using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;

namespace Pantrymo.Application.Handlers
{
    public class DummyHandler : IRequestHandler<DummyQuery, IQueryable<Site>>
    {
        private readonly IDataContext _dataContext;

        public DummyHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<IQueryable<Site>> Handle(DummyQuery request, CancellationToken cancellationToken)
        {
            var result = _dataContext.Sites;
            var tk = Task.FromResult(result);
            return tk;
        }
    }
}
