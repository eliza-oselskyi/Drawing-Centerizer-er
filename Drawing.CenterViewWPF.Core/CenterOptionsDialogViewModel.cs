using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
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

    private async void ExecuteCenterAbsolutelyAll(object obj)
    {
        CloseRequested?.Invoke(this, true);
        await Task.Run(() =>
        {
            var drawings = DrawingHandler.Instance.GetDrawings();
            var count = 1;
            while (drawings.MoveNext())
            {
                var curr = drawings.Current;
                var drawing = new DrawingModel(curr);
                SendToTeklaDialog($"Centering drawing {count} of {drawings.GetSize()}...");
                count++;
                switch (curr)
                {
                    case GADrawing:
                        drawing.CenterDrawing(new GaViewCenteringStrategy());
                        break;
                    case AssemblyDrawing:
                        drawing.CenterDrawing(new FabViewCenteringStrategy());
                        break;
                }
            }
        });
        
        SendToTeklaDialog($"Done.");
        
        QuitRequested?.Invoke(this, false);
    }
    

    private async void ExecuteCenterAllGa(object obj)
    {
        CloseRequested?.Invoke(this, true);
        await Task.Run(() =>{
            var drawings = DrawingHandler.Instance.GetDrawings();
            var count = 1;
            while (drawings.MoveNext())     
            {
                var curr = drawings.Current;
                if (curr is not GADrawing) continue;
                var drawing = new DrawingModel(curr);
                SendToTeklaDialog($"Centering drawing {count} of {drawings.GetSize()}...");
                count++;
                drawing.CenterDrawing(new GaViewCenteringStrategy());
            }
        });
        
        SendToTeklaDialog($"Done.");
        QuitRequested?.Invoke(this, false);
    }

    private async void ExecuteCenterAllFabs(object obj)
    {
        CloseRequested?.Invoke(this, true);

        await Task.Run(() =>
        {
            var drawings = DrawingHandler.Instance.GetDrawings();
            var count = 1;
            while (drawings.MoveNext())
            {
                var curr = drawings.Current;
                if (curr is not AssemblyDrawing) continue;
                var drawing = new DrawingModel(curr);
                SendToTeklaDialog($"Centering drawing {count} of {drawings.GetSize()}...");
                count++;
                drawing.CenterDrawing(new FabViewCenteringStrategy());
            }
        });
        
        SendToTeklaDialog($"Done.");
        QuitRequested?.Invoke(this, false);
    }


    private void ExecuteCancel(object obj)
    {
        SendToTeklaDialog($"Aborted.");
        QuitRequested?.Invoke(this, false);
    }

    private async void ExecuteCenterGa(object obj)
    {
        CloseRequested?.Invoke(this, true);

        await Task.Run(() =>
        {
            var selector = DrawingHandler.Instance.GetDrawingSelector();
            var selected = selector.GetSelected();
            var count = 1;

            while (selected.MoveNext())
            {
                var curr = selected.Current;
                if (curr is not GADrawing) continue;
                var drawing = new DrawingModel(curr);
                SendToTeklaDialog($"Centering drawing {count} of {selected.GetSize()}...");
                count++;
                drawing.CenterDrawing(new GaViewCenteringStrategy());
            }
        });
        
        SendToTeklaDialog($"Done.");
        QuitRequested?.Invoke(this, false);
    }

    private async void ExecuteCenterFab(object obj)
    {
        CloseRequested?.Invoke(this, true);

        await Task.Run(() =>
        {
            var selector = DrawingHandler.Instance.GetDrawingSelector();
            var selected = selector.GetSelected();
            var count = 1;

            while (selected.MoveNext())
            {
                var curr = selected.Current;
                if (curr is not AssemblyDrawing) continue;
                var drawing = new DrawingModel(curr);
                SendToTeklaDialog($"Centering drawing {count} of {selected.GetSize()}...");
                count++;
                drawing.CenterDrawing(new FabViewCenteringStrategy());
            }
        });
        
        SendToTeklaDialog($"Done.");
        QuitRequested?.Invoke(this, false);
    }

    private async void ExecuteCenterAll(object obj)
    {
        CloseRequested?.Invoke(this, true);

        await Task.Run(() => {
            var selector = DrawingHandler.Instance.GetDrawingSelector();
            var selected = selector.GetSelected();
            var count = 1;

            while (selected.MoveNext())
            {
                var curr = selected.Current;
                if (curr is GADrawing or AssemblyDrawing )
                {
                    var drawing = new DrawingModel(curr);
                SendToTeklaDialog($"Centering drawing {count} of {selected.GetSize()}...");
                count++;
                    switch (curr)
                    {
                        case GADrawing:
                            drawing.CenterDrawing(new GaViewCenteringStrategy());
                            break;
                        case AssemblyDrawing:
                            drawing.CenterDrawing(new FabViewCenteringStrategy());
                            break;
                    }
                }
            }
        });
        
        SendToTeklaDialog($"Done.");
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
}