using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Application.Services
{
    public class PantrymoDataSyncService : DataSyncService
    {
        private readonly RemoteDataAccess _dataAccess;
        private readonly IDataContext _dataContext;

        public PantrymoDataSyncService(RemoteDataAccess dataAccess, IDataContext dataContext) 
        {
            _dataAccess = dataAccess;
            _dataContext = dataContext;       
        }

        protected override DataSync[] SetupDataSync()
        {
            return new DataSync[]
            {
                new DataSync<Site>(_dataContext.Sites, dateFrom => _dataAccess.GetSites(dateFrom), records => _dataContext.InsertAsync(records)),
                new DataSync<Component>(_dataContext.Components, dateFrom => _dataAccess.GetComponents(dateFrom), records => _dataContext.InsertAsync(records)),
                new DataSync<AlternateComponentName>(_dataContext.AlternateComponentNames, dateFrom => _dataAccess.GetAlternateComponentName(dateFrom), records => _dataContext.InsertAsync(records))
            };
        }
      
        protected override  async Task CommitLocalChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
