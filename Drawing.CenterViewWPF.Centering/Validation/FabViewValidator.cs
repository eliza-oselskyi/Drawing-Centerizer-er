using Drawing.CenterViewWPF.Centering.Interfaces;
using Tekla.Structures.Drawing;
using View = Drawing.CenterViewWPF.Centering.TeklaWrapper.View;

namespace Drawing.CenterViewWPF.Centering.Validation;

/// <summary>
///     Represents a validator for fabrication (Fab) views in a drawing.
///     This class checks whether a given view meets the criteria to be classified as a valid Fab view.
/// </summary>
public class FabViewValidator : IViewValidator
{
    public bool IsValid(View view)
    {
        if (view.TeklaView is ContainerView && !view.TeklaView.IsSheet)
            // probably should just return false here
            return true;
        else
            return view.ViewType is not (Tekla.Structures.Drawing.View.ViewTypes.DetailView
                or Tekla.Structures.Drawing.View.ViewTypes.SectionView
                or Tekla.Structures.Drawing.View.ViewTypes.UnknownViewType);
    }
}