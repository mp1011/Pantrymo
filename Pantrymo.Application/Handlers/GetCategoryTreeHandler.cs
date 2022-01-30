using MediatR;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Queries;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Handlers
{
    public class GetCategoryTreeHandler : IRequestHandler<GetCategoryTreeQuery, Category>
    {
        private readonly CategoryTreeBuilder _categoryTreeBuilder;

        public GetCategoryTreeHandler(CategoryTreeBuilder categoryTreeLoader)
        {
            _categoryTreeBuilder = categoryTreeLoader;
        }

        public async Task<Category> Handle(GetCategoryTreeQuery request, CancellationToken cancellationToken)
        {
            return await _categoryTreeBuilder.BuildCategoryTree();
        }
    }
}
