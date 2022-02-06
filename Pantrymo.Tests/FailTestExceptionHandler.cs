using NUnit.Framework;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Pantrymo.Tests
{
    public class FailTestExceptionHandler : IExceptionHandler
    {
        public void Handle(Exception exception)
        {
            Assert.Fail(exception.GetFriendlyString());
        }

        public Task HandleFault(Task task)
        {
            return task.HandleError(e => Handle(e));
        }

        public Task<T?> HandleFault<T>(Task<T?> task)
        {
            return task.HandleError(e =>
            {
                Handle(e);
                return default(T);
            });
        }
    }
}
