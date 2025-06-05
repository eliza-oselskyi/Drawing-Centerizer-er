using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.Strategies;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Core.Commands;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Core;

public class CenterOptionsDialogViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<bool> QuitRequested;
    public event EventHandler<bool> CloseRequested;
    private bool _hasSelectedDrawings;
    public bool HasSelectedDrawings
    {
        get => _hasSelectedDrawings;
        set
        {
            if (_hasSelectedDrawings == value) return;
            _hasSelectedDrawings = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasNoSelectedDrawings));
        }
    }
    
    public bool HasNoSelectedDrawings => !HasSelectedDrawings;
    
    public ICommand CenterAllCommand { get; set; }
    public ICommand CenterAbsolutelyAllCommand { get; set; }
    public ICommand CenterAllFabsCommand { get; set; }
    public ICommand CenterAllGaCommand { get; set; }
    public ICommand CenterFabCommand { get; set; }
    public ICommand CenterGaCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    
    public CenterOptionsDialogViewModel()
    {
        var selector = DrawingHandler.Instance.GetDrawingSelector();
        var selected = selector.GetSelected();

        HasSelectedDrawings = selected.GetSize() > 0 && selected.MoveNext();
        selected.Reset();
        
        CenterAbsolutelyAllCommand = new RelayCommand(ExecuteCenterAbsolutelyAll, _ => true);
        CenterAllCommand = new RelayCommand(ExecuteCenterAll, _ => true);
        CenterAllFabsCommand = new RelayCommand(ExecuteCenterAllFabs, _ => true);
        CenterAllGaCommand = new RelayCommand(ExecuteCenterAllGa, _ => true);
        CenterFabCommand = new RelayCommand(ExecuteCenterFab, _ => true);
        CenterGaCommand = new RelayCommand(ExecuteCenterGa, _ => true);
        CancelCommand = new RelayCommand(ExecuteCancel, _ => true);
    }

private async Task CenterDrawingsCore(Func<Tekla.Structures.Drawing.Drawing, bool> drawingFilter, IViewCenteringStrategy strategy, bool useSelectedOnly = false)
{
    CloseRequested?.Invoke(this, true);

    await Task.Run(() =>
    {
        var drawings = useSelectedOnly 
            ? DrawingHandler.Instance.GetDrawingSelector().GetSelected()
            : DrawingHandler.Instance.GetDrawings();
            
        var count = 1;
        while (drawings.MoveNext())
        {
            var curr = drawings.Current;
            if (!drawingFilter(curr)) continue;
            
            var drawing = new DrawingModel(curr);
            SendToTeklaDialog($"Centering drawing {count} of {drawings.GetSize()}...");
            count++;
            
            var centeringStrategy = strategy ?? GetAppropriateStrategy(curr);
            drawing.CenterDrawing(centeringStrategy);
        }
    });

    SendToTeklaDialog("Done.");
    QuitRequested?.Invoke(this, false);
}

private static IViewCenteringStrategy GetAppropriateStrategy(Tekla.Structures.Drawing.Drawing drawing) => drawing switch
{
    GADrawing => new GaViewCenteringStrategy(),
    AssemblyDrawing => new FabViewCenteringStrategy(),
    _ => throw new ArgumentException($"Unsupported drawing type: {drawing.GetType()}")
};

private async void ExecuteCenterAbsolutelyAll(object obj)
{
    await CenterDrawingsCore(IsAnyTargetDrawing, null, false);
}

private async void ExecuteCenterAll(object obj)
{
    await CenterDrawingsCore(IsAnyTargetDrawing, null, true);
}

private async void ExecuteCenterAllGa(object obj)
{
    await CenterDrawingsCore(IsGaDrawing, new GaViewCenteringStrategy(), false);
}

private async void ExecuteCenterAllFabs(object obj)
{
    await CenterDrawingsCore(IsFabDrawing, new FabViewCenteringStrategy(), false);
}

private async void ExecuteCenterGa(object obj)
{
    await CenterDrawingsCore(IsGaDrawing, new GaViewCenteringStrategy(), true);
}

private async void ExecuteCenterFab(object obj)
{
    await CenterDrawingsCore(IsFabDrawing, new FabViewCenteringStrategy(), true);
}

    private void ExecuteCancel(object obj)
    {
        SendToTeklaDialog("Aborted.");
        QuitRequested?.Invoke(this, false);
    }

    private void SendToTeklaDialog(string message)
    {
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt(message);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private static bool IsGaDrawing(Tekla.Structures.Drawing.Drawing drawing) => drawing is GADrawing;
    private static bool IsFabDrawing(Tekla.Structures.Drawing.Drawing drawing) => drawing is AssemblyDrawing;
    private static bool IsAnyTargetDrawing(Tekla.Structures.Drawing.Drawing drawing) => drawing is GADrawing or AssemblyDrawing;
}