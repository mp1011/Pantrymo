﻿using MediatR;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Pantrymo.Application.Features;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;
using Pantrymo.ClientInfrastructure;
using Pantrymo.ClientInfrastructure.Services;

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
            builder.Services.AddDbContext<IDataContext, PantryMoDBContext>();
            builder.Services.AddScoped<IDataAccess, RemoteDataAccessWithLocalFallback>();
            builder.Services.AddScoped<RemoteDataAccess>();
            builder.Services.AddScoped<LocalDataAccess>();
            builder.Services.AddScoped<DataSyncService>();
            builder.Services.AddSingleton<NetworkCheckService>();
            builder.Services.AddScoped<IFullHierarchyLoader, RemoteFullHierarchyLoader>();
            builder.Services.AddScoped<CategoryTreeBuilder>();
            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
            builder.Services.AddScoped<IngredientSuggestionService>();

            return builder.Build();
        }
    }
}