#nullable disable

namespace Pantrymo.Domain.Extensions
{
    public static class ExceptionExtensions
    {
        private const string NamespaceKeyword = "Pantrymo";

        public static IEnumerable<Exception> EnumerateAll(this Exception ex)           
        {
            if (ex is AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions.SelectMany(EnumerateAll))
                    yield return e;
            }
            else if(ex != null)
            {
                yield return ex;

                foreach(var inner in ex.InnerException.EnumerateAll())
                    yield return inner;
            }
        }

        public static string[] GetStackTrace(this Exception e)
            => e.StackTrace.Split(" at ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        public static string GetInternalFailingMethod(this Exception e)
        {
            return e.GetStackTrace().FirstOrDefault(p => p.Contains(NamespaceKeyword));
        }

        public static string GetFriendlyString(this Exception ex)
        {
            var allExceptions = ex.EnumerateAll().ToArray();

            var failingMethod = allExceptions
                .Select(GetInternalFailingMethod)
                .FirstOrDefault(p => p != null) ?? allExceptions.First().GetStackTrace().Last();


            var joinedMessage = string.Join("->",
                allExceptions
                    .Where(p => !p.Message.Contains("inner exception"))
                    .Select(p => p.Message)
                    .ToArray());

            return $"{joinedMessage} at {failingMethod}";
        }
    }
}
