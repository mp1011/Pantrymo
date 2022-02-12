using Pantrymo.Application.Models;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using static Pantrymo.Domain.Features.DataSyncFeature;

namespace Pantrymo.Application.Features
{
    public class PantrymoDataSyncFeature
    {
        public class InsertDownloadedRecipes : InsertDownloadedRecords<IRecipeDTO>
        {
            public InsertDownloadedRecipes(IBaseDataContext baseDataContext, IExceptionHandler exceptionHandler) 
                : base(baseDataContext, exceptionHandler)
            {
            }
        }
    }
}
