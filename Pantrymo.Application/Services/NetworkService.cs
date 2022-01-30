using System.Net.NetworkInformation;

namespace Pantrymo.Application.Services
{
    public class NetworkCheckService
    {
        public bool HasInternet() => NetworkInterface.GetIsNetworkAvailable();
    }
}
