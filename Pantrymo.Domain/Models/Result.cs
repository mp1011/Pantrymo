namespace Pantrymo.Domain.Models
{
    public class Result<T>
    {
        public T Data { get; }
        public bool Success { get; }

        public bool Failure => !Success;

        internal Result(T data, bool success)
        {
            Data = data;
            Success = success;
        }
    }

    public static class Result
    {
        public static Result<T> Success<T>(T payload) => new Result<T>(payload, true);

        public static Result<T> Failure<T>(T payload) => new Result<T>(payload, false);
    }
}
