using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using System.Diagnostics;

namespace Pantrymo.Domain.Extensions
{
    public static class TaskExtensions
    {
        public static Task<Result<bool>> AsResult(this Task task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    return Result.Failure(t.Exception ?? new Exception("unknown error"), false);
                else
                    return Result.Success(true);
            });
        }

        public static Task<Result<T?>> AsResult<T>(this Task<T?> task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    return Result.Failure<T>(t.Exception ?? new Exception("unknown error"));
                else 
                    return Result.Success(t.Result);
            });
        }

        public static Task<Result<T[]>> AsResult<T>(this Task<T[]> task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    return Result.Failure(t.Exception, t.Result ?? new T[] { });
                else
                    return Result.Success(t.Result);
            });
        }

        public static Task<T[]> NullToEmpty<T>(this Task<T[]?> task)
        {
            return task.ContinueWith(t =>
            {
                return t.Result ?? new T[] { };
            });
        }

        public static Task HandleError(this Task task, IExceptionHandler exceptionHandler)
            => exceptionHandler.HandleFault(task);

        public static Task<T?> HandleError<T>(this Task<T?> task, IExceptionHandler exceptionHandler)
            => exceptionHandler.HandleFault(task);

        public static Task HandleError(this Task task, Action<Exception> onError)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    onError(t.Exception ?? new Exception("unknown error"));
            });
        }

        public static Task<T> HandleError<T>(this Task<T> task, Func<Exception,T> onError)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    return onError(t.Exception);
                else 
                    return t.Result;
            });
        }


        public static Task<T?> DefaultIfFaulted<T>(this Task<T?> task)
        {
            return task.HandleError(ex => default);
        }

        public static Task<bool> CheckSuccess(this Task task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.WriteLine(t.Exception.GetFriendlyString());
                    return false;
                }

                return true;
            });
        }

        public static Task<bool> CheckSuccess(this Task task, IExceptionHandler exceptionHandler)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    exceptionHandler.Handle(t.Exception);
                    return false;
                }

                return true;
            });
        }


        public static Task<T?> DebugLogError<T>(this Task<T?> task)
        {
            return task.HandleError(ex =>
            {
                Debug.WriteLine(ex.GetFriendlyString());
                return default;
            });
        }

        public static Task DebugLogError(this Task task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    var color = Console.ForegroundColor;

                    Debug.WriteLine(t.Exception.GetFriendlyString());
                }
            });
        }

    }
}
