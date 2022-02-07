using MediatR;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;
using Pantrymo.Domain.Services.Sync;

namespace Pantrymo.Application.Services
{
    public class PantrymoDataSyncService : DataSyncService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IDataContext _dataContext;
        private readonly IMediator _mediator;

        public PantrymoDataSyncService(IMediator mediator, IDataAccess dataAccess, IDataContext dataContext, IExceptionHandler exceptionHandler) 
            :base(exceptionHandler)
        {
            _mediator = mediator;
            _dataAccess = dataAccess;
            _dataContext = dataContext;       
        }

        protected override DataTypeSync[] SetupDataSync()
        {
            return new DataTypeSync[]
            {
                new DateBasedDataSync<ISite>(
                    _exceptionHandler,
                    _dataContext.Sites, 
                    dateFrom => _dataAccess.GetSites(dateFrom), 
                    records => _dataContext.Save(records)),

                new DateBasedDataSync<IComponent>(
                     _exceptionHandler,
                    _dataContext.Components, 
                    dateFrom => _dataAccess.GetComponents(dateFrom), 
                    records => _dataContext.Save(records)),

                new DateBasedDataSync<IAlternateComponentName>(
                     _exceptionHandler,
                    _dataContext.AlternateComponentNames, 
                    dateFrom => _dataAccess.GetAlternateComponentName(dateFrom),
                    records => _dataContext.Save(records)),

                new DateBasedDataSync<IAuthor>(
                    _exceptionHandler,
                    _dataContext.Authors,
                    dateFrom => _dataAccess.GetAuthors(dateFrom),
                    records => _dataContext.Save(records)),

                new DateBasedDataSync<IComponentNegativeRelation>(
                    _exceptionHandler,
                    _dataContext.ComponentNegativeRelations,
                    dateFrom => _dataAccess.GetComponentNegativeRelations(dateFrom),
                    records => _dataContext.Save(records)),

                new DateBasedDataSync<ICuisine>(
                    _exceptionHandler,
                    _dataContext.Cuisines,
                    dateFrom => _dataAccess.GetCuisines(dateFrom),
                    records => _dataContext.Save(records)),

            };
        }

        protected override void OnSyncStatusChanged(DataTypeSync dataTypeSync)
        {
            _mediator.Publish(new DataSyncFeature.SyncStatusChanged(dataTypeSync.ModelType, dataTypeSync.SyncStatus));
        }

        protected override  async Task CommitLocalChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
