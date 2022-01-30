using MediatR;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Queries
{
    public record GetRecipeQuery(int Id) : IRequest<Recipe> { }
}
