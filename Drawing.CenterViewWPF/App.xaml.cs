using System;
using System.Security.Authentication.ExtendedProtection;
using System.Windows;
using Drawing.CenterViewWPF.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Drawing.CenterViewWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private static IServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();

            var viewModel = serviceProvider.GetService<MainWindowViewModel>();
            if (viewModel.ShowMainWindow())
            {
                var mainWindow = serviceProvider.GetService<MainWindow>();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("No active drawing");
                Application.Current.Shutdown();
            }
            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
        }
    }
}