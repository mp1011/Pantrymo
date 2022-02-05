using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;

namespace Pantrymo.ServerInfrastructure.Services
{
    public class FullHierarchyLoader : IFullHierarchyLoader
    {
        private readonly SqlServerDbContext _dbContext;

        public FullHierarchyLoader(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FullHierarchy[]> GetFullHierarchy()
        {
            return await _dbContext.FullHierarchy
                .FromSqlRaw("exec GetFullHierarchy")
                .Cast<FullHierarchy>()
                .ToArrayAsync();
        }
    }
}
