using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Features
{
    public class FullHierarchyFeature
    {
        public record Query() : IRequest<FullHierarchy[]> { }

        public class Handler : IRequestHandler<Query, FullHierarchy[]>
        {
            private readonly IFullHierarchyLoader _fullHierarchyLoader;

            public Handler(IFullHierarchyLoader fullHierarchyLoader)
            {
                _fullHierarchyLoader = fullHierarchyLoader;
            }

            public async Task<FullHierarchy[]> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _fullHierarchyLoader.GetFullHierarchy();
            }
        }
    }
}
