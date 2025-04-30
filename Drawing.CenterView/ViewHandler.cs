using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using TSMO = Tekla.Structures.Model.Operations;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterView;

public class TestProgram
{
    private static readonly DrawingHandler _drawingHandler =  new DrawingHandler();
    public static void TestEntryPoint()
    {
        var  drawingSelector = _drawingHandler.GetDrawingSelector();
        var selectedDrawings = drawingSelector.GetSelected();
        var centerHandler = new ViewHandler();

        if (selectedDrawings.GetSize() <= 0)
        {
            Console.WriteLine("Nothing selected");
        }
        else
        {
            while (selectedDrawings.MoveNext())
            {
                var current = selectedDrawings.Current;
                IView currentDrawing = null;
                if (current is GADrawing)
                {
                    var view = current.GetSheet().GetViews();
                    currentDrawing = new GaView((View)view.Current);
                }
                else if (current is AssemblyDrawing)
                {
                    var view = current.GetSheet().GetViews();
                    currentDrawing = new FabView((View)view.Current);
                }

                currentDrawing?.Center(centerHandler);
            }
        }

    }
}

public interface IViewVisitor
{
    public void Visit(FabView view);
    public void Visit(GaView view);
    
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
    public void Visit(FabView view)
    {
        Console.WriteLine(@"Centering FabView");
    }

    public void Visit(GaView view)
    {
        Console.WriteLine(@"Centering GaView");
    }
}

public class FabView : IView
{
    public FabView(View view)
    {
        View = view;
    }

    public View  View { get; set; }
    public void Center(IViewVisitor visitor)
    {
        visitor.Visit(this);
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

public class GaView : IView
{
    public GaView(View view)
    {
        View = view;
    }

    public View  View { get; set; }
    public void Center(IViewVisitor visitor)
    {
        visitor.Visit(this);
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
