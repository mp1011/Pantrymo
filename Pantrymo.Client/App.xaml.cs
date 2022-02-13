using Pantrymo.Application.Services;
using Pantrymo.Domain.Services;

namespace Pantrymo.Client
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }
    }
}