using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Common.Enums;

namespace Drawing.CenterViewWPF.Centering.Validation;


public class GaViewValidator : IViewValidator
{
    public bool IsValid(View view)
    {
        var viewType = (GaDrawingViewType)view.ViewType;
        return viewType != GaDrawingViewType.None;
    }
}