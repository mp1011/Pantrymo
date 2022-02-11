using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Services;
using System.Data;

namespace Pantrymo.ServerInfrastructure.Services
{
    public class DbRecipeSearchProvider : IRecipeSearchProvider
    {
        private readonly SqlServerDbContext _dbContext;

        public DbRecipeSearchProvider(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RecipeSearchResult[]> Search(IComponent[] components, ICuisine[] cuisines, RecipeSearchArgs searchArgs)
        {
            var dt = new DataTable();
            dt.Columns.Add("ComponentType", typeof(int));
            dt.Columns.Add("Exclude", typeof(bool));
            foreach (var c in components)
            {
                dt.Rows.Add(c.Id, false);
            }

            //foreach (var c in excludeComponents)
            //{
            //    dt.Rows.Add(c, true);
            //}

            var input = new SqlParameter();
            input.ParameterName = "@Input";
            input.SqlDbType = SqlDbType.Structured;
            input.TypeName = "ComponentInput";
            input.Value = dt;

            var dtCuisines = new DataTable();
            dtCuisines.Columns.Add("text", typeof(string));
            foreach (var c in cuisines)
                dtCuisines.Rows.Add(c.Name);

            var cuisinesParam = new SqlParameter();
            cuisinesParam.ParameterName = "@Cuisines";
            cuisinesParam.SqlDbType = SqlDbType.Structured;
            cuisinesParam.TypeName = "StringArray";
            cuisinesParam.Value = dtCuisines;

            var dtTraits = new DataTable();
            dtTraits.Columns.Add("text", typeof(string));
            //foreach (var c in filterTraits.Where(p => !p.IsNullOrEmpty()))
            //    dtTraits.Rows.Add(c);

            var traitsParam = new SqlParameter();
            traitsParam.ParameterName = "@Traits";
            traitsParam.SqlDbType = SqlDbType.Structured;
            traitsParam.TypeName = "StringArray";
            traitsParam.Value = dtTraits;

            var skip = new SqlParameter("@Skip", searchArgs.From);
            var take = new SqlParameter("@Take", searchArgs.To - searchArgs.From);

            var matchMin = new SqlParameter("@MatchMinimum", 1);

            var results = await _dbContext.RecipeSearchResults.FromSqlRaw("RecipeSearch @Input, @Cuisines, @Traits, @Skip, @Take, @MatchMinimum",
                input, cuisinesParam, traitsParam, skip, take, matchMin)
                    .Cast<RecipeSearchResult>()
                    .ToArrayAsync();

            return results;
        }
    }

}
