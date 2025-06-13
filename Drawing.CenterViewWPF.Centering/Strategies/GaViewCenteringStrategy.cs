using System;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Strategies;

/// <summary>
///     Represents a strategy for centering General Arrangement (GA) views in a drawing application.
/// </summary>
/// <remarks>
///     This strategy is specifically designed to align the view origin and center the content
///     within a given sheet for General Arrangement drawings. It adjusts the view's position
///     based on the sheet dimensions and offsets determined by the view type.
/// </remarks>
public class GaViewCenteringStrategy : IViewCenteringStrategy
{
    /// <summary>
    ///     Centers the provided view based on the specified parameters.
    /// </summary>
    /// <param name="view">The view to be centered.</param>
    /// <param name="isGuiMode">Indicates whether the operation should be performed in GUI mode. Defaults to false.</param>
    /// <returns>Returns true if the centering operation was successful; otherwise, false.</returns>
    public bool Center(View view, bool isGuiMode = false)
    {
        var sheet = view.TeklaView.GetDrawing().GetSheet();
        double sheetHeightOffset = 0;
        var enumToInt = Convert.ChangeType(view.ViewType, view.ViewType.GetTypeCode());
        switch (enumToInt)
        {
            case 1:
                sheetHeightOffset = 25.4; // 1"
                break;
            case >= 2 and <= 24:
                sheetHeightOffset = 22.225; // 7/8"
                break;
            default:
                break;
        }

        sheet.Origin.Y = sheetHeightOffset;
        var originalOriginX = view.TeklaView.Origin.X;
        var originalOriginY = view.TeklaView.Origin.Y;
        view.TeklaView.Origin = sheet.Origin;
        if (!isGuiMode) view.TeklaView.Modify();
        var viewCenterPoint = view.TeklaView.GetAxisAlignedBoundingBox().GetCenterPoint();

        var sheetHeight = sheet.Height / 2;
        var sheetWidth = (sheet.Width - 33.274) / 2;
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