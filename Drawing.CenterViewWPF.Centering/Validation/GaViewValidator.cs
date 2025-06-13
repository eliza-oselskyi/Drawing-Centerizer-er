using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Common.Enums;

namespace Drawing.CenterViewWPF.Centering.Validation;

/// <summary>
///     Represents a validator for General Arrangement (GA) views.
///     This class determines if a given view is a valid GA view by validating its view type.
/// </summary>
public class GaViewValidator : IViewValidator
{
    public bool IsValid(View view)
    {
        var viewType = (GaDrawingViewType)view.ViewType;
        return viewType != GaDrawingViewType.None;
    }
}