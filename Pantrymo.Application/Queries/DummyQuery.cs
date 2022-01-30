using MediatR;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Queries
{
    public record DummyQuery() : IRequest<IQueryable<Site>> { }

}
