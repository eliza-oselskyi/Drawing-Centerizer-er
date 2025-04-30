using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using TSMO = Tekla.Structures.Model.Operations;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterView;

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

public class DrawingHandlerExtension
{
    private ViewHandler _viewHandler = new ViewHandler();
    public DrawingEnumerator Drawings;

    public DrawingHandlerExtension(DrawingEnumerator drawings)
    {
        Drawings = drawings;
    }

    public void CenterDriver()
    {
        var centerHandler = new ViewHandler();
        while (Drawings.MoveNext())
        {
            var drawingsCurrent = Drawings.Current;
            IView currentView = null;

            switch (drawingsCurrent)
            {
                case GADrawing when DrawingUtils.IsValidDrawingForCenter(drawingsCurrent):
                {
                    var allDrawingViews = drawingsCurrent.GetSheet().GetViews();
                    while (allDrawingViews.MoveNext())
                    {
                        var viewTypeDict = DrawingMethods.GetViewTypeDict((View)allDrawingViews.Current);
                        var viewTypeEnum = DrawingMethods.GetViewTypeEnum(viewTypeDict);

                        if (viewTypeEnum == GaViewType.None) continue;
                        currentView = new GaView((View)allDrawingViews.Current);
                    }

                    break;
                }
                case AssemblyDrawing:
                {
                    var view = drawingsCurrent.GetSheet().GetViews();
                    while (view.MoveNext())
                    {
                        if (view.Current.GetView().IsSheet) continue;
                        var fabView = new FabView((View)view.Current);
                        var viewTypeEnum = (GaViewType)fabView.GetViewTypeEnum(_viewHandler);
                        if (viewTypeEnum == GaViewType.None) continue;
                        currentView = fabView;
                    }

                    break;
                }
            }

            currentView?.Center(centerHandler);
        }
    }
}

public interface IViewVisitor
{
    public void CenterVisit(FabView view);
    public void CenterVisit(GaView view);
    public bool IsValidViewForCenterVisit(GaView view);
    public bool IsValidViewForCenterVisit(FabView view);
    public Enum GetViewTypeEnumVisit(FabView view);
}

interface IView
{
    void Center(IViewVisitor visitor);
    bool IsValidViewForCenter(IViewVisitor visitor);
    Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor);
    Enum GetViewTypeEnum(IViewVisitor visitor);
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

public class FabView : IView
{
    public FabView(View view)
    {
        View = view;
    }

    public View View { get; set; }

    public void Center(IViewVisitor visitor)
    {
        visitor.CenterVisit(this);
    }

    public bool IsValidViewForCenter(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Enum GetViewTypeEnum(IViewVisitor visitor)
    {
        return visitor.GetViewTypeEnumVisit(this);
    }
}

public class GaView : IView
{
    public GaView(View view)
    {
        View = view;
    }

    public View View { get; set; }

    public void Center(IViewVisitor visitor)
    {
        visitor.CenterVisit(this);
    }

    public bool IsValidViewForCenter(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Enum GetViewTypeEnum(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }
}