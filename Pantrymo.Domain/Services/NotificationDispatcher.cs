using MediatR;
using System.Collections.Concurrent;

namespace Pantrymo.Domain.Services
{

    public abstract class BaseNotificationHandler<T> : INotificationHandler<T>
        where T: INotification
    {
        private readonly NotificationDispatcher<T> _dispatcher;

        public BaseNotificationHandler(NotificationDispatcher<T> dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchEvent(notification, cancellationToken);
        }
    }

    public class NotificationDispatcher<T>
        where T: INotification
    {
        private ConcurrentBag<INotificationHandler<T>> _handlers = new ConcurrentBag<INotificationHandler<T>>();

        public async Task DispatchEvent(T notification, CancellationToken cancellationToken)
        {
            var tasks = _handlers
                .Select(h => h.Handle(notification, cancellationToken))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        public void Register(INotificationHandler<T> handler)
        {
            _handlers.Add(handler);
        }

        public void Unregister(INotificationHandler<T> handler)
        {
            _handlers.TryTake(out handler);
        }
    }
}
