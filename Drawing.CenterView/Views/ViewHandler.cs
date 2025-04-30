using System;
using System.Diagnostics;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.Model.Operations;

namespace Drawing.CenterView.Views;

public class TestProgram
{
    private static readonly DrawingHandler DrawingHandler = new DrawingHandler();
    private static readonly DrawingSelector DrawingSelector = DrawingHandler.GetDrawingSelector();

    public static void TestEntryPoint()
    {
        var selectedDrawings = DrawingSelector.GetSelected();

        if (selectedDrawings.GetSize() <= 0)
        {
            PromptNoneSelected();
        }
        else
        {
            PromptSelected();
        }
    }

    private static void PromptSelected()
    {
        Console.WriteLine("Drawings selected prompt");
        var drawingHandler = new DrawingHandlerExtension(DrawingSelector.GetSelected());
        var stopWatch = new Stopwatch();
        
            const string boxTitle = "Center All Drawings?";
            const string boxQuestion = "Should ONLY the selected drawings be centered?\n\n" +
                                       "  Yes = Center only selected\n" +
                                       "  No = Center all";
            var boxResult = MessageBox.Show(boxQuestion,
                boxTitle,
                MessageBoxButtons.YesNoCancel,
                0,
                0,
                MessageBoxOptions.DefaultDesktopOnly);

            switch (boxResult)
            {
                case DialogResult.Yes:
                    stopWatch.Start();
                    drawingHandler.CenterDriver();
                    break;
                case DialogResult.No:
                    stopWatch.Start();
                    PromptNoneSelected();
                    break;
                case DialogResult.Cancel:
                    Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Aborting.");
                    return;
                case DialogResult.None:
                case DialogResult.OK:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }  

    private static void PromptNoneSelected()
    {
        Console.WriteLine("Drawings none selected prompt");
        
        var drawingHandler = new DrawingHandlerExtension(DrawingHandler.GetDrawings());
        var stopWatch = new Stopwatch();
        
        DialogResult result =CustomMessageBox.ShowPrompt();
        
        //Console.WriteLine(result);
        
        drawingHandler.CenterDriver(result);

        if (false)
        {
            const string boxTitle = "Center All Drawings?";
            const string boxQuestion = "No drawings are currently selected.\n\n" +
                                       "  Yes = Center only erection  drawings\n" +
                                       "  No = Center all";
            var boxResult = MessageBox.Show(boxQuestion,
                boxTitle,
                MessageBoxButtons.YesNoCancel,
                0,
                0,
                MessageBoxOptions.DefaultDesktopOnly);
            
            

            switch (boxResult)
            {
                case DialogResult.OK:
                    stopWatch.Start();
                    drawingHandler.CenterDriver();
                    break;
                case DialogResult.Cancel:
                    Operation.DisplayPrompt("Aborting.");
                    return;
                case DialogResult.None:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                case DialogResult.Yes:
                case DialogResult.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

public class ViewHandler : IViewVisitor
{
    public void CenterVisit(FabView view)
    {
        Console.WriteLine(@"Centering FabView");
    }

    public void CenterVisit(GaView view)
    {
        var view2 = view.View.GetView();
        var dict = DrawingMethods.GetViewTypeDict(view2);
        var viewEnum = DrawingMethods.GetViewTypeEnum(dict);
        DrawingMethods.CenterView(view2, (int)viewEnum, out Tuple<Tekla.Structures.Drawing.Drawing, string> s);
        Console.WriteLine(@"Centering GaView");
    }

    public bool IsValidViewForCenterVisit(GaView view)
    {
        throw new NotImplementedException();
    }

    bool IViewVisitor.IsValidViewForCenterVisit(FabView view)
    {
        throw new NotImplementedException();
    }

    public Enum GetViewTypeEnumVisit(FabView view)
    {
        return GaViewType.None;
    }
}
