using System;
using System.Collections.Generic;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;
using View = Drawing.CenterViewWPF.Centering.TeklaWrapper.View;

namespace Drawing.CenterViewWPF.Centering.Strategies;

/// <summary>
/// Provides a centering strategy for Fab views in Tekla Structures drawings.
/// This class implements the IViewCenteringStrategy interface to adjust the
/// placement of views to align them relative to the center of the drawing sheet.
/// </summary>
public class FabViewCenteringStrategy : IViewCenteringStrategy
{
    private Vector GetDimensionVector(Tekla.Structures.Drawing.View view)
    {
        var allObjects = view.GetAllObjects();
        var dimensionList = new List<StraightDimension>();
        while (allObjects.MoveNext())
        {
            if (allObjects.Current is StraightDimension dimension)
            {
                dimensionList.Add(dimension);
            }
        }

        return dimensionList.Count == 1 ? dimensionList[0].UpDirection : new Vector();
    }

    /// Centers the given view on a drawing sheet by adjusting its origin and modifying its position.
    /// Optionally operates in GUI or non-GUI mode based on the provided parameter.
    /// <param name="view">The target view to be centered.</param>
    /// <param name="isGuiMode">Determines whether the centering is performed in GUI mode. Defaults to false.</param>
    /// <return>Returns true if the view was successfully centered, or false if no modifications were necessary.</return>
    public bool Center(View view, bool isGuiMode = false)
    {
        const double DIMENSION_LINE_HEIGHT = 72;
        const double TITLE_BOX_HEIGHT = 22.352; // 7/8"
        
        var sheet = view.TeklaView.GetDrawing().GetSheet();
        double sheetHeightOffset = TITLE_BOX_HEIGHT - DIMENSION_LINE_HEIGHT;
        sheet.Origin.Y = sheetHeightOffset;
        var originalOriginX = view.TeklaView.Origin.X;
        var originalOriginY = view.TeklaView.Origin.Y;
        view.TeklaView.Origin = sheet.Origin;
        if (!isGuiMode) view.TeklaView.Modify();
        // TODO: At some point, add collision detector if have multiple views on sheet
        var viewCenterPoint = view.TeklaView.GetAxisAlignedBoundingBox().GetCenterPoint();
        
        var sheetHeight = sheet.Height / 2;
        var sheetWidth = (sheet.Width) / 2;
        var xOffset = sheetWidth - viewCenterPoint.X;
        var yOffset = sheetHeight - viewCenterPoint.Y;

        if (Math.Abs(view.TeklaView.ExtremaCenter.X - sheetWidth) < 0.0001 &&
            Math.Abs(view.TeklaView.ExtremaCenter.Y - sheetHeight) < 0.0001)
        {
            Console.WriteLine("Nothing to do.");
        }
        else if (Math.Abs(view.TeklaView.ExtremaCenter.X - sheetWidth) > 0.0001 ||
                 Math.Abs(view.TeklaView.ExtremaCenter.Y - sheetHeight) > 0.0001)
        {
            Console.WriteLine($"Centering {view.ViewType}");
            view.TeklaView.Origin.X += xOffset;
            view.TeklaView.Origin.Y += yOffset;

            view.TeklaView.Modify();
            return true;
        }

        return false;
    }
}