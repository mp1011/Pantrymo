using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;
using Pantrymo.Application.Services;
using Pantrymo.ServerInfrastructure;
using Pantrymo.ServerInfrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddMediatR(typeof(GetCategoryTreeQuery));
builder.Services.AddDbContext<IDataContext, PantryMoDBContext>();
builder.Services.AddScoped<IDataAccess, LocalDataAccess>();
builder.Services.AddScoped<CategoryTreeBuilder>();
builder.Services.AddScoped<IFullHierarchyLoader, FullHierarchyLoader>();
builder.Services.AddScoped<SettingsService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
