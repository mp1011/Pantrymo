using MediatR;
using Newtonsoft.Json;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;
using Pantrymo.Domain.Features;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using Pantrymo.ServerInfrastructure;
using Pantrymo.ServerInfrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddControllers()
	.AddNewtonsoftJson(o =>
	{
		o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		o.SerializerSettings.ContractResolver = new CustomContractResolver(typeof(IRecipe).Assembly);
	});

builder.Services.AddMediatR(typeof(CategoryTreeFeature), typeof(DataSyncFeature));
builder.Services.AddDbContext<IDataContext, SqlServerDbContext>();
builder.Services.AddScoped<IBaseDataContext>(sp => sp.GetService<IDataContext>());
builder.Services.AddScoped<IDataAccess, LocalDataAccess>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IFullHierarchyLoader, FullHierarchyLoader>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<ICacheService, NoCacheService>();
builder.Services.AddScoped<IngredientSuggestionService>();
builder.Services.AddScoped<IRecipeSearchService, LocalRecipeSearchService>();
builder.Services.AddScoped<IRecipeSearchProvider, DbRecipeSearchProvider>();
builder.Services.AddScoped<ISearchService<IComponent>, BasicComponentSearchService>();
builder.Services.AddScoped<ISearchService<ICuisine>, BasicCuisineSearchService>();
builder.Services.AddScoped<IExceptionHandler, DebugLogExceptionHandler>();
builder.Services.AddScoped<IObjectMapper, ReflectionObjectMapper>();
builder.Services.AddScoped<NotificationDispatcher<DataSyncFeature.Notification>>();
builder.Services.AddScoped<NotificationDispatcher<ShowProgressFeature.Notification>>();

builder.Services.AddSingleton<IDataSyncService, EmptyDataSyncService>();

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
