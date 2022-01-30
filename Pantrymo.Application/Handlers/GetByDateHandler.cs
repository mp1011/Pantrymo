using MediatR;
using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Handlers
{
    public class GetSitesByDateHandler : IRequestHandler<GetByDateQuery<Site>, Site[]>
    {
        private readonly ISiteAPI _siteAPI;

        public GetSitesByDateHandler(ISiteAPI siteAPI)
        {
            _siteAPI = siteAPI;
        }

        public async Task<Site[]> Handle(GetByDateQuery<Site> request, CancellationToken cancellationToken)
            => await _siteAPI.GetSites(request.DateFrom).NullToEmpty(); 
    }
}
