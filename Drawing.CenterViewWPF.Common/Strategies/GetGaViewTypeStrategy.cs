using System;
using Drawing.CenterViewWPF.Common.Enums;
using Drawing.CenterViewWPF.Common.Interfaces;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Strategies;

/// <summary>
///     Represents a strategy class for identifying the view type of General Arrangement (GA) drawings.
/// </summary>
/// <remarks>
///     This class implements the <see cref="IGetViewTypeStrategy" /> interface. It provides a method to determine
///     the type of view based on the "ViewType" user property of the given Tekla drawing view.
///     For invalid or missing view type descriptions, it defaults the type to <see cref="GaDrawingViewType.None" />.
/// </remarks>
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