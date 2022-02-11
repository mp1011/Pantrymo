using MediatR;

namespace Pantrymo.Domain.Models
{
    public record DataDownloadedNotification<T>(T[] Models) : INotification { }
}
