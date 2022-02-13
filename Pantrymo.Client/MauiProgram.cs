using MediatR;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;
using Pantrymo.ClientInfrastructure;
using Pantrymo.ClientInfrastructure.Services;
using Pantrymo.Domain.Features;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using Pantrymo.SqlInfrastructure.Models;

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
            builder.Services.AddMediatR(typeof(CategoryTreeFeature), typeof(DataSyncFeature));
            builder.Services.AddDbContext<IDataContext, SqliteDbContext>();
            builder.Services.AddScoped<IBaseDataContext>(sp => sp.GetService<IDataContext>());
            builder.Services.AddScoped<IDataAccess, RemoteDataAccessWithLocalFallback>();
            builder.Services.AddScoped<RemoteDataAccess>();
            builder.Services.AddScoped<LocalDataAccess>();
            builder.Services.AddSingleton<IDataSyncService, PantrymoDataSyncService>();
            builder.Services.AddScoped<IFullHierarchyLoader, RemoteFullHierarchyLoader>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
            builder.Services.AddScoped<IngredientSuggestionService>();
            builder.Services.AddScoped<LocalRecipeSearchService>();
            builder.Services.AddScoped<IRecipeSearchService, RemoteRecipeSearchService>();
            builder.Services.AddScoped<IRecipeSearchProvider, InMemoryRecipeSearchProvider>();
            builder.Services.AddScoped<ISearchService<IComponent>, BasicComponentSearchService>();
            builder.Services.AddScoped<ISearchService<ICuisine>, BasicCuisineSearchService>();
            builder.Services.AddScoped<IExceptionHandler, NLogExceptionHandler>();
            builder.Services.AddScoped<IObjectMapper, ReflectionObjectMapper>();
            builder.Services.AddSingleton<NotificationDispatcher<DataSyncFeature.Notification>>();
            builder.Services.AddScoped<ILogger>(sp =>
            {        
                if(LogManager.Configuration == null)
                {
                    var settings = sp.GetService<ISettingsService>();
                    var logConfig = new LoggingConfiguration();
                    var fileTarget = new FileTarget { FileName = $"{settings.LocalDataFolder}\\log.txt" };
                    logConfig.AddRuleForOneLevel(LogLevel.Error, fileTarget);
                    LogManager.Configuration = logConfig;
                }

                var logger = LogManager.GetCurrentClassLogger();
                return logger;
            });

           
           
            
            builder.Services.AddScoped<HttpService>();
            builder.Services.AddSingleton(_ => new CustomJsonSerializer(typeof(IRecipe).Assembly, typeof(Recipe).Assembly));
            return builder.Build();
        }
    }
}