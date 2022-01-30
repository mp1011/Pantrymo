using Pantrymo.Application.Services;

namespace Pantrymo.Client
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App(DataSyncService dataSyncService)
        {
            InitializeComponent();
            dataSyncService.BackgroundSync();

            MainPage = new MainPage();
        }
    }
}