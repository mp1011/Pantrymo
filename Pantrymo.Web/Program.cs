using MediatR;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;
using Pantrymo.Domain.Services;
using Pantrymo.ServerInfrastructure;
using Pantrymo.ServerInfrastructure.Services;
using static Pantrymo.Application.Features.DataSyncFeature;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
	.AddControllers()
	.AddNewtonsoftJson();

builder.Services.AddMediatR(typeof(CategoryTreeFeature));
builder.Services.AddDbContext<IDataContext, SqlServerDbContext>();
builder.Services.AddScoped<IDataAccess, LocalDataAccess>();
builder.Services.AddScoped<CategoryTreeBuilder>();
builder.Services.AddScoped<IFullHierarchyLoader, FullHierarchyLoader>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<ICacheService, NoCacheService>();
builder.Services.AddScoped<IngredientSuggestionService>();
builder.Services.AddScoped<RecipeSearchService>();
builder.Services.AddScoped<IRecipeSearchProvider, DbRecipeSearchProvider>();
builder.Services.AddScoped<ISearchService<IComponent>, BasicComponentSearchService>();
builder.Services.AddScoped<ISearchService<ICuisine>, BasicCuisineSearchService>();
builder.Services.AddScoped<IExceptionHandler, DebugLogExceptionHandler>();
builder.Services.AddScoped<IObjectMapper, ReflectionObjectMapper>();
builder.Services.AddScoped<NotificationDispatcher<SyncStatusChanged>>();

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
