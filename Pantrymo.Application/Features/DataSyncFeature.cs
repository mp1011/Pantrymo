using MediatR;
using Pantrymo.Application.Services;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using Pantrymo.Domain.Services.Sync;

namespace Pantrymo.Application.Features
{
    public class DataSyncFeature
    {
        public record Query() : IRequest<SyncTypeStatus[]> { }

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

        public class SyncStatusChangedHandler : INotificationHandler<SyncTypeStatus>
        {
            private readonly NotificationDispatcher<SyncTypeStatus> _dispatcher;

            public SyncStatusChangedHandler(NotificationDispatcher<SyncTypeStatus> dispatcher)
            {
                _dispatcher = dispatcher;
            }

            public async Task Handle(SyncTypeStatus notification, CancellationToken cancellationToken)
            {
                await _dispatcher.DispatchEvent(notification, cancellationToken);
            }
        }
    }
}
