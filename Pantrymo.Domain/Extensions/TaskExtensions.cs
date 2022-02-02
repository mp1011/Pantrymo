﻿using Pantrymo.Domain.Models;
using System.Diagnostics;

namespace Pantrymo.Domain.Extensions
{
    public static class TaskExtensions
    {

        public static Task<Result<T>> AsResult<T>(this Task<T> task)
            where T:class,new()
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    return Result.Failure(t.Result ?? new T());
                else 
                    return Result.Success(t.Result);
            });
        }

        public static Task<Result<T[]>> AsResult<T>(this Task<T[]> task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    return Result.Failure(t.Result ?? new T[] { });
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


        public static Task<T?> DebugLogError<T>(this Task<T?> task)
        {
            return task.HandleError(ex =>
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
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

                    Debug.WriteLine(t.Exception?.Message);
                    Debug.WriteLine(t.Exception?.StackTrace);
                }
            });
        }

    }
}