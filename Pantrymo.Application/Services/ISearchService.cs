using Pantrymo.Application.Models;
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Services
{
    public interface ISearchService<T>
        where T:IWithName
    {
        Task<T[]> Search(params string[] text);
    }

    public abstract class BasicSearchService<T> : ISearchService<T>
        where T:IWithName
    {
        protected abstract IQueryable<T> Query { get; }
 
        public Task<T[]> Search(params string[] text)
        {
            text = text.Select(t => t.ToLower()).ToArray();

            var result = Query
                .Where(p => text.Contains(p.Name.ToLower()))
                .ToArray();

            return Task.FromResult(result);
        }
    }

    public class BasicComponentSearchService : BasicSearchService<IComponent>
    {
        public BasicComponentSearchService(IDataContext dataContext)
        {
            Query = dataContext.Components;
        }

        protected override IQueryable<IComponent> Query { get; }
    }

    public class BasicCuisineSearchService : BasicSearchService<ICuisine>
    {
        public BasicCuisineSearchService(IDataContext dataContext)
        {
            Query = dataContext.Cuisines;
        }

        protected override IQueryable<ICuisine> Query { get; }
    }
}
