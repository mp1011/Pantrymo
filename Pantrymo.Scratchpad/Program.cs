using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;

Console.WriteLine("SERVER");
Test(InitServer());

Console.WriteLine();
Console.WriteLine("CLIENT");
Test(InitClient());
Console.ReadKey();

async void Test(ServiceProvider serviceProvider)
{
    var mediator = serviceProvider.GetRequiredService<IMediator>();  
    var query = await mediator.Send(new DummyQuery());

    foreach (var result in GetTopDates(query))
    {
        Console.WriteLine(result.LastModified);
    }
}

IWithLastModifiedDate[] GetTopDates(IQueryable<IWithLastModifiedDate> query)
{
    query = query
            .Where(p=>p.LastModified >= new DateTime(2022,1,1))
            .OrderByDescending(p => p.LastModified)
            .Take(5);

    Console.WriteLine(query.ToQueryString());

    return query.ToArray();
}


ServiceProvider InitServer()
{
    return new ServiceCollection()
        .AddMediatR(typeof(GetRecipeQuery))
        .AddDbContext<IDataContext, Pantrymo.ServerInfrastructure.PantryMoDBContext>()
        .BuildServiceProvider();
}

ServiceProvider InitClient()
{
    return new ServiceCollection()
        .AddMediatR(typeof(GetRecipeQuery))
        .AddDbContext<IDataContext, Pantrymo.ClientInfrastructure.PantryMoDBContext>()
        .BuildServiceProvider();
}