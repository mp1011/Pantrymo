using MediatR;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Features
{
    public class CategoryTreeFeature
    {
        public record Query() : IRequest<Category> { }

        public class Handler : IRequestHandler<Query, Category>
        {
            private readonly CategoryService _categoryTreeBuilder;

            public Handler(CategoryService categoryTreeLoader)
            {
                _categoryTreeBuilder = categoryTreeLoader;
            }

            public async Task<Category> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _categoryTreeBuilder.GetOrBuildCategoryTree();
            }
        }
    }
}
