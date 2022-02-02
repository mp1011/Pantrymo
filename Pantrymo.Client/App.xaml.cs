using Pantrymo.Application.Services;

namespace Pantrymo.Client
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App(PantrymoDataSyncService dataSyncService)
        {
            InitializeComponent();
            dataSyncService.BackgroundSync();

            MainPage = new MainPage();
        }
    }
}