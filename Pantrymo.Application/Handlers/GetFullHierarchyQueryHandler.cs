using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Handlers
{
    public class GetFullHierarchyQueryHandler : IRequestHandler<GetFullHierarchyQuery, FullHierarchy[]>
    {
        private readonly IFullHierarchyLoader _fullHierarchyLoader;

        public GetFullHierarchyQueryHandler(IFullHierarchyLoader fullHierarchyLoader)
        {
            _fullHierarchyLoader = fullHierarchyLoader;
        }

        public async Task<FullHierarchy[]> Handle(GetFullHierarchyQuery request, CancellationToken cancellationToken)
        {
            return await _fullHierarchyLoader.GetFullHierarchy();
        }
    }
}
