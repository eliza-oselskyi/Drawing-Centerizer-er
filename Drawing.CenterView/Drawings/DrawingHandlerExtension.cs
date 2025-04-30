using System;
using System.Windows.Forms;
using Drawing.CenterView.Views;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model.Operations;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterView;

public class DrawingHandlerExtension
{
    private ViewHandler _viewHandler = new ViewHandler();
    public DrawingEnumerator Drawings;

    public DrawingHandlerExtension(DrawingEnumerator drawings)
    {
        Drawings = drawings;
    }

    public void CenterDriver(DialogResult dialogResult)
    {
        switch (dialogResult)
        {
            case  DialogResult.OK:
                CenterDriver(null );
                break;
            case DialogResult.Yes:
                CenterDriver(true);
                break;
            case DialogResult.No:
                CenterDriver(false);
                break;
            case DialogResult.Cancel:
                Operation.DisplayPrompt("Aborting.");
                break;
        }
    }
    public void CenterDriver(bool? flag)
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
                    if (flag == true) continue; // True == fab only, false erection only
                    if (flag is not (null or false)) continue;
                    
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
                        if (flag == false) continue;
                        if (flag is not (null or true)) continue;
                        
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
