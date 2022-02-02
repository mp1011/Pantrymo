using System.Net.NetworkInformation;

namespace Pantrymo.Domain.Services
{
    public class NetworkCheckService
    {
        public bool HasInternet() => NetworkInterface.GetIsNetworkAvailable();
    }
}
