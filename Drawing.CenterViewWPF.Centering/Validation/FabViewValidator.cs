using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Validation;

public class FabViewValidator : IViewValidator
{
    public bool IsValid(View view)
    {
        return view.ViewType is not (Tekla.Structures.Drawing.View.ViewTypes.DetailView
            or Tekla.Structures.Drawing.View.ViewTypes.SectionView
            or Tekla.Structures.Drawing.View.ViewTypes.UnknownViewType);
    }
}