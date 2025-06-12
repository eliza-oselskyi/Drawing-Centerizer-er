using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.Strategies;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Core.Commands;
using Drawing.CenterViewWPF.Core.Configuration;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Core
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        
        public ICommand CenterViewCommand { get; set; }
        public ICommand ShiftViewCommand { get; set; }
        public event EventHandler<bool> QuitRequested;
        private Tekla.Structures.Drawing.UI.Events _events;

        private bool _isDarkMode;
        private bool _stayOpen;

        public MainWindowViewModel()
        {
            InitializeEvents();
            ConfigurationManager.LoadConfiguration();

            CenterViewCommand = new RelayCommand(ExecuteCenterView, _ => true);
            ShiftViewCommand = new RelayCommand(ExecuteShiftView, _ => true);
        }

        private void InitializeEvents()
        {
            _events = new Tekla.Structures.Drawing.UI.Events();
            _events.DrawingEditorClosed += () => QuitRequested?.Invoke(this, false);
            _events.Register();
        }

        private void ExecuteShiftView(object obj)
        {
            if (obj is not string direction) return;

            var drawing = DrawingHandler.Instance.GetActiveDrawing();
            if (drawing == null) return;
            var drawingModel = new DrawingModel(drawing);
            
            var views = drawingModel.GetViews();

            bool isBigShift = direction.StartsWith("big", StringComparison.OrdinalIgnoreCase);
            string actualDirection = isBigShift ? direction.Substring(3) : direction;

            var view = views.Where(v => v.IsValid).Select(v => v).ToList();
            view[0].Shift(actualDirection, isBigShift);
            view[0].TeklaView.Modify();
        }

        private void ExecuteCenterView(object obj)
        {
            try
            {
                var drawing = DrawingHandler.Instance.GetActiveDrawing();
                var drawingModel = new DrawingModel(drawing, true);
                IViewCenteringStrategy strategy;

                if (drawing is GADrawing)
                {
                    strategy = new GaViewCenteringStrategy();
                }
                else if (drawing is AssemblyDrawing)
                {
                    strategy = new FabViewCenteringStrategy();
                }
                else
                {
                    throw new InvalidTypeException($"drawing ({drawing.GetType().Name}) is not of a supported type");
                }

                drawingModel.CenterDrawing(strategy);
            }
            catch (Exception _)
            {
                Tekla.Structures.Model.Operations.Operation.DisplayPrompt($"Warning: No drawing is active. Cannot perform operation.");
            }
        }

        public bool ShowMainWindow()
        {
            return DrawingHandler.Instance.GetActiveDrawing() != null;
        }

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
        
        public void Dispose()
        {
            _events.UnRegister();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
}