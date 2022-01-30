using MediatR;
using Pantrymo.Application.Models.AppModels;

namespace Pantrymo.Application.Queries
{
    public record GetCategoryTreeQuery() : IRequest<Category> { }
}
