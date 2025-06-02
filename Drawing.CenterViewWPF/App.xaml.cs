using System;
using System.Security.Authentication.ExtendedProtection;
using System.Windows;
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
            
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MainWindow>();
        }
    }
}