using System;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Common.Enums;

namespace Drawing.CenterViewWPF.Centering.Strategies;

public class GaViewCenteringStrategy : IViewCenteringStrategy
{
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
         case >= 2 and <= 24 :
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