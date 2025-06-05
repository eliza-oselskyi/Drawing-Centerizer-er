using System;
using System.Windows.Input;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.Strategies;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Core.Commands;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Core
{
    public class MainWindowViewModel
    {
        
        public ICommand CenterViewCommand { get; set; }
        public ICommand ShiftViewCommand { get; set; }

        public MainWindowViewModel()
        {
            CenterViewCommand = new RelayCommand(ExecuteCenterView, _ => true);
            ShiftViewCommand = new RelayCommand(ExecuteShiftView, _ => true);
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
            
            views[0].Shift(actualDirection, isBigShift);
            views[0].TeklaView.Modify();
        }

        private void ExecuteCenterView(object obj)
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

        public bool ShowMainWindow()
        {
            return DrawingHandler.Instance.GetActiveDrawing() != null;
        }
    }
}