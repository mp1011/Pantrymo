using MediatR;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Queries
{
    public record GetByDateQuery<T>(DateTime DateFrom) : IRequest<T[]>
        where T : IWithLastModifiedDate { }

}
