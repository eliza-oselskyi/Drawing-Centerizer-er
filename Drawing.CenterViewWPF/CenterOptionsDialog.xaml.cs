using System.Windows;
using Drawing.CenterViewWPF.Core;

namespace Drawing.CenterViewWPF;

public partial class CenterOptionsDialog : Window
{
    private readonly CenterOptionsDialogViewModel _viewModel;
    public CenterOptionsDialog()
    {
        InitializeComponent();
        _viewModel = new CenterOptionsDialogViewModel();
        DataContext = _viewModel;

        _viewModel.CloseRequested += (sender, args) =>
        {
            Dispatcher.Invoke(Hide);
        };

        _viewModel.QuitRequested += (sender, args) =>
        {
            Dispatcher.Invoke(() => Application.Current.Shutdown());
        };
    }
}