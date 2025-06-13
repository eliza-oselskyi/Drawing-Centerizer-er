using System;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
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
        private static Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            const string appName = "Drawing.CenterViewWPF";
            _mutex = new Mutex(true, appName, out var createdNew);

            if (!createdNew)
            {
                // Another instance is already running
                MessageBox.Show("Application is already running.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Current.Shutdown();
                return;
            }

            try {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                serviceProvider = serviceCollection.BuildServiceProvider();

                var viewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
                if (viewModel.ShowMainWindow())
                {
                    var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                }
                else
                {
                    var optionsDialog = serviceProvider.GetRequiredService<CenterOptionsDialog>();
                    optionsDialog.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
            }
            base.OnExit(e);
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            
            serviceCollection.AddSingleton<CenterOptionsDialog>();
            serviceCollection.AddSingleton<CenterOptionsDialogViewModel>();
        }
    }
}