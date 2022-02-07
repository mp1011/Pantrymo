namespace Pantrymo.Domain.Models
{
    public class Result<T>
    {
        public T? Data { get; }
        public bool Success { get; }

        public bool Failure => !Success;

        public Exception? Error { get; }

        internal Result(T? data)
        {
            Data = data;
            Success = true;
        }

        internal Result(Exception e, T? data=default)
        {
            Data = data;
            Success = false;
            Error = e;
        }
    }

    public static class Result
    {
        public static Result<T> Success<T>(T payload) => new Result<T>(payload);

        public static Result<T?> Failure<T>(Exception e, T? payload=default) => new Result<T?>(e, payload);
    }
}
