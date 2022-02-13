using MediatR;
using Pantrymo.Domain.Services;

namespace Pantrymo.Domain.Features
{
    public class ShowProgressFeature
    {
        public record Notification(string Message) : INotification { }

        public class ProgressChangedHandler : INotificationHandler<Notification>
        {
            private readonly NotificationDispatcher<Notification> _dispatcher;

            public ProgressChangedHandler(NotificationDispatcher<Notification> dispatcher)
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
