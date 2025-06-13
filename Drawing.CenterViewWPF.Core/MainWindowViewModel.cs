using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.Strategies;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Core.Commands;
using Drawing.CenterViewWPF.Core.Configuration;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model.Operations;
using Events = Tekla.Structures.Drawing.UI.Events;

namespace Drawing.CenterViewWPF.Core;

/// <summary>
///     Represents the ViewModel for the main window in the application.
///     Implements INotifyPropertyChanged for property change notifications.
/// </summary>
public class MainWindowViewModel : INotifyPropertyChanged
{
    private Events _events;

    private bool _isDarkMode;
    private bool _stayOpen;

    /// <summary>
    ///     Represents the ViewModel for the main application window in the Drawing.CenterViewWPF project.
    ///     Handles commands, configuration loading, event initialization, and interaction logic.
    ///     Implements the INotifyPropertyChanged interface to provide property change notifications.
    /// </summary>
    public MainWindowViewModel()
    {
        InitializeEvents();
        LoadConfiguration();

        CenterViewCommand = new RelayCommand(ExecuteCenterView, _ => true);
        ShiftViewCommand = new RelayCommand(ExecuteShiftView, _ => true);
    }

    public ICommand CenterViewCommand { get; set; }
    public ICommand ShiftViewCommand { get; set; }

    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                ConfigurationManager.Current.IsDarkMode = value;
                OnPropertyChanged();
            }
        }
    }

    public bool StayOpen
    {
        get => _stayOpen;
        set
        {
            if (_stayOpen != value)
            {
                _stayOpen = value;
                ConfigurationManager.Current.StayOpen = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<bool> QuitRequested;

    /// <summary>
    ///     Loads application configuration settings using the ConfigurationManager class.
    ///     Initializes property values such as IsDarkMode and StayOpen based on the loaded configuration.
    /// </summary>
    private void LoadConfiguration()
    {
        ConfigurationManager.LoadConfiguration();
        _isDarkMode = ConfigurationManager.Current.IsDarkMode;
        _stayOpen = ConfigurationManager.Current.StayOpen;
    }

    /// <summary>
    ///     Initializes event handlers related to the Tekla Structures drawing editor.
    ///     Registers the events and attaches custom logic to handle specific events such as when the drawing editor is closed.
    ///     Ensures proper event management for application behavior.
    /// </summary>
    private void InitializeEvents()
    {
        _events = new Events();
        _events.DrawingEditorClosed += () =>
        {
            if (!StayOpen) QuitRequested?.Invoke(this, false);
        };
        _events.Register();
    }

    /// <summary>
    ///     Executes the logic to shift the current active drawing view in a specified direction.
    ///     The shift can be a large or small movement depending on the specified direction string.
    /// </summary>
    /// <param name="obj">
    ///     Represents the direction of the shift as a string.
    ///     Prefix "big" in the direction string indicates a larger shift distance.
    /// </param>
    private void ExecuteShiftView(object obj)
    {
        if (obj is not string direction) return;

        var drawing = DrawingHandler.Instance.GetActiveDrawing();
        if (drawing == null) return;
        var drawingModel = new DrawingModel(drawing);

        var views = drawingModel.GetViews();

        var isBigShift = direction.StartsWith("big", StringComparison.OrdinalIgnoreCase);
        var actualDirection = isBigShift ? direction.Substring(3) : direction;

        var view = views.Where(v => v.IsValid).Select(v => v).ToList();
        view[0].Shift(actualDirection, isBigShift);
        view[0].TeklaView.Modify();
    }

    /// <summary>
    ///     Executes the logic to center the view for an active drawing in Tekla Structures.
    ///     Selects the appropriate centering strategy based on the type of the active drawing
    ///     (e.g., General Arrangement or Assembly drawing) and performs the centering operation.
    ///     Displays a warning if no active drawing is found.
    /// </summary>
    /// <param name="obj">An optional parameter passed by the command, which is not utilized in the implementation.</param>
    private void ExecuteCenterView(object obj)
    {
        try
        {
            var drawing = DrawingHandler.Instance.GetActiveDrawing();
            var drawingModel = new DrawingModel(drawing, true);
            IViewCenteringStrategy strategy;

            if (drawing is GADrawing)
                strategy = new GaViewCenteringStrategy();
            else if (drawing is AssemblyDrawing)
                strategy = new FabViewCenteringStrategy();
            else
                throw new InvalidTypeException($"drawing ({drawing.GetType().Name}) is not of a supported type");

            drawingModel.CenterDrawing(strategy);
        }
        catch (Exception _)
        {
            Operation.DisplayPrompt($"Warning: No drawing is active. Cannot perform operation.");
        }
    }

    /// <summary>
    ///     Determines whether the main window should be displayed based on the active drawing status.
    ///     Returns true if there is an active drawing in the application that can be handled; otherwise, false.
    /// </summary>
    /// <returns>A boolean indicating whether the main window should be displayed.</returns>
    public bool ShowMainWindow()
    {
        return DrawingHandler.Instance.GetActiveDrawing() != null;
    }

    public void Dispose()
    {
        _events.UnRegister();
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