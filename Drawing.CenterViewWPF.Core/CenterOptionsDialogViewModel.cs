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
using Tekla.Structures.Drawing.UI;

namespace Drawing.CenterViewWPF.Core;

/// <summary>
/// Represents the view model for the Center Options Dialog in the application.
/// Provides commands and properties to control the dialog's behavior and handle user interactions.
/// Implements <see cref="INotifyPropertyChanged"/> to support data binding.
/// </summary>
public class CenterOptionsDialogViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<bool> QuitRequested;
    public event EventHandler<bool> CloseRequested;
    private Tekla.Structures.Drawing.UI.Events _events;
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
    private readonly DrawingSelector _drawingSelector = DrawingHandler.Instance.GetDrawingSelector();
    
    public ICommand CenterAllCommand { get; set; }
    public ICommand CenterAbsolutelyAllCommand { get; set; }
    public ICommand CenterAllFabsCommand { get; set; }
    public ICommand CenterAllGaCommand { get; set; }
    public ICommand CenterFabCommand { get; set; }
    public ICommand CenterGaCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    /// <summary>
    /// Represents the view model for the Center Options Dialog in the application.
    /// Handles the business logic and user interactions for centering different types of drawings.
    /// </summary>
    /// <remarks>
    /// The view model provides commands for various centering actions, such as centering all drawings,
    /// specific types of drawings, or cancelling the operation. It maintains properties to determine
    /// the state of user selection and raises events when the dialog needs to close or quit.
    /// </remarks>
    /// <implements>
    /// Implements <see cref="INotifyPropertyChanged"/> to support data binding functionality
    /// for changes in properties.
    /// </implements>
    /// <events>
    /// Raises <see cref="QuitRequested"/> to indicate when the application should exit.
    /// Raises <see cref="CloseRequested"/> to signal the dialog to close.
    /// </events>
    public CenterOptionsDialogViewModel()
    {
        InitializeEvents();

        UpdateSelectionState();

        
        CenterAbsolutelyAllCommand = new RelayCommand(ExecuteCenterAbsolutelyAll, _ => true);
        CenterAllCommand = new RelayCommand(ExecuteCenterAll, _ => true);
        CenterAllFabsCommand = new RelayCommand(ExecuteCenterAllFabs, _ => true);
        CenterAllGaCommand = new RelayCommand(ExecuteCenterAllGa, _ => true);
        CenterFabCommand = new RelayCommand(ExecuteCenterFab, _ => true);
        CenterGaCommand = new RelayCommand(ExecuteCenterGa, _ => true);
        CancelCommand = new RelayCommand(ExecuteCancel, _ => true);
    }

    /// <summary>
    /// Initializes and registers events related to drawing operations within the Center Options Dialog.
    /// Manages event subscriptions for changes in drawing selection, drawing editor state,
    /// and document manager events.
    /// </summary>
    /// <remarks>
    /// This method sets up listeners for drawing-related events including selection changes,
    /// opening the drawing editor, and closing the document manager. It invokes relevant actions
    /// such as updating the selection state or triggering quit requests when these events occur.
    /// The events are registered through an instance of <see cref="Tekla.Structures.Drawing.UI.Events"/>.
    /// </remarks>
    /// <exceptions>
    /// Throws exceptions if event registrations fail or the <see cref="Tekla.Structures.Drawing.UI.Events"/>
    /// instance is not initialized properly.
    /// </exceptions>
    private void InitializeEvents()
    {
        _events = new Tekla.Structures.Drawing.UI.Events();
        _events.DrawingListSelectionChanged += DrawingSelector_SelectionChanged;
        _events.DrawingEditorOpened += () => QuitRequested?.Invoke(this, true);
        _events.DocumentManagerClosed += DrawingSelector_SelectionChanged;
        _events.Register();
    }

    /// <summary>
    /// Handles the event triggered when the drawing selector's selection changes.
    /// Updates the state of selected drawings based on the current selection in the drawing manager.
    /// </summary>
    /// <remarks>
    /// This method is registered as an event handler for selection change events in the Tekla Structures drawing interface.
    /// It updates the <see cref="HasSelectedDrawings"/> property, which is used to determine the availability of commands
    /// and user actions. The method relies on the <c>UpdateSelectionState</c> method for managing the selection state.
    /// </remarks>
    /// <events>
    /// Subscribed to <see cref="Tekla.Structures.Drawing.UI.Events.DrawingListSelectionChanged"/>
    /// to reflect changes in the selected drawings.
    /// Subscribed to <see cref="Tekla.Structures.Drawing.UI.Events.DocumentManagerClosed"/> to reset state when the document is closed.
    /// </events>
    private void DrawingSelector_SelectionChanged()
    {
        UpdateSelectionState();
    }

    /// <summary>
    /// Updates the selection state of the dialog based on the currently selected drawings.
    /// Determines whether any drawings are selected and sets the corresponding property.
    /// </summary>
    /// <remarks>
    /// This method retrieves the list of currently selected drawings using the <see cref="DrawingSelector"/>
    /// instance. It updates the <see cref="HasSelectedDrawings"/> property based on whether there are any
    /// selected drawings and ensures accurate state management for user interactions.
    /// </remarks>
    private void UpdateSelectionState()
    {
        var selected = _drawingSelector.GetSelected();
        HasSelectedDrawings = selected.GetSize() > 0 && selected.MoveNext();
        selected.Reset();
    }

    /// <summary>
    /// Contains the core logic for centering drawings using a specified filtering condition
    /// and view centering strategy. Allows optional restriction of the operation to selected drawings only.
    /// </summary>
    /// <param name="drawingFilter">
    /// A function that defines the filter criteria for determining which drawings should be centered.
    /// </param>
    /// <param name="strategy">
    /// An instance of <see cref="IViewCenteringStrategy"/> that dictates the centering behavior for the drawings.
    /// If null, an appropriate strategy will be determined internally.
    /// </param>
    /// <param name="useSelectedOnly">
    /// A boolean value indicating whether to apply the operation only to selected drawings. Defaults to false.
    /// </param>
    /// <returns>
    /// An asynchronous operation which centers the specified drawings and completes after all drawings are processed.
    /// </returns>
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

    /// <summary>
    /// Determines and returns the appropriate view centering strategy for the given drawing.
    /// </summary>
    /// <param name="drawing">The drawing for which the appropriate centering strategy is to be determined.</param>
    /// <returns>An instance of <see cref="IViewCenteringStrategy"/> that matches the type of the given drawing.</returns>
    /// <exception cref="ArgumentException">Thrown when the type of the given drawing is not supported.</exception>
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

    public void Dispose()
    {
        _events.UnRegister();
    }
}