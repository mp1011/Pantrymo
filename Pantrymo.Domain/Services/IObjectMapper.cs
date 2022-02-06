namespace Pantrymo.Domain.Services
{
    public interface IObjectMapper
    {
        void CopyAllProperties<T>(T source, T destination);
    }

    public class ReflectionObjectMapper : IObjectMapper
    {
        private readonly IExceptionHandler _exceptionHandler;

        public ReflectionObjectMapper(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public void CopyAllProperties<T>(T source, T destination)
        {
            foreach(var property in typeof(T).GetProperties())
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                try
                {
                    var value = property.GetValue(source);
                    property.SetValue(source, value);
                }
                catch(Exception e)
                {
                    _exceptionHandler.Handle(e);
                }
            }
        }
    }
}
