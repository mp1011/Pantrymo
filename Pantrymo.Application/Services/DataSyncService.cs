using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Services
{
    public class DataSyncService
    {
        private readonly ISiteAPI _siteAPI;
        private readonly IDataContext _dataContext;

        public DataSyncService(ISiteAPI siteAPI, IDataContext dataContext)
        {
            _siteAPI = siteAPI;
            _dataContext = dataContext;
        }

        public void BackgroundSync()
        {
            Task.Run(() => StartBackgroundSync());
        }

        private async Task StartBackgroundSync()
        {
            while(true)
            {
                var synced = (await TrySync().DebugLogError());
                if (synced)
                    await Task.Delay(TimeSpan.FromMinutes(15)); 
                else
                    await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }

        private async Task<bool> TrySync()
        {
            var localLastModified = _dataContext.Sites.GetLatestModifiedDate();
            var newFromServer = await _siteAPI.GetSites(localLastModified);
            if (newFromServer == null)
                return false;

            if (newFromServer.Any())
            {
                await _dataContext.InsertAsync(newFromServer);
                await _dataContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
