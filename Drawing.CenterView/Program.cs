using System;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace Drawing.CenterView;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var model = new Model();
        var drawingHandler = new DrawingHandler();
        if (!model.GetConnectionStatus()) return;

        if (drawingHandler.GetActiveDrawing() != null)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PluginForm());
        }
        else
        {
            QuickCenterClass.EntryPoint();
        }
    }
}