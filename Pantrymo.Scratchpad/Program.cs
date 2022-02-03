using Pantrymo.Application.Services;
using Pantrymo.Domain.Services;
using Pantrymo.ServerInfrastructure;

var ctx = new PantryMoDBContext(new FakeSettingsService(),new Microsoft.EntityFrameworkCore.DbContextOptions<PantryMoDBContext>());

var x = ctx.Recipes.FirstOrDefault(p => p.Id == 6361);

Console.WriteLine(x.Title);

class FakeSettingsService : ISettingsService
{
    public string ConnectionString => "Server=localhost;Database=PantryMoDB;Trusted_Connection=True;";

    public string Host => throw new NotImplementedException();

    public string LocalDataFolder => throw new NotImplementedException();

    public TimeSpan GetCacheDuration<T>()
    {
        throw new NotImplementedException();
    }
}