using System;
using Drawing.CenterViewWPF.Common.Interfaces;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Strategies;

public class GetFabViewTypeStrategy : IGetViewTypeStrategy
{
    public Enum GetViewType(View view, out string typeString)
    {
        typeString = "A";
        return view.ViewType;
    }
}