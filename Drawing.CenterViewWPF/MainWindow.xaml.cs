using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Drawing.CenterViewWPF;
using Drawing.CenterViewWPF.Core;

namespace Drawing.CenterViewWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public bool IsConnected { get; set; }
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            IsConnected = false;
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
            _viewModel.QuitRequested += (sender, _) =>
            {
             Dispatcher.Invoke(() => Application.Current.Shutdown());
            };
        }

        private void ThemeToggle_CheckChanged(object sender, RoutedEventArgs e)
        {
            var resources = Application.Current.Resources;

            if (ThemeToggle.IsChecked == true)
            {
                // Switch to dark mode
                resources["BackgroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"));
                resources["ForegroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                resources["SecondaryBackgroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));
                resources["BorderBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#404040"));
                resources["DisabledForegroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"));
                //resources["MouseOverBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff616e"));
            }
            else
            {
                // Switch to light mode
                resources["BackgroundBrush"] = new SolidColorBrush(Colors.White);
                resources["ForegroundBrush"] = new SolidColorBrush(Colors.Black);
                resources["SecondaryBackgroundBrush"] = new SolidColorBrush(Color.FromRgb(230,236,245));
                resources["BorderBrush"] = new SolidColorBrush(Color.FromRgb(204,204,204));
                resources["DisabledForegroundBrush"] = new SolidColorBrush(Color.FromRgb(102,102,102));
                resources["MouseOverBrush"] = new SolidColorBrush(Color.FromRgb(50, 216, 245));
            }
        }
    }
}