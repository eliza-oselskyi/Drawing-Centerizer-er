using System;
using Drawing.CenterViewWPF.Common.Interfaces;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Strategies;

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