using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Features;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using Pantrymo.Domain.Services.Sync;

namespace Pantrymo.Application.Services
{
    public class PantrymoDataSyncService : BaseDataSyncService
    {
        public PantrymoDataSyncService(IMediator mediator, IDataAccess dataAccess, IDataContext dataContext, IExceptionHandler exceptionHandler) 
            :base(exceptionHandler,dataAccess,dataContext,mediator)
        { 
        }

        protected override DataTypeSync[] SetupDataSync()
        {
            return new DataTypeSync[]
            {
                new DateBasedDataSync<ISite>(_exceptionHandler, _dataContext, _dataAccess),
                new DateBasedDataSync<IComponent>(_exceptionHandler, _dataContext, _dataAccess),
                new DateBasedDataSync<IAlternateComponentName>(_exceptionHandler, _dataContext, _dataAccess),
                new DateBasedDataSync<IAuthor>(_exceptionHandler, _dataContext, _dataAccess),
                new DateBasedDataSync<IComponentNegativeRelation>(_exceptionHandler, _dataContext, _dataAccess),
                new DateBasedDataSync<ICuisine>(_exceptionHandler, _dataContext, _dataAccess),   
                new UpdateExistingOnlyDataSync<IRecipeDTO>(_exceptionHandler, _dataContext, _dataAccess),
            };
        }

        protected override void OnSyncStatusChanged(DataTypeSync dataTypeSync)
        {
            _mediator.Publish(new DataSyncFeature.Notification(
                new SyncTypeStatus(dataTypeSync.ModelType, dataTypeSync.SyncStatus)));
        }

        protected override  async Task CommitLocalChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
