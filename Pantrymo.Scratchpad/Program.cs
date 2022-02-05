#nullable disable

using Newtonsoft.Json;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;

//var ctx = new SqlServerDbContext(new FakeSettingsService(),new Microsoft.EntityFrameworkCore.DbContextOptions<SqlServerDbContext>());

partial class Program
{
    static void Main(string[] args)
    {
        ISite site = new Site { Name = "Hello" };

        var json = JsonConvert.SerializeObject(site);
        var json2 = System.Text.Json.JsonSerializer.Serialize(site);

        Console.Write(json);

    }

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