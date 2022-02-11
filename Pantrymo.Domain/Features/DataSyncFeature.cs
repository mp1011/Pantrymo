using MediatR;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Domain.Features
{
    public class DataSyncFeature
    {
        public record Query() : IRequest<SyncTypeStatus[]> { }

        public record Notification(SyncTypeStatus TypeStatus) : INotification  { }

        public class DataSyncStatusQueryHandler : IRequestHandler<Query, SyncTypeStatus[]>
        {
            private readonly IDataSyncService _dataSyncService;

            public DataSyncStatusQueryHandler(IDataSyncService dataSyncService)
            {
                _dataSyncService = dataSyncService;
            }

            public Task<SyncTypeStatus[]> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_dataSyncService.GetCurrentSyncStatus());
            }
        }

        public class SyncStatusChangedHandler : INotificationHandler<Notification>
        {
            private readonly NotificationDispatcher<Notification> _dispatcher;

            public SyncStatusChangedHandler(NotificationDispatcher<Notification> dispatcher)
            {
                _dispatcher = dispatcher;
            }

            public async Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                await _dispatcher.DispatchEvent(notification, cancellationToken);
            }
        }
    }
}
