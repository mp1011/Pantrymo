#nullable disable

using Pantrymo.Domain.Services;

//var ctx = new SqlServerDbContext(new FakeSettingsService(),new Microsoft.EntityFrameworkCore.DbContextOptions<SqlServerDbContext>());

partial class Program
{
    static void Main(string[] args)
    {
       HelloFrom("Generated Code");
    }

    static void HelloFrom(string name) { }
}




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