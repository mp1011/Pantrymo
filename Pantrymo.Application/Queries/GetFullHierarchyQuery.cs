using MediatR;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Queries
{
    public record GetFullHierarchyQuery() : IRequest<FullHierarchy[]> { }
}
