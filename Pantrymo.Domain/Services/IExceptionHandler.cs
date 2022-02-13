#nullable disable
using NLog;
using Pantrymo.Domain.Extensions;
using System.Diagnostics;

namespace Pantrymo.Domain.Services
{
    public interface IExceptionHandler
    {
        void Handle(Exception exception);

        Task HandleFault(Task task);

        Task<T?> HandleFault<T>(Task<T?> task);
    }

    public class DebugLogExceptionHandler : IExceptionHandler
    {
        public void Handle(Exception exception)
        {
            Debug.WriteLine(exception.GetFriendlyString());
        }

        public Task HandleFault(Task task)
        {
            return task.DebugLogError();
        }

        public Task<T?> HandleFault<T>(Task<T?> task)
        {
            return task.DebugLogError();
        }
    }


    public class NLogExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;

        public NLogExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void Handle(Exception exception)
        {
            _logger.Error(exception.GetFriendlyString());
        }

        public Task HandleFault(Task task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    Handle(t.Exception);
            });
        }

        public Task<T?> HandleFault<T>(Task<T?> task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Handle(t.Exception);
                    return default;
                }
                else
                    return t.Result;
            });
        }
    }

}
