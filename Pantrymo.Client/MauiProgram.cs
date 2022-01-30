using MediatR;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;
using Pantrymo.Application.Services;
using Pantrymo.ClientInfrastructure;

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

            builder.Services.AddBlazorWebView();
            builder.Services.AddMediatR(typeof(GetRecipeQuery));
            builder.Services.AddDbContext<IDataContext, PantryMoDBContext>();
            builder.Services.AddScoped<IDataAccess, RemoteDataAccessWithLocalFallback>();
            builder.Services.AddScoped<RemoteDataAccess>();
            builder.Services.AddScoped<LocalDataAccess>();
            builder.Services.AddScoped<DataSyncService>();
            builder.Services.AddSingleton<NetworkCheckService>();

            return builder.Build();
        }
    }
}