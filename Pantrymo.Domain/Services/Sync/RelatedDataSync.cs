using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services.Sync
{
    public class RelatedDataSync<TChild, TParent> : DataTypeSync<TChild>
        where TChild : class, IWithId, IWithLastModifiedDate
        where TParent : class, IWithId, IWithLastModifiedDate   
    {
        public RelatedDataSync(IExceptionHandler exceptionHandler, IBaseDataContext dataContext) 
            : base(exceptionHandler, dataContext)
        {
        }

        protected override Task<Result<TChild[]>> GetUpdatedRecordsFromServer()
        {
            throw new NotImplementedException();
        }
    }
}
