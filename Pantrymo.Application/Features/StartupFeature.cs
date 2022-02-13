using MediatR;
using Pantrymo.Application.Services;
using Pantrymo.Domain.Services;

namespace Pantrymo.Application.Features
{
    public class StartupFeature
    {
        public class Command : IRequest { }

        public class Handler : IRequestHandler<Command>
        {
            private readonly CategoryService _categoryService;
            private readonly IDataSyncService _dataSyncService;

            public Handler(CategoryService categoryService, IDataSyncService dataSyncService)
            {
                _categoryService = categoryService;
                _dataSyncService = dataSyncService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _dataSyncService.ImmediateSync();
                await _categoryService.GetOrBuildCategoryTree();

                _dataSyncService.BackgroundSync();

                return new Unit();
            }
        }
    }
}
