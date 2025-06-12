using System;
using Drawing.CenterViewWPF.Common.Enums;
using Drawing.CenterViewWPF.Common.Interfaces;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Strategies;
    
public class GetGaViewTypeStrategy : IGetViewTypeStrategy
{
    public Enum GetViewType(ViewBase view, out string typeString)
    {
        typeString = "G";
        var viewType = string.Empty;
        view.GetUserProperty("ViewType", ref viewType);
        try
        {
            return Utility.GetValueFromDescription<GaDrawingViewType>(viewType);
        }
        catch (Exception _)
        {
            return GaDrawingViewType.None;
        }
    }
}