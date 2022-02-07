using MediatR;
using Pantrymo.Domain.Services.Sync;
using System.Diagnostics;

namespace Pantrymo.Application.Features
{
    public class DataSyncFeature
    {
        public record SyncStatusChanged(Type ModelType, SyncStatus Status) : INotification { }

        public class DebugLogReciever : INotificationHandler<SyncStatusChanged>
        {
            public Task Handle(SyncStatusChanged notification, CancellationToken cancellationToken)
            {
                if (notification.Status.Succeeded)
                    Debug.WriteLine($"Sync for {notification.ModelType} succeeded, sync count = {notification.Status.RecordSyncCount}");
                else
                    Debug.WriteLine($"Sync for {notification.ModelType} failed");

                return Task.CompletedTask;
            }
        }
    }
}
