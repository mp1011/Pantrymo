using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Application.Services
{
    public class PantrymoDataSyncService : DataSyncService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IDataContext _dataContext;

        public PantrymoDataSyncService(IDataAccess dataAccess, IDataContext dataContext, IExceptionHandler exceptionHandler) 
            :base(exceptionHandler)
        {
            _dataAccess = dataAccess;
            _dataContext = dataContext;       
        }

        protected override DataSync[] SetupDataSync()
        {
            return new DataSync[]
            {
                new DataSync<ISite>(_dataContext.Sites, dateFrom => _dataAccess.GetSites(dateFrom), records => _dataContext.Save(records)),
                new DataSync<IComponent>(_dataContext.Components, dateFrom => _dataAccess.GetComponents(dateFrom), records => _dataContext.Save(records)),
                new DataSync<IAlternateComponentName>(_dataContext.AlternateComponentNames, dateFrom => _dataAccess.GetAlternateComponentName(dateFrom), records => _dataContext.Save(records))
            };
        }
      
        protected override  async Task CommitLocalChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
