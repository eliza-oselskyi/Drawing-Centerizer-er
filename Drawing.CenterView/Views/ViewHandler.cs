using System;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;

namespace Drawing.CenterView.Views;

public class TestProgram
{
    private static readonly DrawingHandler _drawingHandler = new DrawingHandler();
    private static readonly DrawingSelector _drawingSelector = _drawingHandler.GetDrawingSelector();

    public static void TestEntryPoint()
    {
        var selectedDrawings = _drawingSelector.GetSelected();

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
        var drawingHandler = new DrawingHandlerExtension(_drawingSelector.GetSelected());
        drawingHandler.CenterDriver();
    }

    private static void PromptNoneSelected()
    {
        Console.WriteLine("Drawings none selected prompt");
        var drawingHandler = new DrawingHandlerExtension(_drawingHandler.GetDrawings());
        drawingHandler.CenterDriver();
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
