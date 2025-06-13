using System;
using Drawing.CenterViewWPF.Common.Interfaces;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Strategies;

/// <summary>
/// Implements a strategy for determining the fabrication view type of a drawing view.
/// </summary>
/// <remarks>
/// This class is specifically tailored for fabrication (assembly) drawings, as indicated by its connection
/// to the <see cref="DrawingType.Assembly" /> in the corresponding context. It determines the view type of
/// a given <see cref="ViewBase" /> and provides an associated type string.
/// </remarks>
public class GetFabViewTypeStrategy : IGetViewTypeStrategy
{
    public Enum GetViewType(ViewBase view, out string typeString)
    {
        if (!view.IsSheet && view is ContainerView)
        {
            typeString = "A";
            return View.ViewTypes.TopView;
        }
        else {
            typeString = "A";
            return ((View)view).ViewType;
        }
    }
}