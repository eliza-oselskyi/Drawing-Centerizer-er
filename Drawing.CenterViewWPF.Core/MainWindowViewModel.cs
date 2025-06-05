using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Core
{
    public class MainWindowViewModel
    {
        public bool ShowMainWindow()
        {
            return DrawingHandler.Instance.GetActiveDrawing() != null;
        }
    }
}