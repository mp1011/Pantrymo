using MediatR;
using Pantrymo.Domain.Services;
using Pantrymo.Domain.Services.Sync;

namespace Pantrymo.Application.Features
{
    public class DataSyncFeature
    {
        public record SyncStatusChanged(Type ModelType, SyncStatus Status) : INotification { }

        public class SyncStatusChangedHandler : BaseNotificationHandler<SyncStatusChanged>
        {
            public SyncStatusChangedHandler(NotificationDispatcher<SyncStatusChanged> dispatcher) : base(dispatcher) { }
        }
    }
}
