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
}
