using MediatR;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;
using Pantrymo.ClientInfrastructure;
using Pantrymo.ClientInfrastructure.Services;
using Pantrymo.Domain.Services;
using static Pantrymo.Application.Features.DataSyncFeature;

namespace Pantrymo.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .RegisterBlazorMauiWebView()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Host.ConfigureAppConfiguration((app, config) =>
            {
                var path = new FileInfo(typeof(MauiApp).Assembly.Location).Directory;
                config.AddJsonFile($@"{path}\appsettings.json", optional: false, false);
            });
            

            builder.Services.AddBlazorWebView();
            builder.Services.AddMediatR(typeof(CategoryTreeFeature));
            builder.Services.AddDbContext<IDataContext, SqliteDbContext>();
            builder.Services.AddScoped<IDataAccess, RemoteDataAccessWithLocalFallback>();
            builder.Services.AddScoped<RemoteDataAccess>();
            builder.Services.AddScoped<LocalDataAccess>();
            builder.Services.AddScoped<PantrymoDataSyncService>();
            builder.Services.AddSingleton<NetworkCheckService>();
            builder.Services.AddScoped<IFullHierarchyLoader, RemoteFullHierarchyLoader>();
            builder.Services.AddScoped<CategoryTreeBuilder>();
            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
            builder.Services.AddScoped<IngredientSuggestionService>();
            builder.Services.AddScoped<RecipeSearchService>();
            builder.Services.AddScoped<IRecipeSearchProvider, InMemoryRecipeSearchProvider>();
            builder.Services.AddScoped<ISearchService<IComponent>, BasicComponentSearchService>();
            builder.Services.AddScoped<ISearchService<ICuisine>, BasicCuisineSearchService>();
            builder.Services.AddScoped<IExceptionHandler, DebugLogExceptionHandler>();
            builder.Services.AddScoped<IObjectMapper, ReflectionObjectMapper>();
            builder.Services.AddSingleton<NotificationDispatcher<SyncStatusChanged>>();

            builder.Services.AddScoped<HttpService>();
            builder.Services.AddSingleton(_ => new CustomJsonSerializer(typeof(IRecipe).Assembly, typeof(Recipe).Assembly));
            return builder.Build();
        }
    }
}