using MediatR;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Domain.Features
{
    public class DataSyncFeature
    {
        public record Query() : IRequest<SyncTypeStatus[]> { }

        public record Notification(SyncTypeStatus TypeStatus) : INotification { }

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

        public abstract class DataDownloadedHandler<T> : INotificationHandler<DataDownloadedNotification<T>>
        {
            public async Task Handle(DataDownloadedNotification<T> notification, CancellationToken cancellationToken)
            {
                await OnDataDownloaded(notification.Models);
            }

            protected abstract Task OnDataDownloaded(T[] records);
        }

        public abstract class InsertDownloadedRecords<T> : DataDownloadedHandler<T>
            where T:IWithId,IWithLastModifiedDate
        {
            private readonly IBaseDataContext _baseDataContext;
            private readonly IExceptionHandler _exceptionHandler;

            protected InsertDownloadedRecords(IBaseDataContext baseDataContext,
                IExceptionHandler exceptionHandler)
            {
                _baseDataContext = baseDataContext;
                _exceptionHandler = exceptionHandler;
            }

            protected override async Task OnDataDownloaded(T[] records)
            {
                var success = await _baseDataContext
                    .Save(records)
                    .CheckSuccess(_exceptionHandler);
                
                if(success)
                    await _baseDataContext.SaveChangesAsync();
            }
        }
    }
}
